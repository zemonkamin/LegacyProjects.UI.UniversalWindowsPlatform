using System;
using LegacyProjects.UI.UniversalWindowsPlatform.Models;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Events
{
    public sealed class SidebarSelectionChangedEventArgs : EventArgs
    {
        public SidebarSelectionChangedEventArgs(int oldIndex, int newIndex, SidebarItem oldItem, SidebarItem newItem)
        {
            OldIndex = oldIndex;
            NewIndex = newIndex;
            OldItem = oldItem;
            NewItem = newItem;
        }

        public int OldIndex { get; private set; }
        public int NewIndex { get; private set; }
        public SidebarItem OldItem { get; private set; }
        public SidebarItem NewItem { get; private set; }
    }
}
