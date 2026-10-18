# PhotoViewer

`PhotoViewer` opens a full-window, dark photo overlay for one or more remote images. It supports swipe/`FlipView` navigation, a photo counter, up to nine clickable position indicators, zoom and pan, and native **Save** and **Share** actions. It is a programmatically opened viewer, **not** a control you place in XAML.

The implementation targets UWP (Windows 10 SDK 14393 / C# 6) and handles both desktop and Windows Mobile layouts.

## Quick start

Add a button to an existing page:

```xml
<Button Content="View photos" Click="ViewPhotos_Click" />
```

In the page's code-behind:

```csharp
using System;
using System.Collections.Generic;
using LegacyProjects.UI.UniversalWindowsPlatform.Controls;
using LegacyProjects.UI.UniversalWindowsPlatform.Models;
using Windows.UI.Xaml;

private async void ViewPhotos_Click(object sender, RoutedEventArgs e)
{
    var photos = new List<PhotoViewerItem>
    {
        new PhotoViewerItem
        {
            Url = "https://example.com/images/photo1.jpg",
            Title = "First photo",
            FileName = "photo1.jpg"
        },
        new PhotoViewerItem
        {
            Url = "https://example.com/images/photo2.jpg",
            Title = "Second photo",
            FileName = "photo2.jpg"
        }
    };

    await PhotoViewer.ShowAsync(photos, 0, new PhotoViewerOptions
    {
        Title = "Photo collection",
        SaveLabel = "Save",
        ShareLabel = "Share"
    });
    // Execution continues after the viewer closes.
}
```

Replace the example URLs with real, publicly reachable or appropriately authorized image URLs. `ShowAsync` accepts `IEnumerable<PhotoViewerItem>`, and its `selectedIndex` parameter is zero-based. The returned `Task` completes when the overlay closes; it does **not** complete immediately after opening.

## PhotoViewerItem

Namespace: `LegacyProjects.UI.UniversalWindowsPlatform.Models`.

| Property | Type | Purpose |
|---|---|---|
| `Url` | `string` | **Required.** Absolute `http://` or `https://` URL. |
| `Title` | `string` | Optional metadata. The current viewer does **not** display this per-photo title. |
| `FileName` | `string` | Suggested base file name for **Save**; invalid filename characters are replaced and the detected image extension is used. |
| `Preview` | `ImageSource` | Optional placeholder shown while the selected full-size image loads. |

A lightweight local preview can be supplied even though the full-size `Url` must be remote:

```csharp
using Windows.UI.Xaml.Media.Imaging;

var photo = new PhotoViewerItem
{
    Url = "https://example.com/images/fullsize.jpg",
    Preview = new BitmapImage(new Uri("ms-appx:///Assets/preview.jpg")),
    FileName = "fullsize.jpg"
};
```

Null entries and items whose `Url` is not an absolute HTTP(S) URI are discarded. If no valid items remain, the viewer does not open. `selectedIndex` refers to the original incoming sequence; if that item is filtered out, an index clamped to the remaining sequence is used. Only HTTP(S) sources are supported for the full-size image; `ms-appx:`, `file:`, `ms-appdata:` and local paths are not accepted as `Url`.

## PhotoViewerOptions

Namespace: `LegacyProjects.UI.UniversalWindowsPlatform.Models`.

| Property | Type | Default / behavior |
|---|---|---|
| `Title` | `string` | Share-sheet title; falls back to `Фотография` if empty. Not an overlay heading. |
| `SaveLabel` | `string` | Save button caption; defaults to `Сохранить` when null. |
| `ShareLabel` | `string` | Share button caption; defaults to `Поделиться` when null. |
| `Referer` | `Uri` | Optional HTTP `Referer` header added when downloading an image. |
| `ResolveUriAsync` | `Func<PhotoViewerItem, CancellationToken, Task<Uri>>` | Optional asynchronous URI resolver, for example to obtain a signed download URL. |

The resolver is called when a photo is loaded or its original file is requested for saving/sharing. It must return an absolute HTTP(S) URI and should honor the supplied cancellation token. A successful resolved URL is reused for the same photo for approximately one minute while the viewer remains open. The original `PhotoViewerItem.Url` must **still** be a valid absolute HTTP(S) URL, even when a resolver is provided.

For example, to supply a `Referer` and hook up a resolver:

```csharp
using System;
using System.Threading.Tasks;

var options = new PhotoViewerOptions
{
    Referer = new Uri("https://example.com/"),
    ResolveUriAsync = (item, token) =>
    {
        token.ThrowIfCancellationRequested();
        // Replace this with your asynchronous signed-URL lookup if required.
        return Task.FromResult(new Uri(item.Url));
    }
};

await PhotoViewer.ShowAsync(photos, 0, options);
```

The built-in HTTP client does not send cookies. A `Referer` or signed URL may be needed for hosts that restrict image access; cookie-based authenticated downloads are not provided by this API.

## Interaction and navigation

- Swipe or use the `FlipView` to change photos. Left/Right arrow keys also change the selected photo when it is not zoomed in; navigation stops at the first/last photo.
- Double-tap/double-click a photo to toggle between 1× and 2.5× zoom. The inner `ScrollViewer` supports zoom/pan up to 5×.
- The header shows the current position (`1 / 3`, etc.). The bottom indicators show a sliding window of at most nine photos; tapping one selects that photo.
- Close with the top-left **X**, `Esc`, the system Back action, or a tap on the surrounding backdrop/outside the displayed image when not zoomed in.
- **Save** opens the system `FileSavePicker`; **Share** opens the Windows share UI. Both work with the downloaded **original** file, rather than the downscaled preview used for display.

Only one viewer can be active. Opening another one closes the previous overlay.

## Public static API

```csharp
await PhotoViewer.ShowAsync(photos);                  // First photo, default options
await PhotoViewer.ShowAsync(photos, selectedIndex: 2); // Start on the third photo

bool opened = PhotoViewer.IsOpen;
bool closed = PhotoViewer.TryCloseCurrent(); // false if no viewer was open
```

`ShowAsync(IEnumerable<PhotoViewerItem> items, int selectedIndex = 0, PhotoViewerOptions options = null)` returns `Task`. `IsOpen` is a static `bool` property; `TryCloseCurrent()` returns whether an active viewer was found. The constructor is private: do not add `<ui:PhotoViewer />` to a XAML tree. There are no public item-selection or viewer-closed events; `await ShowAsync(...)` is the completion signal.

## Download behavior and limitations

- Images are downloaded over HTTP(S), cached inside the app's `LocalCacheFolder/PhotoImages-v1`, and reused for up to 24 hours while fresh. Cache storage is bounded to approximately **32 MiB on Mobile** / **128 MiB on desktop** during cleanup.
- Maximum downloaded file size: **16 MiB on Mobile** / **32 MiB on desktop**. Download and decoding concurrency is limited to avoid overloading slower devices.
- The on-screen decoded bitmap is reduced to a maximum edge of **1280 px on Mobile** / **2560 px on desktop**, with pixel budgets of about **1 million** / **4 million** respectively. Saving and sharing still use the original downloaded bytes.
- An image is rejected if the original dimensions are invalid or exceed the safety limits; on Mobile, non-JPEG images above roughly **8.4 million pixels** are also rejected.
- Changing the selected photo cancels the previous image load; loading the selected photo has a roughly 30-second cancellation deadline. Failed downloads/decodes show a message inside the overlay.
- The decoder currently ignores EXIF orientation and color management for the displayed bitmap. Support for specific encoded formats also depends on the Windows image decoder.
- Button labels can be localized with `SaveLabel` and `ShareLabel`, but the built-in status/error messages, share-title fallback, save-dialog file-type label, close-button accessibility label, and opening-error dialog are currently hardcoded in **Russian**.

For remote images, ensure the app has the UWP `internetClient` capability in its application manifest. The provided library itself does not add this permission to the host app.
