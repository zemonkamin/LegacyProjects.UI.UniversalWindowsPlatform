using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Models
{
    public sealed class UniversalBannerItem : DependencyObject
    {
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(
            "Key",
            typeof(string),
            typeof(UniversalBannerItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty ImagePathProperty = DependencyProperty.Register(
            "ImagePath",
            typeof(string),
            typeof(UniversalBannerItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty PrimaryTextProperty = DependencyProperty.Register(
            "PrimaryText",
            typeof(string),
            typeof(UniversalBannerItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty SecondaryTextProperty = DependencyProperty.Register(
            "SecondaryText",
            typeof(string),
            typeof(UniversalBannerItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty PrimaryTextForegroundProperty = DependencyProperty.Register(
            "PrimaryTextForeground",
            typeof(Brush),
            typeof(UniversalBannerItem),
            new PropertyMetadata(null, OnItemPropertyChanged));

        public static readonly DependencyProperty SecondaryTextForegroundProperty = DependencyProperty.Register(
            "SecondaryTextForeground",
            typeof(Brush),
            typeof(UniversalBannerItem),
            new PropertyMetadata(null, OnItemPropertyChanged));

        public static readonly DependencyProperty ClickActionProperty = DependencyProperty.Register(
            "ClickAction",
            typeof(string),
            typeof(UniversalBannerItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty TagProperty = DependencyProperty.Register(
            "Tag",
            typeof(object),
            typeof(UniversalBannerItem),
            new PropertyMetadata(null, OnItemPropertyChanged));

        internal event EventHandler Changed;

        public string Key
        {
            get { return (string)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }

        public string PrimaryText
        {
            get { return (string)GetValue(PrimaryTextProperty); }
            set { SetValue(PrimaryTextProperty, value); }
        }

        public string SecondaryText
        {
            get { return (string)GetValue(SecondaryTextProperty); }
            set { SetValue(SecondaryTextProperty, value); }
        }

        public Brush PrimaryTextForeground
        {
            get { return (Brush)GetValue(PrimaryTextForegroundProperty); }
            set { SetValue(PrimaryTextForegroundProperty, value); }
        }

        public Brush SecondaryTextForeground
        {
            get { return (Brush)GetValue(SecondaryTextForegroundProperty); }
            set { SetValue(SecondaryTextForegroundProperty, value); }
        }

        public string ClickAction
        {
            get { return (string)GetValue(ClickActionProperty); }
            set { SetValue(ClickActionProperty, value); }
        }

        public object Tag
        {
            get { return GetValue(TagProperty); }
            set { SetValue(TagProperty, value); }
        }

        private static void OnItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            UniversalBannerItem item = sender as UniversalBannerItem;
            if (item != null && item.Changed != null)
            {
                item.Changed(item, EventArgs.Empty);
            }
        }
    }
}
