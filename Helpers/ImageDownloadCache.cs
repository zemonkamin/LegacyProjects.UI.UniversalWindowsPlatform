using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Helpers
{
    public static class ImageDecodeScheduler
    {
        private static readonly SemaphoreSlim Slots = new SemaphoreSlim(
            Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile" ? 1 : 2);

        public static async Task<T> RunAsync<T>(Func<Task<T>> decode, CancellationToken cancellation)
        {
            await Slots.WaitAsync(cancellation).ConfigureAwait(false);
            try
            {
                cancellation.ThrowIfCancellationRequested();
                return await Task.Run(decode).ConfigureAwait(false);
            }
            finally { Slots.Release(); }
        }
    }

    /// <summary>Bounded on-disk image cache shared by page thumbnails and PhotoViewer.</summary>
    public static class ImageDownloadCache
    {
        private static readonly bool Mobile = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile";
        private static readonly HttpClient Client = new HttpClient(new HttpBaseProtocolFilter { CookieUsageBehavior = HttpCookieUsageBehavior.NoCookies });
        private static readonly SemaphoreSlim Downloads = new SemaphoreSlim(Mobile ? 2 : 4);
        private static readonly SharedRequestPool<string> Requests = new SharedRequestPool<string>();
        private static readonly SemaphoreSlim Maintenance = new SemaphoreSlim(1);
        private static DateTime _lastTrim;
        private static long _bytesSinceTrim;
        private static string _directory;

        public static async Task<IRandomAccessStream> OpenAsync(Uri uri, uint maximumBytes, CancellationToken cancellation,
            Action<HttpRequestMessage> configureRequest = null)
        {
            if (uri == null || !uri.IsAbsoluteUri || (uri.Scheme != "http" && uri.Scheme != "https"))
                throw new ArgumentException("Invalid image URI.");
            // File I/O, HTTP continuations and hashing never run on the UI thread.
            string path = await Requests.RunAsync(uri.AbsoluteUri + "|" + maximumBytes,
                token => Task.Run(() => GetFileAsync(uri, maximumBytes, token, configureRequest)), cancellation).ConfigureAwait(false);
            cancellation.ThrowIfCancellationRequested();
            var file = await StorageFile.GetFileFromPathAsync(path).AsTask(cancellation).ConfigureAwait(false);
            IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read).AsTask(cancellation).ConfigureAwait(false);
            if (stream.Size > maximumBytes) { stream.Dispose(); throw new IOException("Image too large."); }
            return stream;
        }

        private static async Task<string> GetFileAsync(Uri uri, uint maximumBytes, CancellationToken cancellation,
            Action<HttpRequestMessage> configureRequest)
        {
            string directory = _directory;
            if (directory == null)
            {
                directory = Path.Combine(ApplicationData.Current.LocalCacheFolder.Path, "PhotoImages-v1");
                Directory.CreateDirectory(directory);
                _directory = directory;
            }
            string hash = CryptographicBuffer.EncodeToHexString(HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256)
                .HashData(CryptographicBuffer.ConvertStringToBinary(uri.AbsoluteUri, BinaryStringEncoding.Utf8)));
            string path = Path.Combine(directory, hash + ".image");
            if (Fresh(path, maximumBytes)) return path;
            await Downloads.WaitAsync(cancellation).ConfigureAwait(false);
            string temporary = path + "." + Guid.NewGuid().ToString("N") + ".part";
            try
            {
                if (Fresh(path, maximumBytes)) return path;
                using (var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellation))
                using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
                {
                    timeout.CancelAfter(TimeSpan.FromSeconds(maximumBytes > 8u * 1024 * 1024 ? 30 : 15));
                    if (configureRequest != null) configureRequest(request);
                    using (HttpResponseMessage response = await Client.SendRequestAsync(request, HttpCompletionOption.ResponseHeadersRead).AsTask(timeout.Token).ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                        if (response.Content.Headers.ContentLength > maximumBytes) throw new IOException("Image too large.");
                        string mime = response.Content.Headers.ContentType == null ? string.Empty : response.Content.Headers.ContentType.MediaType;
                        if (mime.IndexOf("html", StringComparison.OrdinalIgnoreCase) >= 0) throw new IOException("The server returned HTML instead of an image.");
                        using (IInputStream input = await response.Content.ReadAsInputStreamAsync().AsTask(timeout.Token).ConfigureAwait(false))
                        using (Stream stream = input.AsStreamForRead())
                        using (var output = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None, 65536, true))
                        {
                            byte[] buffer = new byte[65536];
                            int read; long length = 0;
                            while ((read = await stream.ReadAsync(buffer, 0, buffer.Length, timeout.Token).ConfigureAwait(false)) > 0)
                            {
                                length += read;
                                if (length > maximumBytes) throw new IOException("Image too large.");
                                await output.WriteAsync(buffer, 0, read, timeout.Token).ConfigureAwait(false);
                            }
                            if (length < 8) throw new IOException("Empty image.");
                        }
                    }
                }
                cancellation.ThrowIfCancellationRequested();
                if (!HasImageHeader(temporary)) throw new IOException("Invalid image header.");
                // A reader may still hold the stale file. Do not replace a file in use.
                if (File.Exists(path))
                {
                    try { File.Delete(path); } catch (IOException) { return path; }
                }
                try { File.Move(temporary, path); }
                catch (IOException) { if (!Fresh(path, maximumBytes)) throw; }
                Interlocked.Add(ref _bytesSinceTrim, new FileInfo(path).Length);
                await TrimAsync(directory, path).ConfigureAwait(false);
                return path;
            }
            finally
            {
                try { if (File.Exists(temporary)) File.Delete(temporary); } catch { }
                Downloads.Release();
            }
        }

        private static bool Fresh(string path, uint maximumBytes)
        {
            var file = new FileInfo(path);
            return file.Exists && file.Length >= 8 && file.Length <= maximumBytes
                && DateTime.UtcNow - file.LastWriteTimeUtc < TimeSpan.FromHours(24);
        }

        private static bool HasImageHeader(string path)
        {
            byte[] data = new byte[12];
            using (var stream = File.OpenRead(path)) if (stream.Read(data, 0, data.Length) < 8) return false;
            return (data[0] == 0xff && data[1] == 0xd8 && data[2] == 0xff)
                || (data[0] == 0x89 && data[1] == 0x50 && data[2] == 0x4e && data[3] == 0x47 && data[4] == 13 && data[5] == 10 && data[6] == 26 && data[7] == 10)
                || (data[0] == 0x47 && data[1] == 0x49 && data[2] == 0x46 && data[3] == 0x38 && (data[4] == 0x37 || data[4] == 0x39) && data[5] == 0x61)
                || (data[0] == 0x42 && data[1] == 0x4d)
                || (data[0] == 0x52 && data[1] == 0x49 && data[2] == 0x46 && data[3] == 0x46 && data[8] == 0x57 && data[9] == 0x45 && data[10] == 0x42 && data[11] == 0x50)
                || (data[0] == 0 && data[1] == 0 && (data[2] == 1 || data[2] == 2) && data[3] == 0)
                || (data[0] == 0x49 && data[1] == 0x49 && data[2] == 42 && data[3] == 0)
                || (data[0] == 0x4d && data[1] == 0x4d && data[2] == 0 && data[3] == 42);
        }

        private static async Task TrimAsync(string directory, string keep)
        {
            if (!await Maintenance.WaitAsync(0).ConfigureAwait(false)) return;
            try
            {
                if (DateTime.UtcNow - _lastTrim < TimeSpan.FromMinutes(1)
                    && Interlocked.Read(ref _bytesSinceTrim) < (Mobile ? 2L : 8L) * 1024 * 1024) return;
                _lastTrim = DateTime.UtcNow;
                Interlocked.Exchange(ref _bytesSinceTrim, 0);
                var files = new DirectoryInfo(directory).GetFiles().OrderByDescending(file => file.LastWriteTimeUtc).ToArray();
                long total = 0, budget = (Mobile ? 32L : 128L) * 1024 * 1024;
                foreach (FileInfo file in files)
                {
                    if (file.Extension == ".part")
                    {
                        if (DateTime.UtcNow - file.LastWriteTimeUtc < TimeSpan.FromHours(1)) continue;
                    }
                    else
                    {
                        total += file.Length;
                        if (file.FullName == keep || (total <= budget && DateTime.UtcNow - file.LastWriteTimeUtc < TimeSpan.FromHours(24))) continue;
                    }
                    try { file.Delete(); } catch { }
                }
            }
            catch { }
            finally { Maintenance.Release(); }
        }
    }
}
