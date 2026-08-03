# PipsPager

A row (or column) of selectable dots that represent pages, matching the WinUI 2.6 `PipsPager`,
which does not exist in the Windows 10 SDK 14393. Useful as a carousel or wizard page indicator.

```xml
xmlns:ui="using:LegacyProjects.UI.UniversalWindowsPlatform.Controls"
```

```xml
<ui:PipsPager
    NumberOfPages="5"
    SelectedIndex="0"
    SelectedIndexChanged="OnPageChanged" />
```

Clicking a dot selects it and raises `SelectedIndexChanged`. `SelectedIndex` is clamped to the valid
range automatically.

## Properties

| Property | Type | Default | Purpose |
|---|---|---:|---|
| `NumberOfPages` | `int` | `0` | Number of dots. |
| `SelectedIndex` | `int` | `0` | Selected page (zero-based). |
| `Orientation` | `Orientation` | `Horizontal` | Dot layout direction. |
| `PipSize` | `double` | `8` | Dot diameter. |
| `PipSpacing` | `double` | `8` | Gap between dots. |
| `SelectedPipBrush` | `Brush` | accent | Selected dot color. |
| `PipBrush` | `Brush` | subtle gray | Unselected dot color. |

## Events

- `SelectedIndexChanged` — provides `OldIndex` and `NewIndex`.
