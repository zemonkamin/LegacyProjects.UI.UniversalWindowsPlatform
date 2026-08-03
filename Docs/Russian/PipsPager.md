# PipsPager

Строка (или столбец) выбираемых точек, представляющих страницы, аналогично `PipsPager` из WinUI 2.6, которого нет в SDK Windows 10 14393. Полезен как индикатор карусели или мастера страниц.

```xml
xmlns:ui="using:LegacyProjects.UI.UniversalWindowsPlatform.Controls"
```

```xml
<ui:PipsPager
    NumberOfPages="5"
    SelectedIndex="0"
    SelectedIndexChanged="OnPageChanged" />
```

Нажатие на точку выбирает её и вызывает `SelectedIndexChanged`. `SelectedIndex` автоматически ограничивается допустимым диапазоном.

## Свойства

| Свойство | Тип | По умолчанию | Назначение |
|---|---|---:|---|
| `NumberOfPages` | `int` | `0` | Количество точек. |
| `SelectedIndex` | `int` | `0` | Выбранная страница (с нуля). |
| `Orientation` | `Orientation` | `Horizontal` | Направление расположения точек. |
| `PipSize` | `double` | `8` | Диаметр точки. |
| `PipSpacing` | `double` | `8` | Расстояние между точками. |
| `SelectedPipBrush` | `Brush` | accent | Цвет выбранной точки. |
| `PipBrush` | `Brush` | приглушённый серый | Цвет невыбранной точки. |

## События

- `SelectedIndexChanged` — предоставляет `OldIndex` и `NewIndex`.