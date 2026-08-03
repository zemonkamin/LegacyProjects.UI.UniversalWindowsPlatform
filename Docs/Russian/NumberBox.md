# NumberBox

Поле ввода числового значения с необязательными кнопками увеличения/уменьшения, ограничением диапазона и парсингом. Моделируется на `NumberBox` из WinUI 2.3, которого нет в SDK Windows 10 14393.

```xml
xmlns:ui="using:LegacyProjects.UI.UniversalWindowsPlatform.Controls"
```

```xml
<ui:NumberBox
    Header="Период (секунды)"
    Minimum="10" Maximum="120"
    SmallChange="5"
    Value="30" />
```

- Введите значение и нажмите **Enter** или переключите фокус для подтверждения (значение ограничивается `Minimum`/`Maximum`).
- Клавиши **Вверх/Вниз** и кнопки вращения шагают с шагом `SmallChange`.
- Пустое поле представляется как `Value = NaN`.

## Свойства

| Свойство | Тип | По умолчанию | Назначение |
|---|---|---:|---|
| `Header` | `string` | `""` | Метка над полем ввода; скрыта, когда пусто. |
| `Value` | `double` | `NaN` | Числовое значение; `NaN` означает пусто. |
| `Minimum` | `double` | `-∞` | Нижняя граница. |
| `Maximum` | `double` | `+∞` | Верхняя граница. |
| `SmallChange` | `double` | `1` | Шаг для кнопок вращения и стрелок клавиатуры. |
| `PlaceholderText` | `string` | `""` | Отображается, когда поле пусто. |
| `SpinButtonPlacementMode` | `NumberBoxSpinButtonPlacement` | `Inline` | `Inline` или `Hidden`. |
| `NumberFormat` | `string` | `null` | Необязательная строка форматирования .NET, например `"F2"`. |

## События

- `ValueChanged` — возникает после подтверждения или изменения значения кнопками вращения.