# NumberBox

A numeric input field with optional increment/decrement spin buttons, range clamping and parsing.
Modeled on the WinUI 2.3 `NumberBox`, which is not in the Windows 10 SDK 14393.

```xml
xmlns:ui="using:LegacyProjects.UI.UniversalWindowsPlatform.Controls"
```

```xml
<ui:NumberBox
    Header="Period (seconds)"
    Minimum="10" Maximum="120"
    SmallChange="5"
    Value="30" />
```

- Type a value and press **Enter** or move focus to commit it (clamped to `Minimum`/`Maximum`).
- **Up/Down** arrow keys and the spin buttons step by `SmallChange`.
- An empty field is represented by `Value = NaN`.

## Properties

| Property | Type | Default | Purpose |
|---|---|---:|---|
| `Header` | `string` | `""` | Label above the input; hidden when empty. |
| `Value` | `double` | `NaN` | The numeric value; `NaN` means empty. |
| `Minimum` | `double` | `-∞` | Lower bound. |
| `Maximum` | `double` | `+∞` | Upper bound. |
| `SmallChange` | `double` | `1` | Step for spin buttons and arrow keys. |
| `PlaceholderText` | `string` | `""` | Shown when empty. |
| `SpinButtonPlacementMode` | `NumberBoxSpinButtonPlacement` | `Inline` | `Inline` or `Hidden`. |
| `NumberFormat` | `string` | `null` | Optional .NET format string, for example `"F2"`. |

## Events

- `ValueChanged` — raised after a commit or spin changes `Value`.
