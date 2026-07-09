# UniversalTabBar

`UniversalTabBar` is a configurable tab bar control. It can be placed at the top, bottom, or at a custom vertical offset. Horizontally it can be stretched, aligned left, aligned right, or moved by a custom offset.

## Basic example

```xml
<ui:UniversalTabBar
    VerticalPlacement="Top"
    HorizontalPlacement="Stretch"
    BarHeight="56"
    Background="#FF101010"
    BackgroundOpacity="0"
    SelectedTabForeground="#FF0078D7"
    UnselectedTabForeground="#FFFF0000"
    SeparatorEnabled="True"
    SeparatorThickness="1"
    SeparatorBrush="#66333333"
    ShowIcons="True"
    ShowText="True"
    TextPosition="BelowIcon"
    ActionTargetType="TestUILibrary.TabbarControl">

    <ui:UniversalTabBar.Items>
        <models:UniversalTabBarItem Text="Home" Glyph="&#xE80F;" ClickAction="OpenHome" />
        <models:UniversalTabBarItem Text="Profile" Glyph="&#xE77B;" ClickAction="OpenProfile" />
        <models:UniversalTabBarItem Text="Disabled" Glyph="&#xE7C3;" IsEnabled="False" />
    </ui:UniversalTabBar.Items>
</ui:UniversalTabBar>
```

## Placement and size

| Property | Values / type | Default | Purpose |
|---|---:|---:|---|
| `VerticalPlacement` | `Top`, `Bottom`, `Custom` | `Bottom` | Vertical position. |
| `HorizontalPlacement` | `Left`, `Right`, `Stretch`, `Custom` | `Stretch` | Horizontal position. |
| `BarHeight` | `double` | `56` | Bar height. |
| `BarWidth` | `double` | `NaN` | Custom width. Use with `Left`, `Right`, or `Custom`. |
| `CustomVerticalOffset` | `double` | `0` | Y offset when `VerticalPlacement="Custom"`. |
| `CustomHorizontalOffset` | `double` | `0` | X offset when `HorizontalPlacement="Custom"`. |
| `Orientation` | `Horizontal`, `Vertical` | `Horizontal` | Item layout direction. |

## Colors and transparency

`BackgroundOpacity` uses inverse opacity semantics:

- `0` means not transparent, fully opaque. This is the default.
- `0.325` means 32.5% transparent.
- `0.9` means 90% transparent.
- `1` means fully transparent.

General colors:

| Property | Default | Purpose |
|---|---:|---|
| `Background` | page/theme background | Bar background. |
| `BackgroundOpacity` | `0` | Background transparency, where `1` is fully transparent. |
| `SelectedTabForeground` | system accent | Default selected icon/text color. |
| `UnselectedTabForeground` | gray | Default unselected icon/text color. |
| `DisabledItemOpacity` | `0.45` | Opacity for disabled items. |

Per-item colors override the shared colors:

```xml
<models:UniversalTabBarItem
    Text="Profile"
    Glyph="&#xE77B;"
    SelectedIconForeground="#FF00B7FF"
    UnselectedIconForeground="#FFFF4444"
    SelectedTextForeground="#FF00B7FF"
    UnselectedTextForeground="#FFFF4444" />
```

Item color properties:

- `IconForeground`
- `TextForeground`
- `SelectedIconForeground`
- `UnselectedIconForeground`
- `SelectedTextForeground`
- `UnselectedTextForeground`

If no per-item color is set, the control uses `SelectedTabForeground` or `UnselectedTabForeground`.

## Separator line

The separator can be enabled or disabled. Its side is selected automatically from placement. In custom/floating placements, multiple sides can be used by the control to visually separate it from content.

```xml
SeparatorEnabled="True"
SeparatorThickness="1"
SeparatorBrush="#66333333"
```

## Icons and text

```xml
ShowIcons="True"
ShowText="True"
TextPosition="BelowIcon"
IconSize="20"
LabelFontSize="11"
```

`TextPosition` values:

- `BelowIcon`
- `AboveIcon`

Glyph icon:

```xml
<models:UniversalTabBarItem Text="Home" IconType="Glyph" Glyph="&#xE80F;" />
```

Image icon:

```xml
<models:UniversalTabBarItem Text="Logo" IconType="Image" ImagePath="Assets/Square150x150Logo.scale-200.png" />
```

## Press effect

Global setting:

```xml
PressEffectEnabled="False"
```

Per-item override:

```xml
<models:UniversalTabBarItem Text="Home" Glyph="&#xE80F;" PressEffectMode="Disabled" />
```

`PressEffectMode` values:

- `Default` uses `PressEffectEnabled` from the tab bar.
- `Enabled` forces the press effect for this item.
- `Disabled` disables the press effect for this item.

## Disabled tabs

```xml
<models:UniversalTabBarItem Text="Library" Glyph="&#xE8F1;" IsEnabled="False" />
```

Disabled items cannot be clicked, do not invoke `ClickAction`, and are rendered with `DisabledItemOpacity`.

## Actions

```xml
<ui:UniversalTabBar ActionTargetType="TestUILibrary.TabbarControl">
    <ui:UniversalTabBar.Items>
        <models:UniversalTabBarItem Text="Home" Glyph="&#xE80F;" ClickAction="OpenHome" />
        <models:UniversalTabBarItem Text="Profile" Glyph="&#xE77B;" ClickAction="OpenProfile" />
    </ui:UniversalTabBar.Items>
</ui:UniversalTabBar>
```

```csharp
public sealed class TabbarControl
{
    public static void OpenHome() { }
    public static void OpenProfile() { }
}
```

## Glass effect

The tab bar has experimental glass properties:

```xml
GlassEffectEnabled="True"
GlassTintColor="#FF00FF00"
GlassTintAmount="0.5"
GlassBlurAmount="0.9"
GlassShaderPath="Shaders/GlassEffect.fx"
```

Important: the glass effect is not recommended for regular use. It can be unstable on older UWP targets, may flicker during scrolling, and may be slower on low-end devices. Prefer a normal `Background` with `BackgroundOpacity` unless the glass effect is required.

Available glass properties:

| Property | Default | Purpose |
|---|---:|---|
| `GlassEffectEnabled` | `False` | Enables experimental glass background. |
| `GlassTintColor` | transparent | Tint color. |
| `GlassTintAmount` | `0` | Tint strength from `0` to `1`. |
| `GlassBlurAmount` | `0` | Blur strength from `0` to `1`. |
| `GlassShaderPath` | `Shaders/GlassEffect.fx` | Shader file path. |

Use `RefreshGlassEffect()` from code-behind only when the background under the tab bar changes and the software fallback must be refreshed.
