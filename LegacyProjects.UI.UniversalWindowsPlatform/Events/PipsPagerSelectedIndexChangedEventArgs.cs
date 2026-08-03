using System;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Events
{
    /// <summary>
    /// Provides data for the <see cref="Controls.PipsPager.SelectedIndexChanged"/> event.
    /// </summary>
    public sealed class PipsPagerSelectedIndexChangedEventArgs : EventArgs
    {
        public PipsPagerSelectedIndexChangedEventArgs(int oldIndex, int newIndex)
        {
            OldIndex = oldIndex;
            NewIndex = newIndex;
        }

        /// <summary>The previously selected page index.</summary>
        public int OldIndex { get; private set; }

        /// <summary>The newly selected page index.</summary>
        public int NewIndex { get; private set; }
    }
}
