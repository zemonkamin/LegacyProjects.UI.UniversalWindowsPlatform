using System;
using LegacyProjects.UI.UniversalWindowsPlatform.Enums;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Models
{
    public sealed class UniversalNavigationBarItem : DependencyObject
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(UniversalNavigationBarItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(
            "Key",
            typeof(string),
            typeof(UniversalNavigationBarItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(
            "Glyph",
            typeof(string),
            typeof(UniversalNavigationBarItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty ImagePathProperty = DependencyProperty.Register(
            "ImagePath",
            typeof(string),
            typeof(UniversalNavigationBarItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty IconTypeProperty = DependencyProperty.Register(
            "IconType",
            typeof(UniversalNavigationBarIconType),
            typeof(UniversalNavigationBarItem),
            new PropertyMetadata(UniversalNavigationBarIconType.Glyph, OnItemPropertyChanged));

        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register(
            "IconFontFamily",
            typeof(string),
            typeof(UniversalNavigationBarItem),
            new PropertyMetadata("Segoe MDL2 Assets", OnItemPropertyChanged));

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
            "IsEnabled",
            typeof(bool),
            typeof(UniversalNavigationBarItem),
            new PropertyMetadata(true, OnItemPropertyChanged));

        public static readonly DependencyProperty ClickActionProperty = DependencyProperty.Register(
            "ClickAction",
            typeof(string),
            typeof(UniversalNavigationBarItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
            "Background",
            typeof(Brush),
            typeof(UniversalNavigationBarItem),
            new PropertyMetadata(null, OnItemPropertyChanged));

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
            "Foreground",
            typeof(Brush),
            typeof(UniversalNavigationBarItem),
            new PropertyMetadata(null, OnItemPropertyChanged));

        public static readonly DependencyProperty DisabledForegroundProperty = DependencyProperty.Register(
            "DisabledForeground",
            typeof(Brush),
            typeof(UniversalNavigationBarItem),
            new PropertyMetadata(null, OnItemPropertyChanged));

        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(
            "Width",
            typeof(double),
            typeof(UniversalNavigationBarItem),
            new PropertyMetadata(double.NaN, OnItemPropertyChanged));

        public static readonly DependencyProperty MinWidthProperty = DependencyProperty.Register(
            "MinWidth",
            typeof(double),
            typeof(UniversalNavigationBarItem),
            new PropertyMetadata(0.0, OnItemPropertyChanged));

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

        public UniversalNavigationBarIconType IconType
        {
            get { return (UniversalNavigationBarIconType)GetValue(IconTypeProperty); }
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

        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        public Brush DisabledForeground
        {
            get { return (Brush)GetValue(DisabledForegroundProperty); }
            set { SetValue(DisabledForegroundProperty, value); }
        }

        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        public double MinWidth
        {
            get { return (double)GetValue(MinWidthProperty); }
            set { SetValue(MinWidthProperty, value); }
        }

        private static void OnItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            UniversalNavigationBarItem item = sender as UniversalNavigationBarItem;
            if (item != null && item.Changed != null)
            {
                item.Changed(item, EventArgs.Empty);
            }
        }
    }
}
