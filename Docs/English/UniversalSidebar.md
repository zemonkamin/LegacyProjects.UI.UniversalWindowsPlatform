# UniversalSidebar

`UniversalSidebar` is a configurable sidebar control with top, middle, and bottom item groups. Top and bottom groups can be pinned, while the middle group can scroll separately. If top and bottom groups are not pinned, the sidebar can behave like one common scrollable list.

## Basic example

```xml
<ui:UniversalSidebar
    x:Name="Sidebar"
    OpenPaneWidth="300"
    CompactPaneWidth="56"
    HorizontalBreakpoint="768"
    Background="#FF111310"
    BackgroundOpacity="0"
    ShowIcons="True"
    ShowIconsWhenClosedInHorizontalMode="True"
    TopItemsPinned="True"
    BottomItemsPinned="True"
    SideSeparatorEnabled="True"
    SeparatorThickness="1"
    SeparatorBrush="#66333333"
    AnimationEnabled="True"
    AnimationDuration="220"
    AnimationEasingFunction="Cubic"
    AnimationEasingMode="EaseOut"
    PressAnimationEnabled="True"
    ActionTargetType="TestUILibrary.AppNavigationActions">

    <ui:UniversalSidebar.TopItems>
        <models:UniversalSidebarItem Text="Home" Key="home" Glyph="&#xE80F;" ClickAction="OpenHome" />
    </ui:UniversalSidebar.TopItems>

    <ui:UniversalSidebar.Items>
        <models:UniversalSidebarItem Text="Banner" Key="banner" Glyph="&#xE7F4;" ClickAction="OpenBannerTest" />
        <models:UniversalSidebarItem Text="Disabled" Glyph="&#xE7C3;" IsEnabled="False" />
    </ui:UniversalSidebar.Items>

    <ui:UniversalSidebar.BottomItems>
        <models:UniversalSidebarItem Text="Settings" Key="settings" Glyph="&#xE713;" />
    </ui:UniversalSidebar.BottomItems>
</ui:UniversalSidebar>
```

Toggle from code-behind:

```csharp
private void ToggleSidebar_Click(object sender, RoutedEventArgs e)
{
    Sidebar.IsPaneOpen = !Sidebar.IsPaneOpen;
}
```

## Groups

| Collection | Purpose |
|---|---|
| `TopItems` | Items rendered at the top. Good for Home, main navigation, menu button rows. |
| `Items` | Middle/common items. Usually the scrollable section. |
| `BottomItems` | Items rendered near the bottom. Good for Settings, Account, About. |

Pinned behavior:

```xml
TopItemsPinned="True"
BottomItemsPinned="True"
```

- If `TopItemsPinned="True"`, top items stay fixed and the middle section scrolls.
- If `BottomItemsPinned="True"`, bottom items stay fixed at the bottom and the middle section scrolls.
- If both are `False`, the control can be used as a more common single scrollable list.

## Width and responsive mode

| Property | Default | Purpose |
|---|---:|---|
| `IsPaneOpen` | `False` | Current open/closed state. |
| `OpenPaneWidth` | `300` | Width when opened. |
| `CompactPaneWidth` | `56` | Compact width when closed in horizontal mode. |
| `HorizontalBreakpoint` | `768` | Window width where horizontal behavior starts. |
| `ShowIconsWhenClosedInHorizontalMode` | `False` | Shows icons in compact horizontal mode. |

`ShowIconsWhenClosedInHorizontalMode="True"` is useful when the closed sidebar should still show icon buttons.

## Colors and opacity

`BackgroundOpacity` uses inverse opacity semantics:

- `0` means not transparent, fully opaque. This is the default.
- `0.325` means 32.5% transparent.
- `0.9` means 90% transparent.
- `1` means fully transparent.

Main color properties:

| Property | Default | Purpose |
|---|---:|---|
| `Background` | page/theme background | Sidebar background. |
| `BackgroundOpacity` | `0` | Background transparency, where `1` is fully transparent. |
| `SelectedItemBackground` | accent-like fallback | Background for selected item. |
| `UnselectedItemBackground` | transparent | Background for normal item. |
| `DisabledItemBackground` | transparent | Background for disabled item. |
| `SelectedItemForeground` | white/accent fallback | Foreground for selected item. |
| `UnselectedItemForeground` | theme foreground fallback | Foreground for normal item. |
| `DisabledItemForeground` | gray fallback | Foreground for disabled item. |
| `DisabledItemOpacity` | `0.45` | Opacity for disabled items. |

Per-item colors can override shared colors:

```xml
<models:UniversalSidebarItem
    Text="Custom"
    Glyph="&#xE8A7;"
    SelectedBackground="#FF005A9E"
    UnselectedBackground="Transparent"
    DisabledBackground="#22222222"
    SelectedForeground="White"
    UnselectedForeground="#FFCCCCCC"
    DisabledForeground="#FF777777" />
```

## Sidebar side separator

```xml
SideSeparatorEnabled="True"
SeparatorThickness="1"
SeparatorBrush="#66333333"
```

The side separator is the vertical line that separates the sidebar from the page content. `SeparatorThickness` and `SeparatorBrush` configure it.

## Icons

Glyph:

```xml
<models:UniversalSidebarItem Text="Home" IconType="Glyph" Glyph="&#xE80F;" />
```

Image:

```xml
<models:UniversalSidebarItem Text="Logo" IconType="Image" ImagePath="Assets/Square150x150Logo.scale-200.png" />
```

Global icon settings:

```xml
ShowIcons="True"
IconSize="18"
IconTextSpacing="16"
```

## Disabled items

```xml
<models:UniversalSidebarItem Text="Disabled" Glyph="&#xE7C3;" IsEnabled="False" />
```

Disabled items cannot be clicked and do not invoke `ClickAction`.

## Open/close animation

```xml
AnimationEnabled="True"
AnimationDuration="220"
AnimationEasingFunction="Cubic"
AnimationEasingMode="EaseOut"
```

Supported easing functions:

- `Cubic`
- `Quadratic`
- `Back`
- `Circle`
- `Exponential`

Supported easing modes:

- `EaseOut`
- `EaseIn`
- `EaseInOut`

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
<ui:UniversalSidebar ActionTargetType="TestUILibrary.AppNavigationActions">
    <ui:UniversalSidebar.Items>
        <models:UniversalSidebarItem Text="Home" Glyph="&#xE80F;" ClickAction="OpenHome" />
    </ui:UniversalSidebar.Items>
</ui:UniversalSidebar>
```

```csharp
public sealed class AppNavigationActions
{
    public static void OpenHome() { }
}
```

The control also exposes `ItemClick` and `SelectionChanged` events for code-behind usage.
