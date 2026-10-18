using System;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Helpers
{
    internal static class PhotoViewerPolicy
    {
        internal static int Index(int requested, int count) { return count <= 0 ? -1 : Math.Max(0, Math.Min(requested, count - 1)); }
        internal static int FirstIndicator(int selected, int count) { return Math.Max(0, Math.Min(selected - 4, count - 9)); }
        internal static bool DecodeSize(uint sourceWidth, uint sourceHeight, bool mobile, out uint width, out uint height)
        {
            width = height = 0;
            if (sourceWidth == 0 || sourceHeight == 0 || sourceWidth > 65535 || sourceHeight > 65535
                || (long)sourceWidth * sourceHeight > 268435456) return false;
            int edge = mobile ? 1280 : 2560;
            double scale = Math.Min(1, (double)edge / Math.Max(sourceWidth, sourceHeight));
            scale = Math.Min(scale, Math.Sqrt((mobile ? 1048576.0 : 4194304.0) / ((long)sourceWidth * sourceHeight)));
            width = Math.Max(1, (uint)(sourceWidth * scale)); height = Math.Max(1, (uint)(sourceHeight * scale));
            return true;
        }
    }
}
