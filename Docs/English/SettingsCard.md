# SettingsCard

A settings row card: an optional leading icon, a header and description, and a trailing action
element. Mirrors the WinUI Community Toolkit `SettingsCard`. The trailing element is the XAML content.

```xml
xmlns:ui="using:LegacyProjects.UI.UniversalWindowsPlatform.Controls"
```

## With a control on the right

```xml
<ui:SettingsCard
    Header="Theme"
    Description="Choose how the app looks."
    IconGlyph="&#xE771;">
    <ComboBox>
        <ComboBoxItem Content="Light" />
        <ComboBoxItem Content="Dark" />
    </ComboBox>
</ui:SettingsCard>
```

## As a clickable navigation row

```xml
<ui:SettingsCard
    Header="About"
    Description="Version and licenses"
    IconGlyph="&#xE946;"
    IsClickable="True"
    Click="OnAboutClick" />
```

When `IsClickable` is true the whole card raises `Click` and shows a chevron.

## Properties

| Property | Type | Default | Purpose |
|---|---|---:|---|
| `Header` | `string` | `""` | Primary title. |
| `Description` | `string` | `""` | Secondary line; hidden when empty. |
| `IconGlyph` | `string` | `""` | Optional leading glyph. |
| `IconFontFamily` | `string` | `Segoe MDL2 Assets` | Font for the glyph. |
| `ActionContent` | `object` | `null` | Trailing element (the XAML content property). |
| `IsClickable` | `bool` | `False` | Whole card acts like a button. |
| `CardBackground` | `Brush` | theme | Card background. |
| `CornerRadiusValue` | `CornerRadius` | `4` | Corner radius. |

## Events

- `Click` — raised when `IsClickable` is true and the card is tapped.
