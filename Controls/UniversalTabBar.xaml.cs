using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using LegacyProjects.UI.UniversalWindowsPlatform.Enums;
using LegacyProjects.UI.UniversalWindowsPlatform.Events;
using LegacyProjects.UI.UniversalWindowsPlatform.Models;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Controls
{
    [ContentProperty(Name = "Items")]
    public sealed partial class UniversalTabBar : UserControl
    {
        public static readonly DependencyProperty VerticalPlacementProperty = DependencyProperty.Register(
            "VerticalPlacement",
            typeof(UniversalTabBarVerticalPlacement),
            typeof(UniversalTabBar),
            new PropertyMetadata(UniversalTabBarVerticalPlacement.Bottom, OnLayoutPropertyChanged));

        public static readonly DependencyProperty HorizontalPlacementProperty = DependencyProperty.Register(
            "HorizontalPlacement",
            typeof(UniversalTabBarHorizontalPlacement),
            typeof(UniversalTabBar),
            new PropertyMetadata(UniversalTabBarHorizontalPlacement.Stretch, OnLayoutPropertyChanged));

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation",
            typeof(Windows.UI.Xaml.Controls.Orientation),
            typeof(UniversalTabBar),
            new PropertyMetadata(Windows.UI.Xaml.Controls.Orientation.Horizontal, OnVisualPropertyChanged));

        public static readonly DependencyProperty BarHeightProperty = DependencyProperty.Register(
            "BarHeight",
            typeof(double),
            typeof(UniversalTabBar),
            new PropertyMetadata(56.0, OnLayoutPropertyChanged));

        public static readonly DependencyProperty BarWidthProperty = DependencyProperty.Register(
            "BarWidth",
            typeof(double),
            typeof(UniversalTabBar),
            new PropertyMetadata(double.NaN, OnLayoutPropertyChanged));

        public static readonly DependencyProperty CustomVerticalOffsetProperty = DependencyProperty.Register(
            "CustomVerticalOffset",
            typeof(double),
            typeof(UniversalTabBar),
            new PropertyMetadata(0.0, OnLayoutPropertyChanged));

        public static readonly DependencyProperty CustomHorizontalOffsetProperty = DependencyProperty.Register(
            "CustomHorizontalOffset",
            typeof(double),
            typeof(UniversalTabBar),
            new PropertyMetadata(0.0, OnLayoutPropertyChanged));

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex",
            typeof(int),
            typeof(UniversalTabBar),
            new PropertyMetadata(0, OnSelectedIndexChanged));

        public static readonly DependencyProperty SelectedTabForegroundProperty = DependencyProperty.Register(
            "SelectedTabForeground",
            typeof(Brush),
            typeof(UniversalTabBar),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty UnselectedTabForegroundProperty = DependencyProperty.Register(
            "UnselectedTabForeground",
            typeof(Brush),
            typeof(UniversalTabBar),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty SeparatorEnabledProperty = DependencyProperty.Register(
            "SeparatorEnabled",
            typeof(bool),
            typeof(UniversalTabBar),
            new PropertyMetadata(true, OnVisualPropertyChanged));

        public static readonly DependencyProperty SeparatorThicknessProperty = DependencyProperty.Register(
            "SeparatorThickness",
            typeof(double),
            typeof(UniversalTabBar),
            new PropertyMetadata(1.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty SeparatorBrushProperty = DependencyProperty.Register(
            "SeparatorBrush",
            typeof(Brush),
            typeof(UniversalTabBar),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty ShowIconsProperty = DependencyProperty.Register(
            "ShowIcons",
            typeof(bool),
            typeof(UniversalTabBar),
            new PropertyMetadata(true, OnVisualPropertyChanged));

        public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register(
            "ShowText",
            typeof(bool),
            typeof(UniversalTabBar),
            new PropertyMetadata(true, OnVisualPropertyChanged));

        public static readonly DependencyProperty TextPositionProperty = DependencyProperty.Register(
            "TextPosition",
            typeof(UniversalTabBarTextPosition),
            typeof(UniversalTabBar),
            new PropertyMetadata(UniversalTabBarTextPosition.BelowIcon, OnVisualPropertyChanged));

        public static readonly DependencyProperty ButtonPaddingProperty = DependencyProperty.Register(
            "ButtonPadding",
            typeof(Thickness),
            typeof(UniversalTabBar),
            new PropertyMetadata(new Thickness(6, 4, 6, 4), OnVisualPropertyChanged));

        public static readonly DependencyProperty ItemSpacingProperty = DependencyProperty.Register(
            "ItemSpacing",
            typeof(double),
            typeof(UniversalTabBar),
            new PropertyMetadata(0.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty ItemMinWidthProperty = DependencyProperty.Register(
            "ItemMinWidth",
            typeof(double),
            typeof(UniversalTabBar),
            new PropertyMetadata(64.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
            "IconSize",
            typeof(double),
            typeof(UniversalTabBar),
            new PropertyMetadata(20.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty LabelFontSizeProperty = DependencyProperty.Register(
            "LabelFontSize",
            typeof(double),
            typeof(UniversalTabBar),
            new PropertyMetadata(11.0, OnVisualPropertyChanged));

        public event EventHandler<UniversalTabBarSelectionChangedEventArgs> SelectionChanged;

        public UniversalTabBar()
        {
            Items = new ObservableCollection<UniversalTabBarItem>();
            Items.CollectionChanged += OnItemsCollectionChanged;

            InitializeComponent();
            ApplyDefaultBrushes();
            UpdateControl();
        }

        public ObservableCollection<UniversalTabBarItem> Items { get; private set; }

        public UniversalTabBarVerticalPlacement VerticalPlacement
        {
            get { return (UniversalTabBarVerticalPlacement)GetValue(VerticalPlacementProperty); }
            set { SetValue(VerticalPlacementProperty, value); }
        }

        public UniversalTabBarHorizontalPlacement HorizontalPlacement
        {
            get { return (UniversalTabBarHorizontalPlacement)GetValue(HorizontalPlacementProperty); }
            set { SetValue(HorizontalPlacementProperty, value); }
        }

        public Windows.UI.Xaml.Controls.Orientation Orientation
        {
            get { return (Windows.UI.Xaml.Controls.Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public double BarHeight
        {
            get { return (double)GetValue(BarHeightProperty); }
            set { SetValue(BarHeightProperty, value); }
        }

        public double BarWidth
        {
            get { return (double)GetValue(BarWidthProperty); }
            set { SetValue(BarWidthProperty, value); }
        }

        public double CustomVerticalOffset
        {
            get { return (double)GetValue(CustomVerticalOffsetProperty); }
            set { SetValue(CustomVerticalOffsetProperty, value); }
        }

        public double CustomHorizontalOffset
        {
            get { return (double)GetValue(CustomHorizontalOffsetProperty); }
            set { SetValue(CustomHorizontalOffsetProperty, value); }
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public UniversalTabBarItem SelectedItem
        {
            get
            {
                if (SelectedIndex < 0 || SelectedIndex >= Items.Count)
                {
                    return null;
                }

                return Items[SelectedIndex];
            }
        }

        public Brush SelectedTabForeground
        {
            get { return (Brush)GetValue(SelectedTabForegroundProperty); }
            set { SetValue(SelectedTabForegroundProperty, value); }
        }

        public Brush UnselectedTabForeground
        {
            get { return (Brush)GetValue(UnselectedTabForegroundProperty); }
            set { SetValue(UnselectedTabForegroundProperty, value); }
        }

        public bool SeparatorEnabled
        {
            get { return (bool)GetValue(SeparatorEnabledProperty); }
            set { SetValue(SeparatorEnabledProperty, value); }
        }

        public double SeparatorThickness
        {
            get { return (double)GetValue(SeparatorThicknessProperty); }
            set { SetValue(SeparatorThicknessProperty, value); }
        }

        public Brush SeparatorBrush
        {
            get { return (Brush)GetValue(SeparatorBrushProperty); }
            set { SetValue(SeparatorBrushProperty, value); }
        }

        public bool ShowIcons
        {
            get { return (bool)GetValue(ShowIconsProperty); }
            set { SetValue(ShowIconsProperty, value); }
        }

        public bool ShowText
        {
            get { return (bool)GetValue(ShowTextProperty); }
            set { SetValue(ShowTextProperty, value); }
        }

        public UniversalTabBarTextPosition TextPosition
        {
            get { return (UniversalTabBarTextPosition)GetValue(TextPositionProperty); }
            set { SetValue(TextPositionProperty, value); }
        }

        public Thickness ButtonPadding
        {
            get { return (Thickness)GetValue(ButtonPaddingProperty); }
            set { SetValue(ButtonPaddingProperty, value); }
        }

        public double ItemSpacing
        {
            get { return (double)GetValue(ItemSpacingProperty); }
            set { SetValue(ItemSpacingProperty, value); }
        }

        public double ItemMinWidth
        {
            get { return (double)GetValue(ItemMinWidthProperty); }
            set { SetValue(ItemMinWidthProperty, value); }
        }

        public double IconSize
        {
            get { return (double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        public double LabelFontSize
        {
            get { return (double)GetValue(LabelFontSizeProperty); }
            set { SetValue(LabelFontSizeProperty, value); }
        }

        private static void OnLayoutPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            UniversalTabBar tabBar = sender as UniversalTabBar;
            if (tabBar != null)
            {
                tabBar.UpdateControl();
            }
        }

        private static void OnVisualPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            UniversalTabBar tabBar = sender as UniversalTabBar;
            if (tabBar != null)
            {
                tabBar.UpdateControl();
            }
        }

        private static void OnSelectedIndexChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            UniversalTabBar tabBar = sender as UniversalTabBar;
            if (tabBar == null)
            {
                return;
            }

            int oldIndex = (int)args.OldValue;
            int newIndex = (int)args.NewValue;
            int normalizedIndex = tabBar.NormalizeSelectedIndex(newIndex);

            if (newIndex != normalizedIndex)
            {
                tabBar.SelectedIndex = normalizedIndex;
                return;
            }

            UniversalTabBarItem oldItem = tabBar.GetItemByIndex(oldIndex);
            UniversalTabBarItem newItem = tabBar.GetItemByIndex(newIndex);

            tabBar.UpdateItems();
            tabBar.RaiseSelectionChanged(oldIndex, newIndex, oldItem, newItem);
        }

        private void ApplyDefaultBrushes()
        {
            if (Background == null)
            {
                Background = GetThemeBrush("ApplicationPageBackgroundThemeBrush", new SolidColorBrush(Colors.Transparent));
            }

            if (SelectedTabForeground == null)
            {
                SelectedTabForeground = GetThemeBrush("SystemControlHighlightAccentBrush", new SolidColorBrush(Color.FromArgb(255, 0, 120, 215)));
            }

            if (UnselectedTabForeground == null)
            {
                UnselectedTabForeground = new SolidColorBrush(Colors.Gray);
            }

            if (SeparatorBrush == null)
            {
                SeparatorBrush = new SolidColorBrush(Color.FromArgb(51, 0, 0, 0));
            }
        }

        private Brush GetThemeBrush(string resourceKey, Brush fallbackBrush)
        {
            object resource = null;

            if (Application.Current != null && Application.Current.Resources != null && Application.Current.Resources.ContainsKey(resourceKey))
            {
                resource = Application.Current.Resources[resourceKey];
            }

            Brush brush = resource as Brush;
            return brush ?? fallbackBrush;
        }

        private void UpdateControl()
        {
            if (RootBorder == null || ItemsGrid == null)
            {
                return;
            }

            UpdateLayoutProperties();
            UpdateSeparator();
            UpdateItems();
        }

        private void UpdateLayoutProperties()
        {
            Height = BarHeight > 0 ? BarHeight : double.NaN;

            if (HorizontalPlacement == UniversalTabBarHorizontalPlacement.Stretch)
            {
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
                Width = double.NaN;
            }
            else if (HorizontalPlacement == UniversalTabBarHorizontalPlacement.Right)
            {
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;
                Width = BarWidth > 0 ? BarWidth : double.NaN;
            }
            else
            {
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
                Width = BarWidth > 0 ? BarWidth : double.NaN;
            }

            if (VerticalPlacement == UniversalTabBarVerticalPlacement.Bottom)
            {
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom;
            }
            else
            {
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
            }

            if (VerticalPlacement == UniversalTabBarVerticalPlacement.Custom || HorizontalPlacement == UniversalTabBarHorizontalPlacement.Custom)
            {
                double left = HorizontalPlacement == UniversalTabBarHorizontalPlacement.Custom ? CustomHorizontalOffset : 0;
                double top = VerticalPlacement == UniversalTabBarVerticalPlacement.Custom ? CustomVerticalOffset : 0;
                Margin = new Thickness(left, top, 0, 0);
            }
            else
            {
                Margin = new Thickness(0);
            }
        }

        private void UpdateSeparator()
        {
            if (!SeparatorEnabled || SeparatorThickness <= 0)
            {
                RootBorder.BorderThickness = new Thickness(0);
                return;
            }

            double left = 0;
            double top = 0;
            double right = 0;
            double bottom = 0;
            double thickness = SeparatorThickness;

            if (VerticalPlacement == UniversalTabBarVerticalPlacement.Top)
            {
                bottom = thickness;
            }
            else if (VerticalPlacement == UniversalTabBarVerticalPlacement.Bottom)
            {
                top = thickness;
            }
            else
            {
                top = thickness;
                bottom = thickness;
            }

            if (HorizontalPlacement == UniversalTabBarHorizontalPlacement.Custom)
            {
                left = thickness;
                right = thickness;
            }

            RootBorder.BorderBrush = SeparatorBrush;
            RootBorder.BorderThickness = new Thickness(left, top, right, bottom);
        }

        private void UpdateItems()
        {
            if (ItemsGrid == null)
            {
                return;
            }

            ItemsGrid.Children.Clear();
            ItemsGrid.RowDefinitions.Clear();
            ItemsGrid.ColumnDefinitions.Clear();

            if (Items == null || Items.Count == 0)
            {
                return;
            }

            int normalizedIndex = NormalizeSelectedIndex(SelectedIndex);
            if (SelectedIndex != normalizedIndex)
            {
                SelectedIndex = normalizedIndex;
                return;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                UniversalTabBarItem item = Items[i];
                Button button = CreateButton(item, i);

                if (Orientation == Windows.UI.Xaml.Controls.Orientation.Horizontal)
                {
                    GridLength width = HorizontalPlacement == UniversalTabBarHorizontalPlacement.Stretch
                        ? new GridLength(1, GridUnitType.Star)
                        : GridLength.Auto;

                    ItemsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = width });
                    Grid.SetColumn(button, i);
                }
                else
                {
                    ItemsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    Grid.SetRow(button, i);
                }

                ItemsGrid.Children.Add(button);
            }
        }

        private Button CreateButton(UniversalTabBarItem item, int index)
        {
            bool selected = index == SelectedIndex;
            Brush foreground = selected ? SelectedTabForeground : UnselectedTabForeground;

            Button button = new Button();
            button.Tag = index;
            button.IsEnabled = item == null || item.IsEnabled;
            button.Padding = ButtonPadding;
            button.MinWidth = ItemMinWidth;
            button.MinHeight = BarHeight > 0 ? BarHeight : 0;
            button.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            button.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
            button.HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            button.VerticalContentAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            button.Foreground = foreground;
            button.Background = new SolidColorBrush(Colors.Transparent);
            button.BorderThickness = new Thickness(0);
            button.Margin = GetItemMargin();
            button.Click += OnButtonClick;

            StackPanel content = new StackPanel();
            content.Orientation = Windows.UI.Xaml.Controls.Orientation.Vertical;
            content.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            content.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;

            FrameworkElement icon = CreateIcon(item, foreground);
            TextBlock text = CreateText(item, foreground);

            if (TextPosition == UniversalTabBarTextPosition.AboveIcon)
            {
                AddTextAndIcon(content, text, icon);
            }
            else
            {
                AddIconAndText(content, icon, text);
            }

            button.Content = content;
            return button;
        }

        private Thickness GetItemMargin()
        {
            if (ItemSpacing <= 0)
            {
                return new Thickness(0);
            }

            double halfSpacing = ItemSpacing / 2;
            if (Orientation == Windows.UI.Xaml.Controls.Orientation.Horizontal)
            {
                return new Thickness(halfSpacing, 0, halfSpacing, 0);
            }

            return new Thickness(0, halfSpacing, 0, halfSpacing);
        }

        private void AddIconAndText(StackPanel content, FrameworkElement icon, TextBlock text)
        {
            if (ShowIcons && icon != null)
            {
                content.Children.Add(icon);
            }

            if (ShowText && text != null)
            {
                content.Children.Add(text);
            }
        }

        private void AddTextAndIcon(StackPanel content, TextBlock text, FrameworkElement icon)
        {
            if (ShowText && text != null)
            {
                content.Children.Add(text);
            }

            if (ShowIcons && icon != null)
            {
                content.Children.Add(icon);
            }
        }

        private FrameworkElement CreateIcon(UniversalTabBarItem item, Brush foreground)
        {
            if (item == null || !ShowIcons)
            {
                return null;
            }

            if (item.IconType == UniversalTabBarIconType.Image)
            {
                ImageSource source = CreateImageSource(item.ImagePath);
                if (source == null)
                {
                    return null;
                }

                Image image = new Image();
                image.Source = source;
                image.Width = IconSize;
                image.Height = IconSize;
                image.Stretch = Stretch.Uniform;
                image.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                return image;
            }

            if (string.IsNullOrEmpty(item.Glyph))
            {
                return null;
            }

            FontIcon fontIcon = new FontIcon();
            fontIcon.Glyph = item.Glyph;
            fontIcon.FontFamily = new FontFamily(string.IsNullOrEmpty(item.IconFontFamily) ? "Segoe MDL2 Assets" : item.IconFontFamily);
            fontIcon.FontSize = IconSize;
            fontIcon.Foreground = foreground;
            fontIcon.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            return fontIcon;
        }

        private ImageSource CreateImageSource(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return null;
            }

            Uri uri;
            string trimmedPath = imagePath.Trim();

            if (Uri.TryCreate(trimmedPath, UriKind.Absolute, out uri))
            {
                return new BitmapImage(uri);
            }

            string normalizedPath = trimmedPath.TrimStart('/');
            return new BitmapImage(new Uri("ms-appx:///" + normalizedPath));
        }

        private TextBlock CreateText(UniversalTabBarItem item, Brush foreground)
        {
            if (item == null || !ShowText || string.IsNullOrEmpty(item.Text))
            {
                return null;
            }

            TextBlock textBlock = new TextBlock();
            textBlock.Text = item.Text;
            textBlock.FontSize = LabelFontSize;
            textBlock.Foreground = foreground;
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.TextWrapping = TextWrapping.NoWrap;
            textBlock.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            return textBlock;
        }

        private void OnButtonClick(object sender, RoutedEventArgs args)
        {
            Button button = sender as Button;
            if (button == null || button.Tag == null)
            {
                return;
            }

            int index = (int)button.Tag;
            if (index != SelectedIndex)
            {
                SelectedIndex = index;
            }
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.OldItems != null)
            {
                foreach (object oldObject in args.OldItems)
                {
                    UniversalTabBarItem oldItem = oldObject as UniversalTabBarItem;
                    if (oldItem != null)
                    {
                        oldItem.Changed -= OnItemChanged;
                    }
                }
            }

            if (args.NewItems != null)
            {
                foreach (object newObject in args.NewItems)
                {
                    UniversalTabBarItem newItem = newObject as UniversalTabBarItem;
                    if (newItem != null)
                    {
                        newItem.Changed += OnItemChanged;
                    }
                }
            }

            UpdateItems();
        }

        private void OnItemChanged(object sender, EventArgs args)
        {
            UpdateItems();
        }

        private int NormalizeSelectedIndex(int selectedIndex)
        {
            if (Items == null || Items.Count == 0)
            {
                return -1;
            }

            if (selectedIndex < 0)
            {
                return -1;
            }

            if (selectedIndex >= Items.Count)
            {
                return Items.Count - 1;
            }

            return selectedIndex;
        }

        private UniversalTabBarItem GetItemByIndex(int index)
        {
            if (Items == null || index < 0 || index >= Items.Count)
            {
                return null;
            }

            return Items[index];
        }

        private void RaiseSelectionChanged(int oldIndex, int newIndex, UniversalTabBarItem oldItem, UniversalTabBarItem newItem)
        {
            if (oldIndex == newIndex)
            {
                return;
            }

            EventHandler<UniversalTabBarSelectionChangedEventArgs> handler = SelectionChanged;
            if (handler != null)
            {
                handler(this, new UniversalTabBarSelectionChangedEventArgs(oldIndex, newIndex, oldItem, newItem));
            }
        }
    }
}
