# UniversalTabBar

`UniversalTabBar` — настраиваемый таббар. Его можно ставить сверху, снизу или задавать свободное вертикальное смещение. По горизонтали он может быть слева, справа, на всю ширину или со свободным смещением.

## Базовый пример

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

## Положение и размер

| Свойство | Значения / тип | По умолчанию | Что делает |
|---|---:|---:|---|
| `VerticalPlacement` | `Top`, `Bottom`, `Custom` | `Bottom` | Вертикальное положение. |
| `HorizontalPlacement` | `Left`, `Right`, `Stretch`, `Custom` | `Stretch` | Горизонтальное положение. |
| `BarHeight` | `double` | `56` | Высота таббара. |
| `BarWidth` | `double` | `NaN` | Своя ширина. Используется со `Left`, `Right` или `Custom`. |
| `CustomVerticalOffset` | `double` | `0` | Y-смещение при `VerticalPlacement="Custom"`. |
| `CustomHorizontalOffset` | `double` | `0` | X-смещение при `HorizontalPlacement="Custom"`. |
| `Orientation` | `Horizontal`, `Vertical` | `Horizontal` | Направление элементов. |

## Цвета и прозрачность

`BackgroundOpacity` работает как прозрачность наоборот относительно обычной opacity:

- `0` — совсем не прозрачный фон. Это значение по умолчанию.
- `0.325` — прозрачность 32.5%.
- `0.9` — прозрачность 90%.
- `1` — максимально прозрачный фон.

Общие цвета:

| Свойство | По умолчанию | Что делает |
|---|---:|---|
| `Background` | фон страницы/темы | Фон таббара. |
| `BackgroundOpacity` | `0` | Прозрачность фона, где `1` полностью прозрачно. |
| `SelectedTabForeground` | системный accent | Общий цвет выбранной иконки/текста. |
| `UnselectedTabForeground` | серый | Общий цвет невыбранной иконки/текста. |
| `DisabledItemOpacity` | `0.45` | Прозрачность неактивных вкладок. |

Индивидуальные цвета у вкладки перекрывают общие цвета:

```xml
<models:UniversalTabBarItem
    Text="Profile"
    Glyph="&#xE77B;"
    SelectedIconForeground="#FF00B7FF"
    UnselectedIconForeground="#FFFF4444"
    SelectedTextForeground="#FF00B7FF"
    UnselectedTextForeground="#FFFF4444" />
```

Свойства цветов у элемента:

- `IconForeground`
- `TextForeground`
- `SelectedIconForeground`
- `UnselectedIconForeground`
- `SelectedTextForeground`
- `UnselectedTextForeground`

Если индивидуальный цвет не задан, используется `SelectedTabForeground` или `UnselectedTabForeground`.

## Разделительная полоска

Полоску можно включить, выключить и настроить. Сторона полоски выбирается автоматически по положению таббара. В свободном/плавающем положении контрол может использовать несколько сторон, чтобы отделить таббар от контента.

```xml
SeparatorEnabled="True"
SeparatorThickness="1"
SeparatorBrush="#66333333"
```

## Иконки и текст

```xml
ShowIcons="True"
ShowText="True"
TextPosition="BelowIcon"
IconSize="20"
LabelFontSize="11"
```

Значения `TextPosition`:

- `BelowIcon` — текст под иконкой.
- `AboveIcon` — текст над иконкой.

Глиф-иконка:

```xml
<models:UniversalTabBarItem Text="Home" IconType="Glyph" Glyph="&#xE80F;" />
```

Иконка-картинка:

```xml
<models:UniversalTabBarItem Text="Logo" IconType="Image" ImagePath="Assets/Square150x150Logo.scale-200.png" />
```

## Эффект нажатия

Общая настройка:

```xml
PressEffectEnabled="False"
```

Переопределение для одной вкладки:

```xml
<models:UniversalTabBarItem Text="Home" Glyph="&#xE80F;" PressEffectMode="Disabled" />
```

Значения `PressEffectMode`:

- `Default` — берёт значение из `PressEffectEnabled` у таббара.
- `Enabled` — включает эффект только для этой вкладки.
- `Disabled` — выключает эффект только для этой вкладки.

## Неактивные вкладки

```xml
<models:UniversalTabBarItem Text="Library" Glyph="&#xE8F1;" IsEnabled="False" />
```

Неактивная вкладка не нажимается, не вызывает `ClickAction` и затемняется через `DisabledItemOpacity`.

## Действия при нажатии

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

У таббара есть экспериментальные параметры glass-эффекта:

```xml
GlassEffectEnabled="True"
GlassTintColor="#FF00FF00"
GlassTintAmount="0.5"
GlassBlurAmount="0.9"
GlassShaderPath="Shaders/GlassEffect.fx"
```

Важно: glass effect не рекомендуется для обычного использования. На старых UWP target-версиях он может работать нестабильно, может мигать при прокрутке и может быть тяжелее для слабых устройств. Лучше использовать обычный `Background` вместе с `BackgroundOpacity`, если стекло не является обязательным.

Параметры glass effect:

| Свойство | По умолчанию | Что делает |
|---|---:|---|
| `GlassEffectEnabled` | `False` | Включает экспериментальный стеклянный фон. |
| `GlassTintColor` | transparent | Цвет тонировки. |
| `GlassTintAmount` | `0` | Сила тонировки от `0` до `1`. |
| `GlassBlurAmount` | `0` | Сила размытия от `0` до `1`. |
| `GlassShaderPath` | `Shaders/GlassEffect.fx` | Путь к shader-файлу. |

Метод `RefreshGlassEffect()` из code-behind нужен только если фон под таббаром изменился и нужно вручную обновить software fallback.
