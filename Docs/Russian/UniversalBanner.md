# UniversalBanner

`UniversalBanner` — баннер/карусель с картинками. У каждого элемента баннера может быть изображение, основной текст, второй текст, отдельные цвета для обоих текстов и действие при нажатии.

## Базовый пример с двумя картинками

```xml
<ui:UniversalBanner
    Height="260"
    AutoScrollEnabled="True"
    ItemDisplayDuration="3000"
    SlideAnimationEnabled="True"
    SlideAnimationDuration="280"
    ScrollDirection="Horizontal"
    ShowIndicators="True"
    IndicatorPlacement="Bottom"
    PrimaryTextMaxLength="38"
    SecondaryTextMaxLength="60"
    ActionTargetType="TestUILibrary.AppNavigationActions">

    <ui:UniversalBanner.Items>
        <models:UniversalBannerItem
            ImagePath="Assets/GlassBackground.jpg"
            PrimaryText="First test title"
            SecondaryText="First test secondary text"
            PrimaryTextForeground="White"
            SecondaryTextForeground="#FFE0E0E0"
            ClickAction="OpenHome" />

        <models:UniversalBannerItem
            ImagePath="Assets/Square150x150Logo.scale-200.png"
            PrimaryText="Second test title"
            SecondaryText="Second test secondary text"
            PrimaryTextForeground="#FF00B7FF"
            SecondaryTextForeground="#FFFFFFFF"
            ClickAction="OpenProfile" />
    </ui:UniversalBanner.Items>
</ui:UniversalBanner>
```

## Пример баннера с одним содержимым

```xml
<ui:UniversalBanner
    Height="180"
    AutoScrollEnabled="False"
    ShowIndicators="False"
    SlideAnimationEnabled="False">
    <ui:UniversalBanner.Items>
        <models:UniversalBannerItem
            ImagePath="Assets/GlassBackground.jpg"
            PrimaryText="Single content banner"
            SecondaryText="This banner has only one item." />
    </ui:UniversalBanner.Items>
</ui:UniversalBanner>
```

## Структура элемента баннера

| Свойство элемента | Что делает |
|---|---|
| `Key` | Необязательный логический идентификатор. |
| `ImagePath` | Путь к картинке, например `Assets/GlassBackground.jpg`. |
| `PrimaryText` | Основной текст. |
| `SecondaryText` | Второй текст. |
| `PrimaryTextForeground` | Цвет основного текста конкретного элемента. |
| `SecondaryTextForeground` | Цвет второго текста конкретного элемента. |
| `ClickAction` | Имя действия, которое вызывается при нажатии. |
| `Tag` | Необязательные пользовательские данные. |

## Прокрутка и время

| Свойство | По умолчанию | Что делает |
|---|---:|---|
| `SelectedIndex` | `0` | Текущий индекс элемента. |
| `AutoScrollEnabled` | `True` | Автоматически переключать элементы. |
| `ItemDisplayDuration` | `3000` | Сколько показывается один элемент, в миллисекундах. |
| `SlideAnimationEnabled` | `True` | Включает анимацию пролистывания. |
| `SlideAnimationDuration` | `280` | Длительность анимации пролистывания, в миллисекундах. |
| `ScrollDirection` | `Horizontal` | `Horizontal` или `Vertical`. |

Вертикальная прокрутка:

```xml
ScrollDirection="Vertical"
```

Выключить анимацию пролистывания:

```xml
SlideAnimationEnabled="False"
```

## Точки-индикаторы

| Свойство | По умолчанию | Что делает |
|---|---:|---|
| `ShowIndicators` | `True` | Показывает/скрывает точки. |
| `IndicatorPlacement` | `Bottom` | `Bottom`, `Top`, `Left` или `Right`. |
| `IndicatorSize` | `8` | Размер точки. |
| `IndicatorSpacing` | `6` | Отступ между точками. |
| `IndicatorActiveBrush` | theme/accent fallback | Цвет активной точки. |
| `IndicatorInactiveBrush` | fallback brush | Цвет неактивной точки. |

Если `IndicatorPlacement="Left"` или `IndicatorPlacement="Right"`, точки идут вертикально.

Пример:

```xml
ShowIndicators="True"
IndicatorPlacement="Right"
IndicatorSize="10"
IndicatorSpacing="8"
IndicatorActiveBrush="#FFFFFFFF"
IndicatorInactiveBrush="#66FFFFFF"
```

## Текстовый слой и ограничение длины текста

| Свойство | По умолчанию | Что делает |
|---|---:|---|
| `TextOverlayHeight` | `150` | Высота области текста. |
| `TextPadding` | `24,0,24,30` | Отступы текста. |
| `PrimaryTextForeground` | white fallback | Общий цвет основного текста. |
| `SecondaryTextForeground` | light fallback | Общий цвет второго текста. |
| `PrimaryTextFontSize` | `22` | Размер основного текста. |
| `SecondaryTextFontSize` | `18` | Размер второго текста. |
| `PrimaryTextMaxLength` | `0` | Максимальная длина основного текста. `0` — не обрезать. |
| `SecondaryTextMaxLength` | `0` | Максимальная длина второго текста. `0` — не обрезать. |

Если текст длиннее указанного лимита, он обрезается и в конце добавляется `...`.

Пример:

```xml
PrimaryTextMaxLength="32"
SecondaryTextMaxLength="70"
```

## Картинка и overlay

| Свойство | По умолчанию | Что делает |
|---|---:|---|
| `ImageStretch` | `UniformToFill` | Режим растягивания картинки. |
| `BackgroundOverlayEnabled` | `True` | Включает затемняющий/градиентный слой под текстом. |

```xml
ImageStretch="UniformToFill"
BackgroundOverlayEnabled="True"
```

## Действия при нажатии

```xml
<ui:UniversalBanner ActionTargetType="TestUILibrary.AppNavigationActions">
    <ui:UniversalBanner.Items>
        <models:UniversalBannerItem ImagePath="Assets/GlassBackground.jpg" PrimaryText="Open" ClickAction="OpenHome" />
    </ui:UniversalBanner.Items>
</ui:UniversalBanner>
```

```csharp
public sealed class AppNavigationActions
{
    public static void OpenHome() { }
}
```

Также у контрола есть событие `ItemClick`.
