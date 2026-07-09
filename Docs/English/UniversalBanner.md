# UniversalBanner

`UniversalBanner` is an image banner/carousel control. Each banner item can have an image, primary text, secondary text, separate colors for both texts, and a click action.

## Basic example with two images

```xml
<ui:UniversalBanner
    Height="260"
    AutoScrollEnabled="True"
    ItemDisplayDuration="3000"
    SlideAnimationEnabled="True"
    SlideAnimationDuration="280"
    ScrollDirection="Horizontal"
    ShowIndicators="True"
    IndicatorPlacement="Bottom"
    PrimaryTextMaxLength="38"
    SecondaryTextMaxLength="60"
    ActionTargetType="TestUILibrary.AppNavigationActions">

    <ui:UniversalBanner.Items>
        <models:UniversalBannerItem
            ImagePath="Assets/GlassBackground.jpg"
            PrimaryText="First test title"
            SecondaryText="First test secondary text"
            PrimaryTextForeground="White"
            SecondaryTextForeground="#FFE0E0E0"
            ClickAction="OpenHome" />

        <models:UniversalBannerItem
            ImagePath="Assets/Square150x150Logo.scale-200.png"
            PrimaryText="Second test title"
            SecondaryText="Second test secondary text"
            PrimaryTextForeground="#FF00B7FF"
            SecondaryTextForeground="#FFFFFFFF"
            ClickAction="OpenProfile" />
    </ui:UniversalBanner.Items>
</ui:UniversalBanner>
```

## One-item banner example

```xml
<ui:UniversalBanner
    Height="180"
    AutoScrollEnabled="False"
    ShowIndicators="False"
    SlideAnimationEnabled="False">
    <ui:UniversalBanner.Items>
        <models:UniversalBannerItem
            ImagePath="Assets/GlassBackground.jpg"
            PrimaryText="Single content banner"
            SecondaryText="This banner has only one item." />
    </ui:UniversalBanner.Items>
</ui:UniversalBanner>
```

## Item structure

| Item property | Purpose |
|---|---|
| `Key` | Optional logical identifier. |
| `ImagePath` | Image path, for example `Assets/GlassBackground.jpg`. |
| `PrimaryText` | Main text line. |
| `SecondaryText` | Secondary text line. |
| `PrimaryTextForeground` | Color of this item's primary text. |
| `SecondaryTextForeground` | Color of this item's secondary text. |
| `ClickAction` | Action name invoked when the item is clicked. |
| `Tag` | Optional object for custom data. |

## Scrolling and timing

| Property | Default | Purpose |
|---|---:|---|
| `SelectedIndex` | `0` | Current item index. |
| `AutoScrollEnabled` | `True` | Enables automatic switching. |
| `ItemDisplayDuration` | `3000` | How long one item is displayed, in milliseconds. |
| `SlideAnimationEnabled` | `True` | Enables slide animation. |
| `SlideAnimationDuration` | `280` | Slide animation duration, in milliseconds. |
| `ScrollDirection` | `Horizontal` | `Horizontal` or `Vertical`. |

Vertical scrolling:

```xml
ScrollDirection="Vertical"
```

Disable slide animation:

```xml
SlideAnimationEnabled="False"
```

## Indicators

| Property | Default | Purpose |
|---|---:|---|
| `ShowIndicators` | `True` | Shows/hides indicator dots. |
| `IndicatorPlacement` | `Bottom` | `Bottom`, `Top`, `Left`, or `Right`. |
| `IndicatorSize` | `8` | Dot size. |
| `IndicatorSpacing` | `6` | Spacing between dots. |
| `IndicatorActiveBrush` | theme/accent fallback | Active dot color. |
| `IndicatorInactiveBrush` | fallback brush | Inactive dot color. |

When `IndicatorPlacement="Left"` or `IndicatorPlacement="Right"`, dots are vertical.

Example:

```xml
ShowIndicators="True"
IndicatorPlacement="Right"
IndicatorSize="10"
IndicatorSpacing="8"
IndicatorActiveBrush="#FFFFFFFF"
IndicatorInactiveBrush="#66FFFFFF"
```

## Text overlay and text length

| Property | Default | Purpose |
|---|---:|---|
| `TextOverlayHeight` | `150` | Height of the text overlay area. |
| `TextPadding` | `24,0,24,30` | Text padding. |
| `PrimaryTextForeground` | white fallback | Default primary text color. |
| `SecondaryTextForeground` | light fallback | Default secondary text color. |
| `PrimaryTextFontSize` | `22` | Primary text size. |
| `SecondaryTextFontSize` | `18` | Secondary text size. |
| `PrimaryTextMaxLength` | `0` | Maximum primary text length. `0` means no trimming. |
| `SecondaryTextMaxLength` | `0` | Maximum secondary text length. `0` means no trimming. |

If text is longer than the configured max length, it is trimmed and `...` is appended.

Example:

```xml
PrimaryTextMaxLength="32"
SecondaryTextMaxLength="70"
```

## Image and overlay

| Property | Default | Purpose |
|---|---:|---|
| `ImageStretch` | `UniformToFill` | Image stretch mode. |
| `BackgroundOverlayEnabled` | `True` | Enables the dark/gradient overlay behind the text. |

```xml
ImageStretch="UniformToFill"
BackgroundOverlayEnabled="True"
```

## Actions

```xml
<ui:UniversalBanner ActionTargetType="TestUILibrary.AppNavigationActions">
    <ui:UniversalBanner.Items>
        <models:UniversalBannerItem ImagePath="Assets/GlassBackground.jpg" PrimaryText="Open" ClickAction="OpenHome" />
    </ui:UniversalBanner.Items>
</ui:UniversalBanner>
```

```csharp
public sealed class AppNavigationActions
{
    public static void OpenHome() { }
}
```

The control also exposes the `ItemClick` event.
