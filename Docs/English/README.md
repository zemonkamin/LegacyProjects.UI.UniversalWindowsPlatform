# LegacyProjects UI UniversalWindowsPlatform Documentation

This archive contains documentation for the custom UWP controls from `LegacyProjects.UI.UniversalWindowsPlatform`.

Recommended XAML namespaces:

```xml
xmlns:ui="using:LegacyProjects.UI.UniversalWindowsPlatform.Controls"
xmlns:models="using:LegacyProjects.UI.UniversalWindowsPlatform.Models"
```

Navigation & layout controls:

- `TabBar`
- `Sidebar`
- `NavigationBar`
- `Banner`

Photo viewing:

- [`PhotoViewer`](PhotoViewer.md) — full-window remote image viewer with zoom, navigation, Save and Share.

New WinUI 2/3 style controls (missing from the Windows 10 SDK 14393):

- `CircularProgress` — determinate/indeterminate ring progress with optional centered text.
- `Expander` — a header with a collapsible content region.
- `SettingsCard` — a settings row with icon, header, description and a trailing action element.
- `NumberBox` — numeric input with spin buttons, range clamping and parsing.
- `PipsPager` — a row of selectable page dots.

All examples are written for UWP / Visual Studio 2015 / C# 6 style projects.

## Shared action system

Most interactive elements support `ActionTargetType` on the control and `ClickAction` on the item.

```xml
<ui:TabBar ActionTargetType="TestUILibrary.AppNavigationActions">
    <ui:TabBar.Items>
        <models:TabBarItem Text="Home" Glyph="&#xE80F;" ClickAction="OpenHome" />
    </ui:TabBar.Items>
</ui:TabBar>
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
