using System;
using LegacyProjects.UI.UniversalWindowsPlatform.Models;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Events
{
    public sealed class UniversalSidebarSelectionChangedEventArgs : EventArgs
    {
        public UniversalSidebarSelectionChangedEventArgs(int oldIndex, int newIndex, UniversalSidebarItem oldItem, UniversalSidebarItem newItem)
        {
            OldIndex = oldIndex;
            NewIndex = newIndex;
            OldItem = oldItem;
            NewItem = newItem;
        }

        public int OldIndex { get; private set; }
        public int NewIndex { get; private set; }
        public UniversalSidebarItem OldItem { get; private set; }
        public UniversalSidebarItem NewItem { get; private set; }
    }
}
