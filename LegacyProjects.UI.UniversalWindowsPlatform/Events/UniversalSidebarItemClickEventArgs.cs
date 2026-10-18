using System;
using LegacyProjects.UI.UniversalWindowsPlatform.Enums;
using LegacyProjects.UI.UniversalWindowsPlatform.Models;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Events
{
    public sealed class UniversalSidebarItemClickEventArgs : EventArgs
    {
        public UniversalSidebarItemClickEventArgs(int index, UniversalSidebarItemSection section, UniversalSidebarItem item, string clickAction)
        {
            Index = index;
            Section = section;
            Item = item;
            ClickAction = clickAction ?? string.Empty;
        }

        public int Index { get; private set; }
        public UniversalSidebarItemSection Section { get; private set; }
        public UniversalSidebarItem Item { get; private set; }
        public string ClickAction { get; private set; }
        public bool Handled { get; set; }
    }
}
