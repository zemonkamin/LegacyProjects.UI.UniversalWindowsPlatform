# LegacyProjects UI UniversalWindowsPlatform Documentation

This archive contains documentation for the custom UWP controls from `LegacyProjects.UI.UniversalWindowsPlatform`.

Recommended XAML namespaces:

```xml
xmlns:ui="using:LegacyProjects.UI.UniversalWindowsPlatform.Controls"
xmlns:models="using:LegacyProjects.UI.UniversalWindowsPlatform.Models"
```

Controls covered here:

- `UniversalTabBar`
- `UniversalSidebar`
- `UniversalNavigationBar`
- `UniversalBanner`

All examples are written for UWP / Visual Studio 2015 / C# 6 style projects.

## Shared action system

Most interactive elements support `ActionTargetType` on the control and `ClickAction` on the item.

```xml
<ui:UniversalTabBar ActionTargetType="TestUILibrary.AppNavigationActions">
    <ui:UniversalTabBar.Items>
        <models:UniversalTabBarItem Text="Home" Glyph="&#xE80F;" ClickAction="OpenHome" />
    </ui:UniversalTabBar.Items>
</ui:UniversalTabBar>
```

Example action class:

```csharp
namespace TestUILibrary
{
    public sealed class AppNavigationActions
    {
        public static void OpenHome()
        {
            // Navigate here.
        }
    }
}
```

The action method may have no parameters, or it may accept the matching event args, item, index, owner control, string action name, or a sender/args pair when the types match.
