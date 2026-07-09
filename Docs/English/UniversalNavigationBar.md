# UniversalNavigationBar

`UniversalNavigationBar` is a top-only navigation bar with left, center, and right item areas. The center area is intended for scrollable content when there are many elements.

Important: the navigation bar is designed to stay at the top of the window. Do not use it as a bottom or floating bar.

## Basic example

```xml
<ui:UniversalNavigationBar
    BarHeight="56"
    Background="#FF111310"
    BackgroundOpacity="0"
    ShowIcons="True"
    ShowText="True"
    SeparatorEnabled="True"
    SeparatorThickness="1"
    SeparatorBrush="#66333333"
    PressAnimationEnabled="True"
    ActionTargetType="TestUILibrary.AppNavigationActions">

    <ui:UniversalNavigationBar.LeftItems>
        <models:UniversalNavigationBarItem Text="Menu" Glyph="&#xE700;" ClickAction="ToggleSidebar" />
    </ui:UniversalNavigationBar.LeftItems>

    <ui:UniversalNavigationBar.CenterItems>
        <models:UniversalNavigationBarItem Text="Home" Glyph="&#xE80F;" ClickAction="OpenHome" />
        <models:UniversalNavigationBarItem Text="Banner" Glyph="&#xE7F4;" ClickAction="OpenBannerTest" />
    </ui:UniversalNavigationBar.CenterItems>

    <ui:UniversalNavigationBar.RightItems>
        <models:UniversalNavigationBarItem Text="Profile" Glyph="&#xE77B;" ClickAction="OpenProfile" />
    </ui:UniversalNavigationBar.RightItems>
</ui:UniversalNavigationBar>
```

## Sections

| Collection | Purpose |
|---|---|
| `LeftItems` | Left side elements. Good for menu/sidebar button. |
| `CenterItems` | Center elements. If there are many items, the center section can scroll. |
| `RightItems` | Right side elements. Good for account, settings, actions. |

## Size and layout

| Property | Default | Purpose |
|---|---:|---|
| `BarHeight` | `56` | Navigation bar height. |
| `ItemHeight` | `56` | Item height. |
| `ItemPadding` | `14,0,14,0` | Item internal padding. |
| `ItemSpacing` | `0` | Spacing between items. |
| `ItemForeground` | theme fallback | Shared item foreground. |
| `DisabledForeground` | gray fallback | Foreground for disabled items. |
| `DisabledItemOpacity` | `0.45` | Opacity for disabled items. |

## Background opacity

`BackgroundOpacity` uses inverse opacity semantics:

- `0` means not transparent, fully opaque. This is the default.
- `0.325` means 32.5% transparent.
- `0.9` means 90% transparent.
- `1` means fully transparent.

```xml
Background="#FF111310"
BackgroundOpacity="0.325"
```

## Bottom separator

The navigation bar supports a bottom separator line similar to the tab bar:

```xml
SeparatorEnabled="True"
SeparatorThickness="1"
SeparatorBrush="#66333333"
```

## Icons and text

```xml
ShowIcons="True"
ShowText="True"
IconSize="18"
IconTextSpacing="8"
LabelFontSize="14"
```

Glyph item:

```xml
<models:UniversalNavigationBarItem Text="Menu" IconType="Glyph" Glyph="&#xE700;" />
```

Image item:

```xml
<models:UniversalNavigationBarItem Text="Logo" IconType="Image" ImagePath="Assets/Square150x150Logo.scale-200.png" />
```

Per-item customization:

```xml
<models:UniversalNavigationBarItem
    Text="Profile"
    Glyph="&#xE77B;"
    Background="#220078D7"
    Foreground="#FFFFFFFF"
    DisabledForeground="#FF777777"
    Width="120"
    MinWidth="80" />
```

## Disabled items

```xml
<models:UniversalNavigationBarItem Text="Disabled" Glyph="&#xE7C3;" IsEnabled="False" />
```

Disabled items cannot be clicked and do not invoke `ClickAction`.

## Press animation

```xml
PressAnimationEnabled="True"
PressAnimationScale="0.96"
PressAnimationDuration="90"
```

Disable it:

```xml
PressAnimationEnabled="False"
```

## Actions

```xml
<ui:UniversalNavigationBar ActionTargetType="TestUILibrary.AppNavigationActions">
    <ui:UniversalNavigationBar.LeftItems>
        <models:UniversalNavigationBarItem Text="Menu" Glyph="&#xE700;" ClickAction="ToggleSidebar" />
    </ui:UniversalNavigationBar.LeftItems>
</ui:UniversalNavigationBar>
```

```csharp
public sealed class AppNavigationActions
{
    public static void ToggleSidebar() { }
}
```

The control also exposes the `ItemClick` event for code-behind usage.
