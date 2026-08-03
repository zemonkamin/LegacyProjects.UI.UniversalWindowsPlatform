using System;
using LegacyProjects.UI.UniversalWindowsPlatform.Enums;
using LegacyProjects.UI.UniversalWindowsPlatform.Models;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Events
{
    public sealed class SidebarItemClickEventArgs : EventArgs
    {
        public SidebarItemClickEventArgs(int index, SidebarItemSection section, SidebarItem item, string clickAction)
        {
            Index = index;
            Section = section;
            Item = item;
            ClickAction = clickAction ?? string.Empty;
        }

        public int Index { get; private set; }
        public SidebarItemSection Section { get; private set; }
        public SidebarItem Item { get; private set; }
        public string ClickAction { get; private set; }
        public bool Handled { get; set; }
    }
}
