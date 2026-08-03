# CircularProgress

A ring-shaped progress control. The Windows 10 SDK 14393 `ProgressRing` is **indeterminate only**;
`CircularProgress` adds the **determinate** ring found in WinUI 2/3, plus optional centered text.

```xml
xmlns:ui="using:LegacyProjects.UI.UniversalWindowsPlatform.Controls"
```

## Determinate (fills based on Value)

```xml
<ui:CircularProgress
    Width="64" Height="64"
    Minimum="0" Maximum="100" Value="72"
    StrokeThickness="6"
    ShowText="True" />
```

With no `Text`, the center shows the percentage. Set `Text` to override (for example a countdown):

```xml
<ui:CircularProgress
    Width="46" Height="46"
    Maximum="30" Value="18"
    ShowText="True" Text="18"
    StrokeThickness="3" />
```

## Indeterminate (rotating arc)

```xml
<ui:CircularProgress Width="40" Height="40" IsIndeterminate="True" />
```

## Properties

| Property | Type | Default | Purpose |
|---|---|---:|---|
| `Minimum` | `double` | `0` | Value for an empty ring. |
| `Maximum` | `double` | `100` | Value for a full ring. |
| `Value` | `double` | `0` | Current progress. |
| `IsIndeterminate` | `bool` | `False` | Rotating arc instead of a fixed value. |
| `StrokeThickness` | `double` | `4` | Ring thickness. |
| `ProgressBrush` | `Brush` | accent | Arc color. |
| `TrackBrush` | `Brush` | subtle gray | Background ring color. |
| `RoundCaps` | `bool` | `True` | Rounded arc ends. |
| `ShowText` | `bool` | `False` | Show centered text. |
| `Text` | `string` | `null` | Explicit center text; percentage when null. |
| `TextForeground` | `Brush` | foreground | Center text color. |
| `TextFontSize` | `double` | auto | Center text size (defaults to ~diameter/3). |
| `IndeterminateArcLength` | `double` | `0.25` | Arc length (0..1) when indeterminate. |
| `IndeterminateDuration` | `double` | `1100` | Full-rotation time in ms. |

> A single arc cannot draw a full 360° circle, so at `Value == Maximum` the ring sweeps ~359.9°,
> which reads as closed with round caps.
