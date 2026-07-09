# UniversalSidebar

`UniversalSidebar` — настраиваемый сайдбар с верхними, средними и нижними кнопками. Верхние и нижние кнопки можно закрепить, а среднюю часть сделать прокручиваемой. Если верхние и нижние не закреплены, сайдбар можно использовать как общий прокручиваемый список.

## Базовый пример

```xml
<ui:UniversalSidebar
    x:Name="Sidebar"
    OpenPaneWidth="300"
    CompactPaneWidth="56"
    HorizontalBreakpoint="768"
    Background="#FF111310"
    BackgroundOpacity="0"
    ShowIcons="True"
    ShowIconsWhenClosedInHorizontalMode="True"
    TopItemsPinned="True"
    BottomItemsPinned="True"
    SideSeparatorEnabled="True"
    SeparatorThickness="1"
    SeparatorBrush="#66333333"
    AnimationEnabled="True"
    AnimationDuration="220"
    AnimationEasingFunction="Cubic"
    AnimationEasingMode="EaseOut"
    PressAnimationEnabled="True"
    ActionTargetType="TestUILibrary.AppNavigationActions">

    <ui:UniversalSidebar.TopItems>
        <models:UniversalSidebarItem Text="Home" Key="home" Glyph="&#xE80F;" ClickAction="OpenHome" />
    </ui:UniversalSidebar.TopItems>

    <ui:UniversalSidebar.Items>
        <models:UniversalSidebarItem Text="Banner" Key="banner" Glyph="&#xE7F4;" ClickAction="OpenBannerTest" />
        <models:UniversalSidebarItem Text="Disabled" Glyph="&#xE7C3;" IsEnabled="False" />
    </ui:UniversalSidebar.Items>

    <ui:UniversalSidebar.BottomItems>
        <models:UniversalSidebarItem Text="Settings" Key="settings" Glyph="&#xE713;" />
    </ui:UniversalSidebar.BottomItems>
</ui:UniversalSidebar>
```

Открытие/закрытие из code-behind:

```csharp
private void ToggleSidebar_Click(object sender, RoutedEventArgs e)
{
    Sidebar.IsPaneOpen = !Sidebar.IsPaneOpen;
}
```

## Группы кнопок

| Коллекция | Назначение |
|---|---|
| `TopItems` | Кнопки сверху. Обычно Home, основная навигация, кнопка меню. |
| `Items` | Средние/обычные кнопки. Обычно это прокручиваемая часть. |
| `BottomItems` | Кнопки снизу. Обычно Settings, Account, About. |

Закрепление:

```xml
TopItemsPinned="True"
BottomItemsPinned="True"
```

- Если `TopItemsPinned="True"`, верхние элементы остаются на месте, а средняя часть листается.
- Если `BottomItemsPinned="True"`, нижние элементы остаются снизу, а средняя часть листается.
- Если оба значения `False`, сайдбар можно использовать как общий прокручиваемый список.

## Ширина и responsive-режим

| Свойство | По умолчанию | Что делает |
|---|---:|---|
| `IsPaneOpen` | `False` | Текущее состояние: открыт или закрыт. |
| `OpenPaneWidth` | `300` | Ширина в открытом состоянии. |
| `CompactPaneWidth` | `56` | Ширина в закрытом горизонтальном режиме. |
| `HorizontalBreakpoint` | `768` | Ширина окна, с которой начинается горизонтальный режим. |
| `ShowIconsWhenClosedInHorizontalMode` | `False` | Показывать иконки в закрытом горизонтальном режиме. |

`ShowIconsWhenClosedInHorizontalMode="True"` удобно, если закрытый сайдбар должен показывать компактные иконки.

## Цвета и прозрачность

`BackgroundOpacity` работает как прозрачность:

- `0` — фон совсем не прозрачный. Это значение по умолчанию.
- `0.325` — прозрачность 32.5%.
- `0.9` — прозрачность 90%.
- `1` — максимально прозрачный фон.

Основные свойства цветов:

| Свойство | По умолчанию | Что делает |
|---|---:|---|
| `Background` | фон страницы/темы | Фон сайдбара. |
| `BackgroundOpacity` | `0` | Прозрачность фона, где `1` полностью прозрачно. |
| `SelectedItemBackground` | accent fallback | Фон выбранной кнопки. |
| `UnselectedItemBackground` | transparent | Фон обычной кнопки. |
| `DisabledItemBackground` | transparent | Фон неактивной кнопки. |
| `SelectedItemForeground` | white/accent fallback | Цвет текста/иконки выбранной кнопки. |
| `UnselectedItemForeground` | theme fallback | Цвет текста/иконки обычной кнопки. |
| `DisabledItemForeground` | gray fallback | Цвет текста/иконки неактивной кнопки. |
| `DisabledItemOpacity` | `0.45` | Затемнение неактивных кнопок. |

Индивидуальные цвета можно задать у конкретной кнопки:

```xml
<models:UniversalSidebarItem
    Text="Custom"
    Glyph="&#xE8A7;"
    SelectedBackground="#FF005A9E"
    UnselectedBackground="Transparent"
    DisabledBackground="#22222222"
    SelectedForeground="White"
    UnselectedForeground="#FFCCCCCC"
    DisabledForeground="#FF777777" />
```

## Боковая разделительная полоска

```xml
SideSeparatorEnabled="True"
SeparatorThickness="1"
SeparatorBrush="#66333333"
```

Это вертикальная линия, которая отделяет сайдбар от контента страницы. Настраивается через `SeparatorThickness` и `SeparatorBrush`.

## Иконки

Глиф:

```xml
<models:UniversalSidebarItem Text="Home" IconType="Glyph" Glyph="&#xE80F;" />
```

Картинка:

```xml
<models:UniversalSidebarItem Text="Logo" IconType="Image" ImagePath="Assets/Square150x150Logo.scale-200.png" />
```

Общие настройки иконок:

```xml
ShowIcons="True"
IconSize="18"
IconTextSpacing="16"
```

## Неактивные кнопки

```xml
<models:UniversalSidebarItem Text="Disabled" Glyph="&#xE7C3;" IsEnabled="False" />
```

Неактивная кнопка не нажимается и не вызывает `ClickAction`.

## Анимация открытия и закрытия

```xml
AnimationEnabled="True"
AnimationDuration="220"
AnimationEasingFunction="Cubic"
AnimationEasingMode="EaseOut"
```

Поддерживаемые easing functions:

- `Cubic`
- `Quadratic`
- `Back`
- `Circle`
- `Exponential`

Поддерживаемые easing modes:

- `EaseOut`
- `EaseIn`
- `EaseInOut`

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
<ui:UniversalSidebar ActionTargetType="TestUILibrary.AppNavigationActions">
    <ui:UniversalSidebar.Items>
        <models:UniversalSidebarItem Text="Home" Glyph="&#xE80F;" ClickAction="OpenHome" />
    </ui:UniversalSidebar.Items>
</ui:UniversalSidebar>
```

```csharp
public sealed class AppNavigationActions
{
    public static void OpenHome() { }
}
```

Также у контрола есть события `ItemClick` и `SelectionChanged` для работы из code-behind.
