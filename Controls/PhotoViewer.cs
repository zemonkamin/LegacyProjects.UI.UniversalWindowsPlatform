using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using LegacyProjects.UI.UniversalWindowsPlatform.Helpers;
using LegacyProjects.UI.UniversalWindowsPlatform.Models;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;
using Windows.Web.Http;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Controls
{
    // Extracted from WinArchive AppPage's screenshot overlay. No application-specific dependencies.
    public sealed class PhotoViewer : UserControl
    {
        private static PhotoViewer _current;
        private readonly Popup _popup = new Popup { IsLightDismissEnabled = false };
        private readonly Grid _content = new Grid();
        private readonly FlipView _photos = new FlipView();
        private readonly TextBlock _counter = new TextBlock();
        private readonly TextBlock _status = new TextBlock();
        private readonly StackPanel _indicators = new StackPanel { Orientation = Orientation.Horizontal };
        private readonly List<PhotoPage> _pages = new List<PhotoPage>();
        private readonly Dictionary<PhotoViewerItem, ResolvedPhoto> _resolved = new Dictionary<PhotoViewerItem, ResolvedPhoto>();
        private readonly PhotoViewerOptions _options;
        private readonly CancellationTokenSource _lifetime = new CancellationTokenSource();
        private readonly TaskCompletionSource<bool> _completion = new TaskCompletionSource<bool>();
        private readonly AppBarButton _save = new AppBarButton();
        private readonly AppBarButton _share = new AppBarButton();
        private readonly Button _close = new Button();
        private readonly bool _mobile = Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile";
        private CancellationTokenSource _imageLoad;
        private DataTransferManager _shareManager;
        private ApplicationView _applicationView;
        private StorageFile _shareFile, _downloadedFile;
        private PhotoViewerItem _downloadedItem;
        private Control _previousFocus;
        private int _generation;
        private bool _closing, _busy;

        public static bool IsOpen { get { return _current != null; } }
        public static bool TryCloseCurrent()
        {
            if (_current == null) return false;
            _current.Close(); return true;
        }

        public static async Task ShowAsync(IEnumerable<PhotoViewerItem> items, int selectedIndex = 0, PhotoViewerOptions options = null)
        {
            var valid = new List<PhotoViewerItem>();
            int originalIndex = 0, mappedIndex = -1;
            if (items != null)
                foreach (PhotoViewerItem item in items)
                {
                    Uri uri;
                    if (item != null && Uri.TryCreate(item.Url, UriKind.Absolute, out uri) && (uri.Scheme == "http" || uri.Scheme == "https"))
                    {
                        if (originalIndex == selectedIndex) mappedIndex = valid.Count;
                        valid.Add(new PhotoViewerItem { Url = item.Url, Title = item.Title, FileName = item.FileName, Preview = item.Preview });
                    }
                    originalIndex++;
                }
            if (valid.Count == 0) return;
            TryCloseCurrent();
            PhotoViewer viewer = null;
            bool failed = false;
            try
            {
                viewer = new PhotoViewer(valid, options ?? new PhotoViewerOptions());
                _current = viewer;
                viewer.Open(mappedIndex >= 0 ? mappedIndex : PhotoViewerPolicy.Index(selectedIndex, valid.Count));
                await viewer._completion.Task;
            }
            catch { if (viewer != null) viewer.Close(); failed = true; }
            if (failed) await new Windows.UI.Popups.MessageDialog("Не удалось открыть просмотрщик фотографий.").ShowAsync();
        }

        private PhotoViewer(List<PhotoViewerItem> items, PhotoViewerOptions options)
        {
            _options = options;
            RequestedTheme = ElementTheme.Dark;
            foreach (PhotoViewerItem item in items) _pages.Add(new PhotoPage { Item = item });
            var root = new Grid { Background = new SolidColorBrush(Color.FromArgb(238, 0, 0, 0)) };
            Content = root;
            root.Children.Add(_content);
            root.Tapped += (s, e) => { if (ReferenceEquals(e.OriginalSource, root)) { e.Handled = true; Close(); } };
            _popup.Child = this;
            _photos.Background = new SolidColorBrush(Colors.Transparent);
            _photos.Margin = new Thickness(0, 64, 0, 52);
            _photos.ItemTemplate = (DataTemplate)XamlReader.Load(@"<DataTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>
                <ScrollViewer HorizontalContentAlignment='Center' VerticalContentAlignment='Center' ZoomMode='Enabled'
                    MinZoomFactor='1' MaxZoomFactor='5' IsZoomChainingEnabled='False' IsScrollInertiaEnabled='True'
                    HorizontalScrollMode='Auto' VerticalScrollMode='Auto' HorizontalScrollBarVisibility='Hidden' VerticalScrollBarVisibility='Hidden'>
                    <Grid Width='{Binding ViewWidth}' Height='{Binding ViewHeight}' Background='Transparent'>
                        <Image Source='{Binding Source}' Stretch='Uniform' HorizontalAlignment='Stretch' VerticalAlignment='Stretch'/>
                        <ProgressRing IsActive='{Binding Loading}' Width='36' Height='36' HorizontalAlignment='Center' VerticalAlignment='Center'/>
                    </Grid>
                </ScrollViewer></DataTemplate>");
            _photos.SelectionChanged += Photos_SelectionChanged;
            _photos.DoubleTapped += Photos_DoubleTapped;
            _photos.Tapped += Photos_Tapped;
            _content.Children.Add(_photos);
            _close.Width = _close.Height = 44;
            _close.Margin = new Thickness(20);
            _close.HorizontalAlignment = HorizontalAlignment.Left; _close.VerticalAlignment = VerticalAlignment.Top;
            _close.Background = new SolidColorBrush(Color.FromArgb(204, 0, 0, 0));
            _close.Foreground = new SolidColorBrush(Colors.White); _close.BorderThickness = new Thickness(0);
            _close.Content = new FontIcon { Glyph = "\uE711", FontSize = 20 };
            _close.Template = (ControlTemplate)XamlReader.Load(@"<ControlTemplate xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' TargetType='Button'>
                <Grid><Ellipse Width='44' Height='44' Fill='{TemplateBinding Background}'/><ContentPresenter Content='{TemplateBinding Content}' HorizontalAlignment='Center' VerticalAlignment='Center'/></Grid></ControlTemplate>");
            AutomationProperties.SetName(_close, "Закрыть фотографию");
            _close.Click += delegate { Close(); };
            _content.Children.Add(_close);
            _counter.Margin = new Thickness(76, 28, 20, 0); _counter.HorizontalAlignment = HorizontalAlignment.Right;
            _counter.VerticalAlignment = VerticalAlignment.Top; _counter.Foreground = new SolidColorBrush(Colors.LightGray);
            _content.Children.Add(_counter);
            _indicators.HorizontalAlignment = HorizontalAlignment.Center; _indicators.VerticalAlignment = VerticalAlignment.Bottom;
            _indicators.Margin = new Thickness(0, 0, 0, 66); _content.Children.Add(_indicators);
            _status.Margin = new Thickness(20, 0, 20, 108); _status.VerticalAlignment = VerticalAlignment.Bottom;
            _status.HorizontalAlignment = HorizontalAlignment.Center; _status.TextWrapping = TextWrapping.Wrap;
            _status.Foreground = new SolidColorBrush(Colors.White); _content.Children.Add(_status);
            var bar = new CommandBar { VerticalAlignment = VerticalAlignment.Bottom, DefaultLabelPosition = CommandBarDefaultLabelPosition.Right,
                Background = new SolidColorBrush(Color.FromArgb(230, 26, 29, 26)) };
            _save.Label = options.SaveLabel ?? "Сохранить"; _save.Icon = new FontIcon { Glyph = "\uE896" };
            _share.Label = options.ShareLabel ?? "Поделиться"; _share.Icon = new FontIcon { Glyph = "\uE72D" };
            _save.Foreground = _share.Foreground = new SolidColorBrush(Colors.White);
            _save.Click += Save_Click; _share.Click += Share_Click;
            bar.PrimaryCommands.Add(_save); bar.PrimaryCommands.Add(_share); _content.Children.Add(bar);
        }

        private void Open(int index)
        {
            _previousFocus = FocusManager.GetFocusedElement() as Control;
            Window.Current.SizeChanged += Window_SizeChanged;
            _applicationView = ApplicationView.GetForCurrentView();
            _applicationView.VisibleBoundsChanged += View_VisibleBoundsChanged;
            Window.Current.CoreWindow.KeyDown += Window_KeyDown;
            SystemNavigationManager.GetForCurrentView().BackRequested += BackRequested;
            _shareManager = DataTransferManager.GetForCurrentView();
            _shareManager.DataRequested += Share_DataRequested;
            Resize();
            _popup.IsOpen = true;
            // Show the host before binding the FlipView (the source overlay's zero-size fix).
            UpdateLayout();
            _photos.SelectionChanged -= Photos_SelectionChanged;
            _photos.ItemsSource = _pages; _photos.SelectedIndex = index;
            _photos.SelectionChanged += Photos_SelectionChanged;
            Photos_SelectionChanged(_photos, null);
            _close.Focus(FocusState.Programmatic);
        }

        public void Close()
        {
            if (_closing) return;
            _closing = true; _generation++;
            _lifetime.Cancel();
            if (_imageLoad != null) _imageLoad.Cancel();
            Window.Current.SizeChanged -= Window_SizeChanged;
            if (_applicationView != null)
            {
                _applicationView.VisibleBoundsChanged -= View_VisibleBoundsChanged;
                _applicationView = null;
            }
            Window.Current.CoreWindow.KeyDown -= Window_KeyDown;
            SystemNavigationManager.GetForCurrentView().BackRequested -= BackRequested;
            if (_shareManager != null) _shareManager.DataRequested -= Share_DataRequested;
            _photos.ItemsSource = null; _indicators.Children.Clear();
            foreach (PhotoPage page in _pages) { page.Source = null; page.Item.Preview = null; }
            _pages.Clear(); _resolved.Clear(); _shareFile = _downloadedFile = null; _downloadedItem = null;
            _popup.IsOpen = false; _popup.Child = null;
            if (_current == this) _current = null;
            if (_previousFocus != null) { _previousFocus.Focus(FocusState.Programmatic); _previousFocus = null; }
            _completion.TrySetResult(true);
        }

        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs args) { Resize(); }
        private void View_VisibleBoundsChanged(ApplicationView sender, object args) { Resize(); }
        private void Resize()
        {
            if (_closing) return;
            Rect bounds = Window.Current.Bounds;
            Width = bounds.Width; Height = bounds.Height;
            // Popup fills the CoreWindow, including areas covered by Mobile system bars.
            // Keep the backdrop full size, but place all content in the unoccluded area.
            Rect visible = _applicationView == null ? bounds : _applicationView.VisibleBounds;
            visible.Intersect(bounds);
            if (visible.IsEmpty || visible.Width <= 0 || visible.Height <= 0) visible = bounds;
            _content.Margin = new Thickness(Math.Max(0, visible.Left - bounds.Left), Math.Max(0, visible.Top - bounds.Top),
                Math.Max(0, bounds.Right - visible.Right), Math.Max(0, bounds.Bottom - visible.Bottom));
            foreach (PhotoPage page in _pages)
            {
                page.ViewWidth = Math.Max(1, visible.Width);
                page.ViewHeight = Math.Max(1, visible.Height - _photos.Margin.Top - _photos.Margin.Bottom);
            }
        }
        private void Window_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Escape) { args.Handled = true; Close(); }
            else if (args.VirtualKey == VirtualKey.Left || args.VirtualKey == VirtualKey.Right)
            {
                ScrollViewer scroll = SelectedScroll();
                if (scroll != null && scroll.ZoomFactor > 1.01) return;
                _photos.SelectedIndex = PhotoViewerPolicy.Index(_photos.SelectedIndex + (args.VirtualKey == VirtualKey.Left ? -1 : 1), _pages.Count);
                args.Handled = true;
            }
        }
        private void BackRequested(object sender, BackRequestedEventArgs args) { if (!args.Handled) { args.Handled = true; Close(); } }
        private ScrollViewer SelectedScroll()
        {
            var container = _photos.ContainerFromIndex(_photos.SelectedIndex) as FlipViewItem;
            return container == null ? null : container.ContentTemplateRoot as ScrollViewer;
        }
        private void Photos_DoubleTapped(object sender, DoubleTappedRoutedEventArgs args)
        {
            ScrollViewer scroll = SelectedScroll();
            if (scroll == null) return;
            float zoom = scroll.ZoomFactor > 1.01 ? 1 : 2.5f;
            Point point = args.GetPosition(scroll);
            scroll.ChangeView(zoom == 1 ? 0 : Math.Max(0, point.X * (zoom - 1)), zoom == 1 ? 0 : Math.Max(0, point.Y * (zoom - 1)), zoom);
            args.Handled = true;
        }
        private void Photos_Tapped(object sender, TappedRoutedEventArgs args)
        {
            ScrollViewer scroll = SelectedScroll();
            if (scroll != null && scroll.ZoomFactor > 1.01) return;
            Image image = args.OriginalSource as Image;
            BitmapSource source = image == null ? null : image.Source as BitmapSource;
            if (source == null) return;
            double scale = Math.Min(image.ActualWidth / source.PixelWidth, image.ActualHeight / source.PixelHeight);
            var photo = new Rect((image.ActualWidth - source.PixelWidth * scale) / 2,
                (image.ActualHeight - source.PixelHeight * scale) / 2, source.PixelWidth * scale, source.PixelHeight * scale);
            if (!photo.Contains(args.GetPosition(image))) { args.Handled = true; Close(); }
        }
        private async void Photos_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            int index = _photos.SelectedIndex;
            if (_closing || index < 0 || index >= _pages.Count) return;
            int generation = ++_generation;
            if (_imageLoad != null) _imageLoad.Cancel();
            var cancellation = CancellationTokenSource.CreateLinkedTokenSource(_lifetime.Token);
            cancellation.CancelAfter(TimeSpan.FromSeconds(30)); _imageLoad = cancellation;
            for (int i = 0; i < _pages.Count; i++) if (i != index) { _pages[i].Source = null; _pages[i].Loading = false; }
            PhotoPage page = _pages[index]; page.Source = page.Item.Preview; page.Loading = true;
            _status.Text = string.Empty; _counter.Text = (index + 1) + " / " + _pages.Count;
            _indicators.Children.Clear();
            int first = PhotoViewerPolicy.FirstIndicator(index, _pages.Count);
            for (int i = first; i < Math.Min(_pages.Count, first + 9); i++)
            {
                int target = i;
                var dot = new Ellipse { Width = 8, Height = 8, Margin = new Thickness(5, 0, 5, 0),
                    Fill = new SolidColorBrush(i == index ? Colors.White : Colors.Gray) };
                dot.Tapped += (s, e) => { _photos.SelectedIndex = target; e.Handled = true; };
                _indicators.Children.Add(dot);
            }
            try
            {
                ImageSource image = await LoadImageAsync(page.Item, cancellation.Token);
                if (!_closing && generation == _generation)
                {
                    page.Source = image;
                    ScrollViewer scroll = SelectedScroll();
                    if (scroll != null) scroll.ChangeView(0, 0, 1, true);
                }
            }
            catch (OperationCanceledException) { if (!_closing && generation == _generation) _status.Text = "Загрузка прервана. Выберите фото ещё раз."; }
            catch (Exception) { if (!_closing && generation == _generation) _status.Text = "Не удалось загрузить фотографию."; }
            finally { page.Loading = false; if (_imageLoad == cancellation) _imageLoad = null; cancellation.Dispose(); }
        }

        private async Task<Uri> ResolveAsync(PhotoViewerItem item, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            ResolvedPhoto resolved;
            if (_resolved.TryGetValue(item, out resolved) && DateTime.UtcNow - resolved.Created < TimeSpan.FromMinutes(1)) return resolved.Uri;
            Uri uri = _options.ResolveUriAsync == null ? new Uri(item.Url) : await _options.ResolveUriAsync(item, cancellation);
            cancellation.ThrowIfCancellationRequested();
            if (uri == null || (uri.Scheme != "http" && uri.Scheme != "https")) throw new InvalidOperationException("Invalid photo URI.");
            _resolved[item] = new ResolvedPhoto { Uri = uri, Created = DateTime.UtcNow };
            return uri;
        }
        private void ConfigureRequest(HttpRequestMessage request)
        {
            request.Headers.UserAgent.ParseAdd("Mozilla/5.0");
            if (_options.Referer != null) request.Headers.Referer = _options.Referer;
        }
        private async Task<ImageSource> LoadImageAsync(PhotoViewerItem item, CancellationToken cancellation)
        {
            Uri uri = await ResolveAsync(item, cancellation);
            DecodedPhoto decoded = await DecodePhotoAsync(uri, cancellation);
            cancellation.ThrowIfCancellationRequested();
            var image = new WriteableBitmap((int)decoded.Width, (int)decoded.Height);
            using (Stream output = image.PixelBuffer.AsStream()) output.Write(decoded.Pixels, 0, decoded.Pixels.Length);
            image.Invalidate(); return image;
        }

        private async Task<DecodedPhoto> DecodePhotoAsync(Uri uri, CancellationToken cancellation)
        {
            uint limit = _mobile ? 16u * 1024 * 1024 : 32u * 1024 * 1024;
            using (IRandomAccessStream stream = await ImageDownloadCache.OpenAsync(uri, limit, cancellation, ConfigureRequest).ConfigureAwait(false))
            {
                return await ImageDecodeScheduler.RunAsync(async () =>
                {
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream).AsTask(cancellation).ConfigureAwait(false);
                    if (_mobile && decoder.DecoderInformation.CodecId != BitmapDecoder.JpegDecoderId
                        && (long)decoder.PixelWidth * decoder.PixelHeight > 8388608) throw new InvalidOperationException("Photo too large.");
                    uint width, height;
                    if (!PhotoViewerPolicy.DecodeSize(decoder.PixelWidth, decoder.PixelHeight, _mobile, out width, out height)) throw new InvalidOperationException("Invalid photo size.");
                    PixelDataProvider pixels = await decoder.GetPixelDataAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied,
                        new BitmapTransform { ScaledWidth = width, ScaledHeight = height, InterpolationMode = BitmapInterpolationMode.Linear },
                        ExifOrientationMode.IgnoreExifOrientation, ColorManagementMode.DoNotColorManage).AsTask(cancellation).ConfigureAwait(false);
                    cancellation.ThrowIfCancellationRequested();
                    return new DecodedPhoto { Width = width, Height = height, Pixels = pixels.DetachPixelData() };
                }, cancellation).ConfigureAwait(false);
            }
        }

        private sealed class DecodedPhoto { internal uint Width, Height; internal byte[] Pixels; }
        private sealed class ResolvedPhoto { internal Uri Uri; internal DateTime Created; }

        private PhotoViewerItem SelectedItem { get { int i = _photos.SelectedIndex; return i >= 0 && i < _pages.Count ? _pages[i].Item : null; } }
        private async Task<StorageFile> OriginalFileAsync(PhotoViewerItem item)
        {
            if (item == _downloadedItem && _downloadedFile != null) return _downloadedFile;
            Uri uri = await ResolveAsync(item, _lifetime.Token);
            uint limit = _mobile ? 16u * 1024 * 1024 : 32u * 1024 * 1024;
            using (IRandomAccessStream original = await ImageDownloadCache.OpenAsync(uri, limit, _lifetime.Token, ConfigureRequest))
            {
                byte[] header = new byte[12];
                using (var reader = new DataReader(original.GetInputStreamAt(0)))
                {
                    uint length = await reader.LoadAsync(12).AsTask(_lifetime.Token);
                    if (length < 12) throw new IOException("Invalid photo.");
                    reader.ReadBytes(header);
                }
                string extension = header[0] == 0x89 ? ".png" : header[0] == 0x47 ? ".gif" : header[0] == 0x42 ? ".bmp"
                    : header[0] == 0x52 ? ".webp" : header[0] == 0x49 || header[0] == 0x4d ? ".tif" : header[0] == 0 ? ".ico" : ".jpg";
                StorageFile file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("photo_" + Guid.NewGuid().ToString("N") + extension);
                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    original.Seek(0);
                    await RandomAccessStream.CopyAsync(original, stream).AsTask(_lifetime.Token);
                    await stream.FlushAsync();
                }
                _downloadedItem = item; _downloadedFile = file;
                return file;
            }
        }
        private void SetBusy(bool busy) { _busy = busy; _save.IsEnabled = _share.IsEnabled = !busy; }
        private async void Save_Click(object sender, RoutedEventArgs args)
        {
            PhotoViewerItem item = SelectedItem;
            if (_busy || item == null) return;
            SetBusy(true); _status.Text = "Подготавливаем фотографию…";
            try
            {
                StorageFile original = await OriginalFileAsync(item);
                if (_closing) return;
                var picker = new FileSavePicker { SuggestedStartLocation = PickerLocationId.PicturesLibrary, SuggestedFileName = SafeName(item.FileName, original.FileType) };
                picker.FileTypeChoices.Add("Фото", new List<string> { original.FileType });
                StorageFile destination = await picker.PickSaveFileAsync();
                if (destination != null) { await original.CopyAndReplaceAsync(destination); _status.Text = "Фотография сохранена."; }
                else _status.Text = string.Empty;
            }
            catch (OperationCanceledException) { }
            catch (Exception) { if (!_closing) _status.Text = "Не удалось сохранить фотографию."; }
            finally { SetBusy(false); }
        }
        private async void Share_Click(object sender, RoutedEventArgs args)
        {
            PhotoViewerItem item = SelectedItem;
            if (_busy || item == null) return;
            SetBusy(true); _status.Text = "Подготавливаем фотографию…";
            try
            {
                _shareFile = await OriginalFileAsync(item);
                if (!_closing) { _status.Text = string.Empty; DataTransferManager.ShowShareUI(); }
            }
            catch (OperationCanceledException) { }
            catch (Exception) { if (!_closing) _status.Text = "Не удалось подготовить фотографию."; }
            finally { SetBusy(false); }
        }
        private void Share_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if (_shareFile == null) return;
            args.Request.Data.Properties.Title = string.IsNullOrWhiteSpace(_options.Title) ? "Фотография" : _options.Title;
            args.Request.Data.SetStorageItems(new List<StorageFile> { _shareFile });
        }
        private static string SafeName(string name, string extension)
        {
            name = string.IsNullOrWhiteSpace(name) ? "photo" : System.IO.Path.GetFileNameWithoutExtension(name);
            foreach (char ch in System.IO.Path.GetInvalidFileNameChars()) name = name.Replace(ch, '_');
            return string.IsNullOrWhiteSpace(name) ? "photo" + extension : name + extension;
        }
        public sealed class PhotoPage : INotifyPropertyChanged
        {
            internal PhotoViewerItem Item;
            private ImageSource _source; private bool _loading; private double _width, _height;
            public event PropertyChangedEventHandler PropertyChanged;
            public ImageSource Source { get { return _source; } set { _source = value; Notify("Source"); } }
            public bool Loading { get { return _loading; } set { _loading = value; Notify("Loading"); } }
            public double ViewWidth { get { return _width; } set { _width = value; Notify("ViewWidth"); } }
            public double ViewHeight { get { return _height; } set { _height = value; Notify("ViewHeight"); } }
            private void Notify(string name) { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name)); }
        }
    }
}
