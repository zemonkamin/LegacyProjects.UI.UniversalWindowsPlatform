# Expander

Заголовок с областью сворачиваемого содержимого, аналогично `Expander` из WinUI 2.6, которого нет в SDK Windows 10 14393. Сворачиваемое содержимое — это XAML **содержимое** элемента.

```xml
xmlns:ui="using:LegacyProjects.UI.UniversalWindowsPlatform.Controls"
```

```xml
<ui:Expander Header="Расширенные параметры" HeaderIconGlyph="&#xE713;" IsExpanded="False">
    <StackPanel>
        <TextBlock Text="Здесь может быть что угодно." TextWrapping="Wrap" />
        <ToggleSwitch Header="Включить функцию" />
    </StackPanel>
</ui:Expander>
```

`Header` принимает строку (отображается полужирным) или любой элемент.

## Свойства

| Свойство | Тип | По умолчанию | Назначение |
|---|---|---:|---|
| `Header` | `object` | `null` | Заголовок — строка или элемент. |
| `ExpanderContent` | `object` | `null` | Сворачиваемое содержимое (свойство содержимого XAML). |
| `IsExpanded` | `bool` | `False` | Отображать содержимое. |
| `ExpandDirection` | `ExpandDirection` | `Down` | `Down` открывает ниже заголовка, `Up` — выше. |
| `HeaderIconGlyph` | `string` | `null` | Необязательный глиф Segoe MDL2 в начале заголовка. |
| `HeaderBackground` | `Brush` | theme | Фон строки заголовка. |
| `ContentBackground` | `Brush` | theme | Фон области содержимого. |
| `CornerRadiusValue` | `CornerRadius` | `4` | Радиус скругления рамки. |

## События

- `Expanded` — возникает при открытии.
- `Collapsed` — возникает при закрытии.