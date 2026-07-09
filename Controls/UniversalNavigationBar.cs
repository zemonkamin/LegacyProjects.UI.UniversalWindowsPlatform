using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using LegacyProjects.UI.UniversalWindowsPlatform.Enums;
using LegacyProjects.UI.UniversalWindowsPlatform.Events;
using LegacyProjects.UI.UniversalWindowsPlatform.Helpers;
using LegacyProjects.UI.UniversalWindowsPlatform.Models;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Controls
{
    [ContentProperty(Name = "CenterItems")]
    public sealed class UniversalNavigationBar : UserControl
    {
        public static readonly DependencyProperty BarHeightProperty = DependencyProperty.Register(
            "BarHeight",
            typeof(double),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(56.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(
            "ItemHeight",
            typeof(double),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(56.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.Register(
            "ItemPadding",
            typeof(Thickness),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(new Thickness(14, 0, 14, 0), OnVisualPropertyChanged));

        public static readonly DependencyProperty ItemSpacingProperty = DependencyProperty.Register(
            "ItemSpacing",
            typeof(double),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(0.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
            "IconSize",
            typeof(double),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(18.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty IconTextSpacingProperty = DependencyProperty.Register(
            "IconTextSpacing",
            typeof(double),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(8.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty LabelFontSizeProperty = DependencyProperty.Register(
            "LabelFontSize",
            typeof(double),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(14.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty LabelFontWeightProperty = DependencyProperty.Register(
            "LabelFontWeight",
            typeof(Windows.UI.Text.FontWeight),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(Windows.UI.Text.FontWeights.Normal, OnVisualPropertyChanged));

        public static readonly DependencyProperty ShowIconsProperty = DependencyProperty.Register(
            "ShowIcons",
            typeof(bool),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(true, OnVisualPropertyChanged));

        public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register(
            "ShowText",
            typeof(bool),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(true, OnVisualPropertyChanged));

        public static readonly DependencyProperty BackgroundOpacityProperty = DependencyProperty.Register(
            "BackgroundOpacity",
            typeof(double),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(0.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty ItemForegroundProperty = DependencyProperty.Register(
            "ItemForeground",
            typeof(Brush),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty DisabledForegroundProperty = DependencyProperty.Register(
            "DisabledForeground",
            typeof(Brush),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty DisabledItemOpacityProperty = DependencyProperty.Register(
            "DisabledItemOpacity",
            typeof(double),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(0.45, OnVisualPropertyChanged));

        public static readonly DependencyProperty SeparatorEnabledProperty = DependencyProperty.Register(
            "SeparatorEnabled",
            typeof(bool),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(true, OnVisualPropertyChanged));

        public static readonly DependencyProperty SeparatorBrushProperty = DependencyProperty.Register(
            "SeparatorBrush",
            typeof(Brush),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty SeparatorThicknessProperty = DependencyProperty.Register(
            "SeparatorThickness",
            typeof(double),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(1.0, OnVisualPropertyChanged));


        public static readonly DependencyProperty PressAnimationEnabledProperty = DependencyProperty.Register(
            "PressAnimationEnabled",
            typeof(bool),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(true, OnVisualPropertyChanged));

        public static readonly DependencyProperty PressAnimationScaleProperty = DependencyProperty.Register(
            "PressAnimationScale",
            typeof(double),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(0.96));

        public static readonly DependencyProperty PressAnimationDurationProperty = DependencyProperty.Register(
            "PressAnimationDuration",
            typeof(double),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(90.0));

        public static readonly DependencyProperty ActionTargetProperty = DependencyProperty.Register(
            "ActionTarget",
            typeof(object),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ActionTargetTypeProperty = DependencyProperty.Register(
            "ActionTargetType",
            typeof(string),
            typeof(UniversalNavigationBar),
            new PropertyMetadata(string.Empty));

        private readonly Grid _root;
        private readonly Border _backgroundBorder;
        private readonly Grid _layoutGrid;
        private readonly StackPanel _leftPanel;
        private readonly StackPanel _centerPanel;
        private readonly StackPanel _rightPanel;
        private readonly ScrollViewer _centerScrollViewer;
        private readonly Border _separatorBorder;

        public event EventHandler<UniversalNavigationBarItemClickEventArgs> ItemClick;

        public UniversalNavigationBar()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Top;

            LeftItems = new ObservableCollection<UniversalNavigationBarItem>();
            CenterItems = new ObservableCollection<UniversalNavigationBarItem>();
            RightItems = new ObservableCollection<UniversalNavigationBarItem>();

            LeftItems.CollectionChanged += OnItemsCollectionChanged;
            CenterItems.CollectionChanged += OnItemsCollectionChanged;
            RightItems.CollectionChanged += OnItemsCollectionChanged;

            _root = new Grid();
            _root.VerticalAlignment = VerticalAlignment.Top;
            _root.HorizontalAlignment = HorizontalAlignment.Stretch;

            _backgroundBorder = new Border();
            _layoutGrid = new Grid();
            _layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            _layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            _layoutGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            _leftPanel = new StackPanel();
            _leftPanel.Orientation = Orientation.Horizontal;

            _centerPanel = new StackPanel();
            _centerPanel.Orientation = Orientation.Horizontal;

            _rightPanel = new StackPanel();
            _rightPanel.Orientation = Orientation.Horizontal;

            _centerScrollViewer = new ScrollViewer();
            _centerScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            _centerScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            _centerScrollViewer.HorizontalScrollMode = ScrollMode.Enabled;
            _centerScrollViewer.VerticalScrollMode = ScrollMode.Disabled;
            _centerScrollViewer.Content = _centerPanel;

            Grid.SetColumn(_leftPanel, 0);
            Grid.SetColumn(_centerScrollViewer, 1);
            Grid.SetColumn(_rightPanel, 2);
            _layoutGrid.Children.Add(_leftPanel);
            _layoutGrid.Children.Add(_centerScrollViewer);
            _layoutGrid.Children.Add(_rightPanel);

            _backgroundBorder.Child = _layoutGrid;
            _root.Children.Add(_backgroundBorder);

            _separatorBorder = new Border();
            _separatorBorder.VerticalAlignment = VerticalAlignment.Bottom;
            _root.Children.Add(_separatorBorder);

            Content = _root;
            Loaded += OnLoaded;
            RegisterPropertyChangedCallback(Control.BackgroundProperty, OnBackgroundPropertyChanged);
        }

        private void OnBackgroundPropertyChanged(DependencyObject sender, DependencyProperty property)
        {
            UpdateVisuals();
        }

        public ObservableCollection<UniversalNavigationBarItem> LeftItems { get; private set; }
        public ObservableCollection<UniversalNavigationBarItem> CenterItems { get; private set; }
        public ObservableCollection<UniversalNavigationBarItem> RightItems { get; private set; }

        public double BarHeight
        {
            get { return (double)GetValue(BarHeightProperty); }
            set { SetValue(BarHeightProperty, value); }
        }

        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        public Thickness ItemPadding
        {
            get { return (Thickness)GetValue(ItemPaddingProperty); }
            set { SetValue(ItemPaddingProperty, value); }
        }

        public double ItemSpacing
        {
            get { return (double)GetValue(ItemSpacingProperty); }
            set { SetValue(ItemSpacingProperty, value); }
        }

        public double IconSize
        {
            get { return (double)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        public double IconTextSpacing
        {
            get { return (double)GetValue(IconTextSpacingProperty); }
            set { SetValue(IconTextSpacingProperty, value); }
        }

        public double LabelFontSize
        {
            get { return (double)GetValue(LabelFontSizeProperty); }
            set { SetValue(LabelFontSizeProperty, value); }
        }

        public Windows.UI.Text.FontWeight LabelFontWeight
        {
            get { return (Windows.UI.Text.FontWeight)GetValue(LabelFontWeightProperty); }
            set { SetValue(LabelFontWeightProperty, value); }
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

        public double BackgroundOpacity
        {
            get { return (double)GetValue(BackgroundOpacityProperty); }
            set { SetValue(BackgroundOpacityProperty, value); }
        }

        public Brush ItemForeground
        {
            get { return (Brush)GetValue(ItemForegroundProperty); }
            set { SetValue(ItemForegroundProperty, value); }
        }

        public Brush DisabledForeground
        {
            get { return (Brush)GetValue(DisabledForegroundProperty); }
            set { SetValue(DisabledForegroundProperty, value); }
        }

        public double DisabledItemOpacity
        {
            get { return (double)GetValue(DisabledItemOpacityProperty); }
            set { SetValue(DisabledItemOpacityProperty, value); }
        }

        public bool SeparatorEnabled
        {
            get { return (bool)GetValue(SeparatorEnabledProperty); }
            set { SetValue(SeparatorEnabledProperty, value); }
        }

        public Brush SeparatorBrush
        {
            get { return (Brush)GetValue(SeparatorBrushProperty); }
            set { SetValue(SeparatorBrushProperty, value); }
        }

        public double SeparatorThickness
        {
            get { return (double)GetValue(SeparatorThicknessProperty); }
            set { SetValue(SeparatorThicknessProperty, value); }
        }

        public bool BottomSeparatorEnabled
        {
            get { return SeparatorEnabled; }
            set { SeparatorEnabled = value; }
        }

        public Brush BottomSeparatorBrush
        {
            get { return SeparatorBrush; }
            set { SeparatorBrush = value; }
        }

        public double BottomSeparatorThickness
        {
            get { return SeparatorThickness; }
            set { SeparatorThickness = value; }
        }


        public bool PressAnimationEnabled
        {
            get { return (bool)GetValue(PressAnimationEnabledProperty); }
            set { SetValue(PressAnimationEnabledProperty, value); }
        }

        public double PressAnimationScale
        {
            get { return (double)GetValue(PressAnimationScaleProperty); }
            set { SetValue(PressAnimationScaleProperty, value); }
        }

        public double PressAnimationDuration
        {
            get { return (double)GetValue(PressAnimationDurationProperty); }
            set { SetValue(PressAnimationDurationProperty, value); }
        }

        public object ActionTarget
        {
            get { return GetValue(ActionTargetProperty); }
            set { SetValue(ActionTargetProperty, value); }
        }

        public string ActionTargetType
        {
            get { return (string)GetValue(ActionTargetTypeProperty); }
            set { SetValue(ActionTargetTypeProperty, value); }
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            ApplyTopDocking();
            UpdateVisuals();
        }

        private static void OnVisualPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            UniversalNavigationBar bar = sender as UniversalNavigationBar;
            if (bar != null)
            {
                bar.UpdateVisuals();
            }
        }

        private void UpdateVisuals()
        {
            ApplyTopDocking();

            if (_backgroundBorder == null)
            {
                return;
            }

            double height = Math.Max(0, BarHeight);
            Height = height;
            _root.Height = height;
            _backgroundBorder.Height = height;
            Brush background = Background != null ? Background : ResolveThemeBrush("ApplicationPageBackgroundThemeBrush", new SolidColorBrush(Color.FromArgb(255, 17, 19, 16)));
            _backgroundBorder.Background = ApplyBrushOpacity(background, BackgroundOpacity);

            if (SeparatorEnabled && SeparatorThickness > 0)
            {
                _separatorBorder.Visibility = Visibility.Visible;
                _separatorBorder.Height = SeparatorThickness;
                _separatorBorder.Background = SeparatorBrush != null ? SeparatorBrush : ResolveThemeBrush("SystemControlForegroundBaseLowBrush", new SolidColorBrush(Color.FromArgb(255, 51, 54, 52)));
            }
            else
            {
                _separatorBorder.Visibility = Visibility.Collapsed;
            }

            RebuildItems();
        }

        private void ApplyTopDocking()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Top;
            Canvas.SetTop(this, 0);
            Canvas.SetLeft(this, 0);
            Margin = new Thickness(0);

            if (_root != null)
            {
                _root.HorizontalAlignment = HorizontalAlignment.Stretch;
                _root.VerticalAlignment = VerticalAlignment.Top;
            }
        }

        private void RebuildItems()
        {
            _leftPanel.Children.Clear();
            _centerPanel.Children.Clear();
            _rightPanel.Children.Clear();

            AddItemsToPanel(_leftPanel, LeftItems, UniversalNavigationBarItemSection.Left);
            AddItemsToPanel(_centerPanel, CenterItems, UniversalNavigationBarItemSection.Center);
            AddItemsToPanel(_rightPanel, RightItems, UniversalNavigationBarItemSection.Right);
        }

        private void AddItemsToPanel(StackPanel panel, ObservableCollection<UniversalNavigationBarItem> collection, UniversalNavigationBarItemSection section)
        {
            if (panel == null || collection == null)
            {
                return;
            }

            for (int i = 0; i < collection.Count; i++)
            {
                panel.Children.Add(CreateButton(collection[i], section, i));
            }
        }

        private Button CreateButton(UniversalNavigationBarItem item, UniversalNavigationBarItemSection section, int index)
        {
            bool isEnabled = item == null || item.IsEnabled;
            Brush foreground = GetItemForeground(item, isEnabled);

            Button button = new Button();
            button.Height = Math.Max(0, ItemHeight);
            button.MinWidth = item != null ? Math.Max(0, item.MinWidth) : 0;
            if (item != null && !double.IsNaN(item.Width))
            {
                button.Width = item.Width;
            }
            button.VerticalAlignment = VerticalAlignment.Center;
            button.HorizontalContentAlignment = HorizontalAlignment.Center;
            button.VerticalContentAlignment = VerticalAlignment.Center;
            button.Padding = ItemPadding;
            button.Margin = new Thickness(Math.Max(0, ItemSpacing) / 2.0, 0, Math.Max(0, ItemSpacing) / 2.0, 0);
            button.BorderThickness = new Thickness(0);
            button.Background = item != null && item.Background != null ? item.Background : new SolidColorBrush(Colors.Transparent);
            button.Foreground = foreground;
            button.IsHitTestVisible = isEnabled;
            button.Opacity = isEnabled ? 1.0 : Clamp01(DisabledItemOpacity);
            button.Tag = new NavigationBarButtonTag(index, section, item);
            button.Click += OnButtonClick;
            AttachPressAnimation(button);

            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;
            stack.VerticalAlignment = VerticalAlignment.Center;
            stack.HorizontalAlignment = HorizontalAlignment.Center;

            FrameworkElement icon = CreateIcon(item, foreground);
            string itemText = item != null ? item.Text : string.Empty;

            if (icon != null && ShowIcons)
            {
                icon.Margin = ShowText && !string.IsNullOrEmpty(itemText) ? new Thickness(0, 0, Math.Max(0, IconTextSpacing), 0) : new Thickness(0);
                stack.Children.Add(icon);
            }

            if (ShowText && !string.IsNullOrEmpty(itemText))
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = itemText;
                textBlock.FontSize = LabelFontSize;
                textBlock.FontWeight = LabelFontWeight;
                textBlock.Foreground = foreground;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
                stack.Children.Add(textBlock);
            }

            button.Content = stack;
            return button;
        }

        private FrameworkElement CreateIcon(UniversalNavigationBarItem item, Brush foreground)
        {
            if (item == null)
            {
                return null;
            }

            if (item.IconType == UniversalNavigationBarIconType.Image && !string.IsNullOrWhiteSpace(item.ImagePath))
            {
                Image image = new Image();
                image.Width = IconSize;
                image.Height = IconSize;
                image.Stretch = Stretch.Uniform;
                try
                {
                    image.Source = new BitmapImage(new Uri(item.ImagePath, UriKind.RelativeOrAbsolute));
                }
                catch
                {
                }

                return image;
            }

            if (string.IsNullOrEmpty(item.Glyph))
            {
                return null;
            }

            FontIcon icon = new FontIcon();
            icon.Glyph = item.Glyph;
            icon.FontFamily = new FontFamily(string.IsNullOrWhiteSpace(item.IconFontFamily) ? "Segoe MDL2 Assets" : item.IconFontFamily);
            icon.FontSize = IconSize;
            icon.Width = IconSize;
            icon.Height = IconSize;
            icon.Foreground = foreground;
            return icon;
        }

        private Brush GetItemForeground(UniversalNavigationBarItem item, bool isEnabled)
        {
            if (!isEnabled)
            {
                if (item != null && item.DisabledForeground != null)
                {
                    return item.DisabledForeground;
                }

                if (DisabledForeground != null)
                {
                    return DisabledForeground;
                }

                return new SolidColorBrush(Color.FromArgb(255, 128, 128, 128));
            }

            if (item != null && item.Foreground != null)
            {
                return item.Foreground;
            }

            if (ItemForeground != null)
            {
                return ItemForeground;
            }

            return ResolveThemeBrush("SystemControlForegroundBaseHighBrush", new SolidColorBrush(Colors.White));
        }

        private Brush ApplyBrushOpacity(Brush brush, double transparency)
        {
            double safeTransparency = Clamp01(transparency);
            SolidColorBrush solidBrush = brush as SolidColorBrush;
            if (solidBrush != null)
            {
                Color color = solidBrush.Color;
                byte alpha = (byte)Math.Round(color.A * (1.0 - safeTransparency));
                return new SolidColorBrush(Color.FromArgb(alpha, color.R, color.G, color.B));
            }

            return brush;
        }

        private Brush ResolveThemeBrush(string key, Brush fallback)
        {
            try
            {
                if (Application.Current != null && Application.Current.Resources != null && Application.Current.Resources.ContainsKey(key))
                {
                    Brush brush = Application.Current.Resources[key] as Brush;
                    if (brush != null)
                    {
                        return brush;
                    }
                }
            }
            catch
            {
            }

            return fallback;
        }


        private void AttachPressAnimation(Button button)
        {
            if (button == null || !PressAnimationEnabled)
            {
                return;
            }

            CompositeTransform transform = new CompositeTransform();
            button.RenderTransform = transform;
            button.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
            button.PointerPressed += OnPressAnimationPointerPressed;
            button.PointerReleased += OnPressAnimationPointerReleased;
            button.PointerCanceled += OnPressAnimationPointerReleased;
            button.PointerExited += OnPressAnimationPointerReleased;
            button.PointerCaptureLost += OnPressAnimationPointerReleased;
        }

        private void OnPressAnimationPointerPressed(object sender, PointerRoutedEventArgs args)
        {
            Button button = sender as Button;
            if (button == null || !button.IsHitTestVisible || !PressAnimationEnabled)
            {
                return;
            }

            AnimateButtonScale(button, Clamp01(PressAnimationScale));
        }

        private void OnPressAnimationPointerReleased(object sender, PointerRoutedEventArgs args)
        {
            Button button = sender as Button;
            if (button == null || !PressAnimationEnabled)
            {
                return;
            }

            AnimateButtonScale(button, 1.0);
        }

        private void AnimateButtonScale(Button button, double targetScale)
        {
            if (button == null)
            {
                return;
            }

            if (button.RenderTransform == null || !(button.RenderTransform is CompositeTransform))
            {
                button.RenderTransform = new CompositeTransform();
                button.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
            }

            CompositeTransform transform = button.RenderTransform as CompositeTransform;
            if (transform == null)
            {
                return;
            }

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(CreateScaleAnimation(transform, targetScale, "ScaleX"));
            storyboard.Children.Add(CreateScaleAnimation(transform, targetScale, "ScaleY"));
            storyboard.Begin();
        }

        private DoubleAnimation CreateScaleAnimation(DependencyObject target, double to, string propertyPath)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.To = to;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(Math.Max(0, PressAnimationDuration)));
            animation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, propertyPath);
            return animation;
        }

        private void OnButtonClick(object sender, RoutedEventArgs args)
        {
            FrameworkElement element = sender as FrameworkElement;
            NavigationBarButtonTag tag = element != null ? element.Tag as NavigationBarButtonTag : null;
            if (tag == null || tag.Item == null || !tag.Item.IsEnabled)
            {
                return;
            }

            UniversalNavigationBarItemClickEventArgs clickArgs = new UniversalNavigationBarItemClickEventArgs(tag.Index, tag.Section, tag.Item, tag.Item.ClickAction);
            RaiseItemClick(clickArgs);
            if (clickArgs.Handled)
            {
                return;
            }

            UniversalActionInvoker.Invoke(this, ActionTarget, ActionTargetType, clickArgs.ClickAction, clickArgs, tag.Item, tag.Index);
        }

        private void RaiseItemClick(UniversalNavigationBarItemClickEventArgs args)
        {
            EventHandler<UniversalNavigationBarItemClickEventArgs> handler = ItemClick;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.OldItems != null)
            {
                foreach (object oldObject in args.OldItems)
                {
                    UniversalNavigationBarItem oldItem = oldObject as UniversalNavigationBarItem;
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
                    UniversalNavigationBarItem newItem = newObject as UniversalNavigationBarItem;
                    if (newItem != null)
                    {
                        newItem.Changed += OnItemChanged;
                    }
                }
            }

            UpdateVisuals();
        }

        private void OnItemChanged(object sender, EventArgs args)
        {
            UpdateVisuals();
        }

        private double Clamp01(double value)
        {
            if (value < 0)
            {
                return 0;
            }

            if (value > 1)
            {
                return 1;
            }

            return value;
        }

        private sealed class NavigationBarButtonTag
        {
            public NavigationBarButtonTag(int index, UniversalNavigationBarItemSection section, UniversalNavigationBarItem item)
            {
                Index = index;
                Section = section;
                Item = item;
            }

            public int Index { get; private set; }
            public UniversalNavigationBarItemSection Section { get; private set; }
            public UniversalNavigationBarItem Item { get; private set; }
        }
    }
}
