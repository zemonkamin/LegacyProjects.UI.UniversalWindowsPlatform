# SettingsCard

Строка параметров: необязательный значок слева, заголовок и описание, а также элемент действия справа. Аналог `SettingsCard` из WinUI Community Toolkit. Элемент действия — это содержимое XAML.

```xml
xmlns:ui="using:LegacyProjects.UI.UniversalWindowsPlatform.Controls"
```

## С элементом управления справа

```xml
<ui:SettingsCard
    Header="Тема"
    Description="Выберите внешний вид приложения."
    IconGlyph="&#xE771;">
    <ComboBox>
        <ComboBoxItem Content="Светлая" />
        <ComboBoxItem Content="Тёмная" />
    </ComboBox>
</ui:SettingsCard>
```

## Как строка навигации с действием

```xml
<ui:SettingsCard
    Header="О программе"
    Description="Версия и лицензии"
    IconGlyph="&#xE946;"
    IsClickable="True"
    Click="OnAboutClick" />
```

Когда `IsClickable` равен `true`, вся карточка вызывает `Click` и показывает указатель.

## Свойства

| Свойство | Тип | По умолчанию | Назначение |
|---|---|---:|---|
| `Header` | `string` | `""` | Основной заголовок. |
| `Description` | `string` | `""` | Вторая строка; скрыта, когда пусто. |
| `IconGlyph` | `string` | `""` | Необязательный ведущий глиф. |
| `IconFontFamily` | `string` | `Segoe MDL2 Assets` | Шрифт для глифа. |
| `ActionContent` | `object` | `null` | Элемент справа (свойство содержимого XAML). |
| `IsClickable` | `bool` | `False` | Вся карточка ведёт себя как кнопка. |
| `CardBackground` | `Brush` | theme | Фон карточки. |
| `CornerRadiusValue` | `CornerRadius` | `4` | Радиус скругления. |

## События

- `Click` — возникает, когда `IsClickable` равен `true` и карточка нажата.