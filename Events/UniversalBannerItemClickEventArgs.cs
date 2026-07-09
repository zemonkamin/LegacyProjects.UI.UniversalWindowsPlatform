using System;
using LegacyProjects.UI.UniversalWindowsPlatform.Models;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Events
{
    public sealed class UniversalBannerItemClickEventArgs : EventArgs
    {
        public UniversalBannerItemClickEventArgs(int index, UniversalBannerItem item, string clickAction)
        {
            Index = index;
            Item = item;
            ClickAction = clickAction ?? string.Empty;
        }

        public int Index { get; private set; }
        public UniversalBannerItem Item { get; private set; }
        public string ClickAction { get; private set; }
        public bool Handled { get; set; }
    }
}
