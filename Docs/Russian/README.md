# Документация LegacyProjects UI UniversalWindowsPlatform

В этом архиве лежит документация для кастомных UWP-элементов из `LegacyProjects.UI.UniversalWindowsPlatform`.

Рекомендуемые XAML namespace:

```xml
xmlns:ui="using:LegacyProjects.UI.UniversalWindowsPlatform.Controls"
xmlns:models="using:LegacyProjects.UI.UniversalWindowsPlatform.Models"
```

Элементы в документации:

- `UniversalTabBar`
- `UniversalSidebar`
- `UniversalNavigationBar`
- `UniversalBanner`

Примеры рассчитаны на UWP / Visual Studio 2015 / C# 6.

## Общая система действий

Большинство кликабельных элементов поддерживает `ActionTargetType` у самого контрола и `ClickAction` у отдельного элемента.

```xml
<ui:UniversalTabBar ActionTargetType="TestUILibrary.AppNavigationActions">
    <ui:UniversalTabBar.Items>
        <models:UniversalTabBarItem Text="Home" Glyph="&#xE80F;" ClickAction="OpenHome" />
    </ui:UniversalTabBar.Items>
</ui:UniversalTabBar>
```

Пример класса действий:

```csharp
namespace TestUILibrary
{
    public sealed class AppNavigationActions
    {
        public static void OpenHome()
        {
            // Здесь навигация.
        }
    }
}
```

Метод действия может быть без параметров. Также можно принимать подходящий event args, item, index, сам control, строку action name или пару sender/args, если типы совпадают.
