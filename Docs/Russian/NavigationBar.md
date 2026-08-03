# NavigationBar

`NavigationBar` — верхний навбар с левой, центральной и правой зонами элементов. Центральная зона рассчитана на прокрутку, если элементов много.

Важно: навбар задуман только как верхняя панель окна. Его не нужно использовать как нижнюю или плавающую панель.

## Базовый пример

```xml
<ui:NavigationBar
    BarHeight="56"
    Background="#FF111310"
    BackgroundOpacity="0"
    ShowIcons="True"
    ShowText="True"
    SeparatorEnabled="True"
    SeparatorThickness="1"
    SeparatorBrush="#66333333"
    PressAnimationEnabled="True"
    ActionTargetType="TestUILibrary.AppNavigationActions">

    <ui:NavigationBar.LeftItems>
        <models:NavigationBarItem Text="Menu" Glyph="&#xE700;" ClickAction="ToggleSidebar" />
    </ui:NavigationBar.LeftItems>

    <ui:NavigationBar.CenterItems>
        <models:NavigationBarItem Text="Home" Glyph="&#xE80F;" ClickAction="OpenHome" />
        <models:NavigationBarItem Text="Banner" Glyph="&#xE7F4;" ClickAction="OpenBannerTest" />
    </ui:NavigationBar.CenterItems>

    <ui:NavigationBar.RightItems>
        <models:NavigationBarItem Text="Profile" Glyph="&#xE77B;" ClickAction="OpenProfile" />
    </ui:NavigationBar.RightItems>
</ui:NavigationBar>
```

## Секции

| Коллекция | Назначение |
|---|---|
| `LeftItems` | Элементы слева. Обычно кнопка меню/сайдбара. |
| `CenterItems` | Центральные элементы. Если их много, центральная часть может листаться. |
| `RightItems` | Элементы справа. Обычно профиль, настройки, действия. |

## Размеры и расположение

| Свойство | По умолчанию | Что делает |
|---|---:|---|
| `BarHeight` | `56` | Высота навбара. |
| `ItemHeight` | `56` | Высота элемента. |
| `ItemPadding` | `14,0,14,0` | Внутренний отступ элемента. |
| `ItemSpacing` | `0` | Отступ между элементами. |
| `ItemForeground` | theme fallback | Общий цвет текста/иконки. |
| `DisabledForeground` | gray fallback | Цвет неактивных элементов. |
| `DisabledItemOpacity` | `0.45` | Прозрачность неактивных элементов. |

## Прозрачность фона

`BackgroundOpacity` работает как прозрачность:

- `0` — фон совсем не прозрачный. Это значение по умолчанию.
- `0.325` — прозрачность 32.5%.
- `0.9` — прозрачность 90%.
- `1` — максимально прозрачный фон.

```xml
Background="#FF111310"
BackgroundOpacity="0.325"
```

## Нижняя разделительная полоска

У навбара есть нижняя разделительная полоска как у таббара:

```xml
SeparatorEnabled="True"
SeparatorThickness="1"
SeparatorBrush="#66333333"
```

## Иконки и текст

```xml
ShowIcons="True"
ShowText="True"
IconSize="18"
IconTextSpacing="8"
LabelFontSize="14"
```

Глиф:

```xml
<models:NavigationBarItem Text="Menu" IconType="Glyph" Glyph="&#xE700;" />
```

Картинка:

```xml
<models:NavigationBarItem Text="Logo" IconType="Image" ImagePath="Assets/Square150x150Logo.scale-200.png" />
```

Настройка отдельного элемента:

```xml
<models:NavigationBarItem
    Text="Profile"
    Glyph="&#xE77B;"
    Background="#220078D7"
    Foreground="#FFFFFFFF"
    DisabledForeground="#FF777777"
    Width="120"
    MinWidth="80" />
```

## Неактивные элементы

```xml
<models:NavigationBarItem Text="Disabled" Glyph="&#xE7C3;" IsEnabled="False" />
```

Неактивный элемент не нажимается и не вызывает `ClickAction`.

## Анимация нажатия

```xml
PressAnimationEnabled="True"
PressAnimationScale="0.96"
PressAnimationDuration="90"
```

Выключение:

```xml
PressAnimationEnabled="False"
```

## Действия при нажатии

```xml
<ui:NavigationBar ActionTargetType="TestUILibrary.AppNavigationActions">
    <ui:NavigationBar.LeftItems>
        <models:NavigationBarItem Text="Menu" Glyph="&#xE700;" ClickAction="ToggleSidebar" />
    </ui:NavigationBar.LeftItems>
</ui:NavigationBar>
```

```csharp
public sealed class AppNavigationActions
{
    public static void ToggleSidebar() { }
}
```

Также у контрола есть событие `ItemClick` для работы из code-behind.
