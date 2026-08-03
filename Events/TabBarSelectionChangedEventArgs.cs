using System;
using LegacyProjects.UI.UniversalWindowsPlatform.Models;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Events
{
    public sealed class TabBarSelectionChangedEventArgs : EventArgs
    {
        public TabBarSelectionChangedEventArgs(int oldIndex, int newIndex, TabBarItem oldItem, TabBarItem newItem)
        {
            OldIndex = oldIndex;
            NewIndex = newIndex;
            OldItem = oldItem;
            NewItem = newItem;
        }

        public int OldIndex { get; private set; }

        public int NewIndex { get; private set; }

        public TabBarItem OldItem { get; private set; }

        public TabBarItem NewItem { get; private set; }
    }
}
