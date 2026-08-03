using System;
using LegacyProjects.UI.UniversalWindowsPlatform.Enums;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Models
{
    public sealed class TabBarItem : DependencyObject
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(TabBarItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(
            "Glyph",
            typeof(string),
            typeof(TabBarItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty ImagePathProperty = DependencyProperty.Register(
            "ImagePath",
            typeof(string),
            typeof(TabBarItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty IconTypeProperty = DependencyProperty.Register(
            "IconType",
            typeof(TabBarIconType),
            typeof(TabBarItem),
            new PropertyMetadata(TabBarIconType.Glyph, OnItemPropertyChanged));

        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register(
            "IconFontFamily",
            typeof(string),
            typeof(TabBarItem),
            new PropertyMetadata("Segoe MDL2 Assets", OnItemPropertyChanged));

        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register(
            "IsEnabled",
            typeof(bool),
            typeof(TabBarItem),
            new PropertyMetadata(true, OnItemPropertyChanged));

        public static readonly DependencyProperty PressEffectModeProperty = DependencyProperty.Register(
            "PressEffectMode",
            typeof(TabBarPressEffectMode),
            typeof(TabBarItem),
            new PropertyMetadata(TabBarPressEffectMode.Default, OnItemPropertyChanged));

        public static readonly DependencyProperty ClickActionProperty = DependencyProperty.Register(
            "ClickAction",
            typeof(string),
            typeof(TabBarItem),
            new PropertyMetadata(string.Empty, OnItemPropertyChanged));

        public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register(
            "IconForeground",
            typeof(Brush),
            typeof(TabBarItem),
            new PropertyMetadata(null, OnItemPropertyChanged));

        public static readonly DependencyProperty TextForegroundProperty = DependencyProperty.Register(
            "TextForeground",
            typeof(Brush),
            typeof(TabBarItem),
            new PropertyMetadata(null, OnItemPropertyChanged));

        public static readonly DependencyProperty SelectedIconForegroundProperty = DependencyProperty.Register(
            "SelectedIconForeground",
            typeof(Brush),
            typeof(TabBarItem),
            new PropertyMetadata(null, OnItemPropertyChanged));

        public static readonly DependencyProperty UnselectedIconForegroundProperty = DependencyProperty.Register(
            "UnselectedIconForeground",
            typeof(Brush),
            typeof(TabBarItem),
            new PropertyMetadata(null, OnItemPropertyChanged));

        public static readonly DependencyProperty SelectedTextForegroundProperty = DependencyProperty.Register(
            "SelectedTextForeground",
            typeof(Brush),
            typeof(TabBarItem),
            new PropertyMetadata(null, OnItemPropertyChanged));

        public static readonly DependencyProperty UnselectedTextForegroundProperty = DependencyProperty.Register(
            "UnselectedTextForeground",
            typeof(Brush),
            typeof(TabBarItem),
            new PropertyMetadata(null, OnItemPropertyChanged));

        internal event EventHandler Changed;

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
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

        public TabBarIconType IconType
        {
            get { return (TabBarIconType)GetValue(IconTypeProperty); }
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

        public TabBarPressEffectMode PressEffectMode
        {
            get { return (TabBarPressEffectMode)GetValue(PressEffectModeProperty); }
            set { SetValue(PressEffectModeProperty, value); }
        }

        public string ClickAction
        {
            get { return (string)GetValue(ClickActionProperty); }
            set { SetValue(ClickActionProperty, value); }
        }

        public Brush IconForeground
        {
            get { return (Brush)GetValue(IconForegroundProperty); }
            set { SetValue(IconForegroundProperty, value); }
        }

        public Brush TextForeground
        {
            get { return (Brush)GetValue(TextForegroundProperty); }
            set { SetValue(TextForegroundProperty, value); }
        }

        public Brush SelectedIconForeground
        {
            get { return (Brush)GetValue(SelectedIconForegroundProperty); }
            set { SetValue(SelectedIconForegroundProperty, value); }
        }

        public Brush UnselectedIconForeground
        {
            get { return (Brush)GetValue(UnselectedIconForegroundProperty); }
            set { SetValue(UnselectedIconForegroundProperty, value); }
        }

        public Brush SelectedTextForeground
        {
            get { return (Brush)GetValue(SelectedTextForegroundProperty); }
            set { SetValue(SelectedTextForegroundProperty, value); }
        }

        public Brush UnselectedTextForeground
        {
            get { return (Brush)GetValue(UnselectedTextForegroundProperty); }
            set { SetValue(UnselectedTextForegroundProperty, value); }
        }

        private static void OnItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            TabBarItem item = sender as TabBarItem;
            if (item != null && item.Changed != null)
            {
                item.Changed(item, EventArgs.Empty);
            }
        }
    }
}
