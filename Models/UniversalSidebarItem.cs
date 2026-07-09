using System;
using LegacyProjects.UI.UniversalWindowsPlatform.Enums;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Models
{
    public sealed class UniversalSidebarItem : DependencyObject
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(UniversalSidebarItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(
            "Key",
            typeof(string),
            typeof(UniversalSidebarItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(
            "Glyph",
            typeof(string),
            typeof(UniversalSidebarItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty ImagePathProperty = DependencyProperty.Register(
            "ImagePath",
            typeof(string),
            typeof(UniversalSidebarItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty IconTypeProperty = DependencyProperty.Register(
            "IconType",
            typeof(UniversalSidebarIconType),
            typeof(UniversalSidebarItem),
            new PropertyMetadata(UniversalSidebarIconType.Glyph, OnItemPropertyChanged));

        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register(
            "IconFontFamily",
            typeof(string),
            typeof(UniversalSidebarItem),
            new PropertyMetadata("Segoe MDL2 Assets", OnItemPropertyChanged));

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
            "IsEnabled",
            typeof(bool),
            typeof(UniversalSidebarItem),
            new PropertyMetadata(true, OnItemPropertyChanged));

        public static readonly DependencyProperty ClickActionProperty = DependencyProperty.Register(
            "ClickAction",
            typeof(string),
            typeof(UniversalSidebarItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty SelectedBackgroundProperty = DependencyProperty.Register(
            "SelectedBackground",
            typeof(Brush),
            typeof(UniversalSidebarItem),
            new PropertyMetadata(null, OnItemPropertyChanged));

        public static readonly DependencyProperty UnselectedBackgroundProperty = DependencyProperty.Register(
            "UnselectedBackground",
            typeof(Brush),
            typeof(UniversalSidebarItem),
            new PropertyMetadata(null, OnItemPropertyChanged));

        public static readonly DependencyProperty DisabledBackgroundProperty = DependencyProperty.Register(
            "DisabledBackground",
            typeof(Brush),
            typeof(UniversalSidebarItem),
            new PropertyMetadata(null, OnItemPropertyChanged));

        public static readonly DependencyProperty SelectedForegroundProperty = DependencyProperty.Register(
            "SelectedForeground",
            typeof(Brush),
            typeof(UniversalSidebarItem),
            new PropertyMetadata(null, OnItemPropertyChanged));

        public static readonly DependencyProperty UnselectedForegroundProperty = DependencyProperty.Register(
            "UnselectedForeground",
            typeof(Brush),
            typeof(UniversalSidebarItem),
            new PropertyMetadata(null, OnItemPropertyChanged));

        public static readonly DependencyProperty DisabledForegroundProperty = DependencyProperty.Register(
            "DisabledForeground",
            typeof(Brush),
            typeof(UniversalSidebarItem),
            new PropertyMetadata(null, OnItemPropertyChanged));

        internal event EventHandler Changed;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string Key
        {
            get { return (string)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        public string Glyph
        {
            get { return (string)GetValue(GlyphProperty); }
            set { SetValue(GlyphProperty, value); }
        }

        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            set { SetValue(ImagePathProperty, value); }
        }

        public UniversalSidebarIconType IconType
        {
            get { return (UniversalSidebarIconType)GetValue(IconTypeProperty); }
            set { SetValue(IconTypeProperty, value); }
        }

        public string IconFontFamily
        {
            get { return (string)GetValue(IconFontFamilyProperty); }
            set { SetValue(IconFontFamilyProperty, value); }
        }

        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        public string ClickAction
        {
            get { return (string)GetValue(ClickActionProperty); }
            set { SetValue(ClickActionProperty, value); }
        }

        public Brush SelectedBackground
        {
            get { return (Brush)GetValue(SelectedBackgroundProperty); }
            set { SetValue(SelectedBackgroundProperty, value); }
        }

        public Brush UnselectedBackground
        {
            get { return (Brush)GetValue(UnselectedBackgroundProperty); }
            set { SetValue(UnselectedBackgroundProperty, value); }
        }

        public Brush DisabledBackground
        {
            get { return (Brush)GetValue(DisabledBackgroundProperty); }
            set { SetValue(DisabledBackgroundProperty, value); }
        }

        public Brush SelectedForeground
        {
            get { return (Brush)GetValue(SelectedForegroundProperty); }
            set { SetValue(SelectedForegroundProperty, value); }
        }

        public Brush UnselectedForeground
        {
            get { return (Brush)GetValue(UnselectedForegroundProperty); }
            set { SetValue(UnselectedForegroundProperty, value); }
        }

        public Brush DisabledForeground
        {
            get { return (Brush)GetValue(DisabledForegroundProperty); }
            set { SetValue(DisabledForegroundProperty, value); }
        }

        private static void OnItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            UniversalSidebarItem item = sender as UniversalSidebarItem;
            if (item != null && item.Changed != null)
            {
                item.Changed(item, EventArgs.Empty);
            }
        }
    }
}
