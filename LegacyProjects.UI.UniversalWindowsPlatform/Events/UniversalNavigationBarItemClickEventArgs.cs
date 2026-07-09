using System;
using LegacyProjects.UI.UniversalWindowsPlatform.Enums;
using LegacyProjects.UI.UniversalWindowsPlatform.Models;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Events
{
    public sealed class UniversalNavigationBarItemClickEventArgs : EventArgs
    {
        public UniversalNavigationBarItemClickEventArgs(int index, UniversalNavigationBarItemSection section, UniversalNavigationBarItem item, string clickAction)
        {
            Index = index;
            Section = section;
            Item = item;
            ClickAction = clickAction ?? string.Empty;
        }

        public int Index { get; private set; }
        public UniversalNavigationBarItemSection Section { get; private set; }
        public UniversalNavigationBarItem Item { get; private set; }
        public string ClickAction { get; private set; }
        public bool Handled { get; set; }
    }
}
