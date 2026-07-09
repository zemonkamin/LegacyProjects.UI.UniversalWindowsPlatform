using System;
using LegacyProjects.UI.UniversalWindowsPlatform.Models;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Events
{
    public sealed class UniversalTabBarItemClickEventArgs : EventArgs
    {
        public UniversalTabBarItemClickEventArgs(int index, UniversalTabBarItem item, string clickAction)
        {
            Index = index;
            Item = item;
            ClickAction = clickAction;
        }

        public int Index { get; private set; }
        public UniversalTabBarItem Item { get; private set; }
        public string ClickAction { get; private set; }
        public bool Handled { get; set; }
    }
}
