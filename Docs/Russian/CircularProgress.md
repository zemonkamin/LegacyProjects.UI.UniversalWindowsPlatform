# CircularProgress

Кольцевой индикатор прогресса. Стандартный `ProgressRing` из SDK Windows 10 14393 поддерживает только **неопределённый** режим; `CircularProgress` добавляет **определённое** кольцо из WinUI 2/3, а также необязательный текст в центре.

```xml
xmlns:ui="using:LegacyProjects.UI.UniversalWindowsPlatform.Controls"
```

## Определённый режим (заполняется на основе Value)

```xml
<ui:CircularProgress
    Width="64" Height="64"
    Minimum="0" Maximum="100" Value="72"
    StrokeThickness="6"
    ShowText="True" />
```

Без `Text` в центре отображается процент. Укажите `Text`, чтобы переопределить (например, обратный отсчёт):

```xml
<ui:CircularProgress
    Width="46" Height="46"
    Maximum="30" Value="18"
    ShowText="True" Text="18"
    StrokeThickness="3" />
```

## Неопределённый режим (вращающаяся дуга)

```xml
<ui:CircularProgress Width="40" Height="40" IsIndeterminate="True" />
```

## Свойства

| Свойство | Тип | По умолчанию | Назначение |
|---|---|---:|---|
| `Minimum` | `double` | `0` | Значение пустого кольца. |
| `Maximum` | `double` | `100` | Значение полного кольца. |
| `Value` | `double` | `0` | Текущий прогресс. |
| `IsIndeterminate` | `bool` | `False` | Вращающаяся дуга вместо фиксированного значения. |
| `StrokeThickness` | `double` | `4` | Толщина кольца. |
| `ProgressBrush` | `Brush` | accent | Цвет дуги. |
| `TrackBrush` | `Brush` | приглушённый серый | Цвет фонового кольца. |
| `RoundCaps` | `bool` | `True` | Скруглённые концы дуги. |
| `ShowText` | `bool` | `False` | Показывать текст в центре. |
| `Text` | `string` | `null` | Явный текст в центре; процент при `null`. |
| `TextForeground` | `Brush` | foreground | Цвет текста в центре. |
| `TextFontSize` | `double` | auto | Размер текста в центре (по умолчанию ~диаметр/3). |
| `IndeterminateArcLength` | `double` | `0.25` | Длина дуги (0..1) в неопределённом режиме. |
| `IndeterminateDuration` | `double` | `1100` | Время полного вращения в мс. |

> Одна дуга не может нарисовать полный круг 360°, поэтому при `Value == Maximum` кольцо проходит ~359,9°, что при скруглённых концах выглядит как замкнутое.