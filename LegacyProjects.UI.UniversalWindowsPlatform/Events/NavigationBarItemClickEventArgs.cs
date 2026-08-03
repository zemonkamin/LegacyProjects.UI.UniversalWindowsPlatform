using System;
using LegacyProjects.UI.UniversalWindowsPlatform.Enums;
using LegacyProjects.UI.UniversalWindowsPlatform.Models;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Events
{
    public sealed class NavigationBarItemClickEventArgs : EventArgs
    {
        public NavigationBarItemClickEventArgs(int index, NavigationBarItemSection section, NavigationBarItem item, string clickAction)
        {
            Index = index;
            Section = section;
            Item = item;
            ClickAction = clickAction ?? string.Empty;
        }

        public int Index { get; private set; }
        public NavigationBarItemSection Section { get; private set; }
        public NavigationBarItem Item { get; private set; }
        public string ClickAction { get; private set; }
        public bool Handled { get; set; }
    }
}
