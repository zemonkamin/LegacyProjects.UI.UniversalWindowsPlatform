using System;
using LegacyProjects.UI.UniversalWindowsPlatform.Models;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Events
{
    public sealed class UniversalTabBarSelectionChangedEventArgs : EventArgs
    {
        public UniversalTabBarSelectionChangedEventArgs(int oldIndex, int newIndex, UniversalTabBarItem oldItem, UniversalTabBarItem newItem)
        {
            OldIndex = oldIndex;
            NewIndex = newIndex;
            OldItem = oldItem;
            NewItem = newItem;
        }

        public int OldIndex { get; private set; }

        public int NewIndex { get; private set; }

        public UniversalTabBarItem OldItem { get; private set; }

        public UniversalTabBarItem NewItem { get; private set; }
    }
}
