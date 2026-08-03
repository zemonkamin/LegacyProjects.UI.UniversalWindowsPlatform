using System;
using LegacyProjects.UI.UniversalWindowsPlatform.Models;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Events
{
    public sealed class BannerItemClickEventArgs : EventArgs
    {
        public BannerItemClickEventArgs(int index, BannerItem item, string clickAction)
        {
            Index = index;
            Item = item;
            ClickAction = clickAction ?? string.Empty;
        }

        public int Index { get; private set; }
        public BannerItem Item { get; private set; }
        public string ClickAction { get; private set; }
        public bool Handled { get; set; }
    }
}
