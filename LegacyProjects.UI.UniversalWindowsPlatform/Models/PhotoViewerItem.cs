using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Models
{
    public sealed class PhotoViewerItem
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public ImageSource Preview { get; set; }
    }

    public sealed class PhotoViewerOptions
    {
        public string Title { get; set; }
        public string SaveLabel { get; set; }
        public string ShareLabel { get; set; }
        public Uri Referer { get; set; }
        public Func<PhotoViewerItem, CancellationToken, Task<Uri>> ResolveUriAsync { get; set; }
    }
}
