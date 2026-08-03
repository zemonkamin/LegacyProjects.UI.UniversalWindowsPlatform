# Expander

A header with a collapsible content region, like the WinUI 2.6 `Expander` (which is not in the
Windows 10 SDK 14393). The collapsible content is the XAML **content** of the element.

```xml
xmlns:ui="using:LegacyProjects.UI.UniversalWindowsPlatform.Controls"
```

```xml
<ui:Expander Header="Advanced options" HeaderIconGlyph="&#xE713;" IsExpanded="False">
    <StackPanel>
        <TextBlock Text="Anything can go here." TextWrapping="Wrap" />
        <ToggleSwitch Header="Enable feature" />
    </StackPanel>
</ui:Expander>
```

`Header` accepts a string (rendered semibold) or any element.

## Properties

| Property | Type | Default | Purpose |
|---|---|---:|---|
| `Header` | `object` | `null` | Header string or element. |
| `ExpanderContent` | `object` | `null` | Collapsible content (the XAML content property). |
| `IsExpanded` | `bool` | `False` | Whether the content is shown. |
| `ExpandDirection` | `ExpandDirection` | `Down` | `Down` opens below the header, `Up` opens above. |
| `HeaderIconGlyph` | `string` | `null` | Optional Segoe MDL2 glyph at the start of the header. |
| `HeaderBackground` | `Brush` | theme | Header row background. |
| `ContentBackground` | `Brush` | theme | Content region background. |
| `CornerRadiusValue` | `CornerRadius` | `4` | Corner radius of the frame. |

## Events

- `Expanded` — raised when it opens.
- `Collapsed` — raised when it closes.
