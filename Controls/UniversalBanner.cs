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
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Controls
{
    [ContentProperty(Name = "Items")]
    public sealed class UniversalBanner : UserControl
    {
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex",
            typeof(int),
            typeof(UniversalBanner),
            new PropertyMetadata(0, OnSelectedIndexPropertyChanged));

        public static readonly DependencyProperty AutoScrollEnabledProperty = DependencyProperty.Register(
            "AutoScrollEnabled",
            typeof(bool),
            typeof(UniversalBanner),
            new PropertyMetadata(true, OnTimerPropertyChanged));

        public static readonly DependencyProperty ItemDisplayDurationProperty = DependencyProperty.Register(
            "ItemDisplayDuration",
            typeof(double),
            typeof(UniversalBanner),
            new PropertyMetadata(3000.0, OnTimerPropertyChanged));

        public static readonly DependencyProperty SlideAnimationEnabledProperty = DependencyProperty.Register(
            "SlideAnimationEnabled",
            typeof(bool),
            typeof(UniversalBanner),
            new PropertyMetadata(true, OnVisualPropertyChanged));

        public static readonly DependencyProperty SlideAnimationDurationProperty = DependencyProperty.Register(
            "SlideAnimationDuration",
            typeof(double),
            typeof(UniversalBanner),
            new PropertyMetadata(280.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty ScrollDirectionProperty = DependencyProperty.Register(
            "ScrollDirection",
            typeof(UniversalBannerScrollDirection),
            typeof(UniversalBanner),
            new PropertyMetadata(UniversalBannerScrollDirection.Horizontal, OnVisualPropertyChanged));

        public static readonly DependencyProperty ShowIndicatorsProperty = DependencyProperty.Register(
            "ShowIndicators",
            typeof(bool),
            typeof(UniversalBanner),
            new PropertyMetadata(true, OnVisualPropertyChanged));

        public static readonly DependencyProperty IndicatorPlacementProperty = DependencyProperty.Register(
            "IndicatorPlacement",
            typeof(UniversalBannerIndicatorPlacement),
            typeof(UniversalBanner),
            new PropertyMetadata(UniversalBannerIndicatorPlacement.Bottom, OnVisualPropertyChanged));

        public static readonly DependencyProperty IndicatorSizeProperty = DependencyProperty.Register(
            "IndicatorSize",
            typeof(double),
            typeof(UniversalBanner),
            new PropertyMetadata(8.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty IndicatorSpacingProperty = DependencyProperty.Register(
            "IndicatorSpacing",
            typeof(double),
            typeof(UniversalBanner),
            new PropertyMetadata(6.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty IndicatorActiveBrushProperty = DependencyProperty.Register(
            "IndicatorActiveBrush",
            typeof(Brush),
            typeof(UniversalBanner),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty IndicatorInactiveBrushProperty = DependencyProperty.Register(
            "IndicatorInactiveBrush",
            typeof(Brush),
            typeof(UniversalBanner),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty TextOverlayHeightProperty = DependencyProperty.Register(
            "TextOverlayHeight",
            typeof(double),
            typeof(UniversalBanner),
            new PropertyMetadata(150.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty TextPaddingProperty = DependencyProperty.Register(
            "TextPadding",
            typeof(Thickness),
            typeof(UniversalBanner),
            new PropertyMetadata(new Thickness(24, 0, 24, 30), OnVisualPropertyChanged));

        public static readonly DependencyProperty PrimaryTextForegroundProperty = DependencyProperty.Register(
            "PrimaryTextForeground",
            typeof(Brush),
            typeof(UniversalBanner),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty SecondaryTextForegroundProperty = DependencyProperty.Register(
            "SecondaryTextForeground",
            typeof(Brush),
            typeof(UniversalBanner),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty PrimaryTextFontSizeProperty = DependencyProperty.Register(
            "PrimaryTextFontSize",
            typeof(double),
            typeof(UniversalBanner),
            new PropertyMetadata(22.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty SecondaryTextFontSizeProperty = DependencyProperty.Register(
            "SecondaryTextFontSize",
            typeof(double),
            typeof(UniversalBanner),
            new PropertyMetadata(18.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty PrimaryTextMaxLengthProperty = DependencyProperty.Register(
            "PrimaryTextMaxLength",
            typeof(int),
            typeof(UniversalBanner),
            new PropertyMetadata(0, OnVisualPropertyChanged));

        public static readonly DependencyProperty SecondaryTextMaxLengthProperty = DependencyProperty.Register(
            "SecondaryTextMaxLength",
            typeof(int),
            typeof(UniversalBanner),
            new PropertyMetadata(0, OnVisualPropertyChanged));

        public static readonly DependencyProperty ImageStretchProperty = DependencyProperty.Register(
            "ImageStretch",
            typeof(Stretch),
            typeof(UniversalBanner),
            new PropertyMetadata(Stretch.UniformToFill, OnVisualPropertyChanged));

        public static readonly DependencyProperty BackgroundOverlayEnabledProperty = DependencyProperty.Register(
            "BackgroundOverlayEnabled",
            typeof(bool),
            typeof(UniversalBanner),
            new PropertyMetadata(true, OnVisualPropertyChanged));

        public static readonly DependencyProperty ActionTargetProperty = DependencyProperty.Register(
            "ActionTarget",
            typeof(object),
            typeof(UniversalBanner),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ActionTargetTypeProperty = DependencyProperty.Register(
            "ActionTargetType",
            typeof(string),
            typeof(UniversalBanner),
            new PropertyMetadata(string.Empty));

        private readonly Grid _root;
        private readonly Grid _viewport;
        private readonly StackPanel _indicatorPanel;
        private readonly DispatcherTimer _timer;
        private FrameworkElement _currentElement;
        private int _currentIndex = -1;
        private bool _isAnimating;
        private bool _ignoreSelectedIndexChange;

        public event EventHandler<UniversalBannerItemClickEventArgs> ItemClick;

        public UniversalBanner()
        {
            Items = new ObservableCollection<UniversalBannerItem>();
            Items.CollectionChanged += OnItemsCollectionChanged;

            _root = new Grid();
            _viewport = new Grid();
            _indicatorPanel = new StackPanel();
            _indicatorPanel.HorizontalAlignment = HorizontalAlignment.Center;
            _indicatorPanel.VerticalAlignment = VerticalAlignment.Bottom;
            _indicatorPanel.Margin = new Thickness(0, 0, 0, 12);

            _root.Children.Add(_viewport);
            _root.Children.Add(_indicatorPanel);

            Content = _root;

            _timer = new DispatcherTimer();
            _timer.Tick += OnTimerTick;

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            SizeChanged += OnSizeChanged;
        }

        public ObservableCollection<UniversalBannerItem> Items { get; private set; }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public bool AutoScrollEnabled
        {
            get { return (bool)GetValue(AutoScrollEnabledProperty); }
            set { SetValue(AutoScrollEnabledProperty, value); }
        }

        public double ItemDisplayDuration
        {
            get { return (double)GetValue(ItemDisplayDurationProperty); }
            set { SetValue(ItemDisplayDurationProperty, value); }
        }

        public bool SlideAnimationEnabled
        {
            get { return (bool)GetValue(SlideAnimationEnabledProperty); }
            set { SetValue(SlideAnimationEnabledProperty, value); }
        }

        public double SlideAnimationDuration
        {
            get { return (double)GetValue(SlideAnimationDurationProperty); }
            set { SetValue(SlideAnimationDurationProperty, value); }
        }

        public UniversalBannerScrollDirection ScrollDirection
        {
            get { return (UniversalBannerScrollDirection)GetValue(ScrollDirectionProperty); }
            set { SetValue(ScrollDirectionProperty, value); }
        }

        public bool ShowIndicators
        {
            get { return (bool)GetValue(ShowIndicatorsProperty); }
            set { SetValue(ShowIndicatorsProperty, value); }
        }

        public UniversalBannerIndicatorPlacement IndicatorPlacement
        {
            get { return (UniversalBannerIndicatorPlacement)GetValue(IndicatorPlacementProperty); }
            set { SetValue(IndicatorPlacementProperty, value); }
        }

        public double IndicatorSize
        {
            get { return (double)GetValue(IndicatorSizeProperty); }
            set { SetValue(IndicatorSizeProperty, value); }
        }

        public double IndicatorSpacing
        {
            get { return (double)GetValue(IndicatorSpacingProperty); }
            set { SetValue(IndicatorSpacingProperty, value); }
        }

        public Brush IndicatorActiveBrush
        {
            get { return (Brush)GetValue(IndicatorActiveBrushProperty); }
            set { SetValue(IndicatorActiveBrushProperty, value); }
        }

        public Brush IndicatorInactiveBrush
        {
            get { return (Brush)GetValue(IndicatorInactiveBrushProperty); }
            set { SetValue(IndicatorInactiveBrushProperty, value); }
        }

        public double TextOverlayHeight
        {
            get { return (double)GetValue(TextOverlayHeightProperty); }
            set { SetValue(TextOverlayHeightProperty, value); }
        }

        public Thickness TextPadding
        {
            get { return (Thickness)GetValue(TextPaddingProperty); }
            set { SetValue(TextPaddingProperty, value); }
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

        public double PrimaryTextFontSize
        {
            get { return (double)GetValue(PrimaryTextFontSizeProperty); }
            set { SetValue(PrimaryTextFontSizeProperty, value); }
        }

        public double SecondaryTextFontSize
        {
            get { return (double)GetValue(SecondaryTextFontSizeProperty); }
            set { SetValue(SecondaryTextFontSizeProperty, value); }
        }

        public int PrimaryTextMaxLength
        {
            get { return (int)GetValue(PrimaryTextMaxLengthProperty); }
            set { SetValue(PrimaryTextMaxLengthProperty, value); }
        }

        public int SecondaryTextMaxLength
        {
            get { return (int)GetValue(SecondaryTextMaxLengthProperty); }
            set { SetValue(SecondaryTextMaxLengthProperty, value); }
        }

        public Stretch ImageStretch
        {
            get { return (Stretch)GetValue(ImageStretchProperty); }
            set { SetValue(ImageStretchProperty, value); }
        }

        public bool BackgroundOverlayEnabled
        {
            get { return (bool)GetValue(BackgroundOverlayEnabledProperty); }
            set { SetValue(BackgroundOverlayEnabledProperty, value); }
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

        public void Next()
        {
            if (Items == null || Items.Count == 0)
            {
                return;
            }

            int next = _currentIndex < 0 ? 0 : (_currentIndex + 1) % Items.Count;
            SetSelectedIndex(next, true);
        }

        public void Previous()
        {
            if (Items == null || Items.Count == 0)
            {
                return;
            }

            int previous = _currentIndex <= 0 ? Items.Count - 1 : _currentIndex - 1;
            SetSelectedIndex(previous, true);
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            Refresh(false);
            RestartTimer();
        }

        private void OnUnloaded(object sender, RoutedEventArgs args)
        {
            StopTimer();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs args)
        {
            if (_root != null)
            {
                _root.Clip = new RectangleGeometry() { Rect = new Windows.Foundation.Rect(0, 0, Math.Max(0, ActualWidth), Math.Max(0, ActualHeight)) };
            }
        }

        private static void OnSelectedIndexPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            UniversalBanner banner = sender as UniversalBanner;
            if (banner != null && !banner._ignoreSelectedIndexChange)
            {
                banner.ShowIndex((int)args.NewValue, true);
                banner.RestartTimer();
            }
        }

        private static void OnTimerPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            UniversalBanner banner = sender as UniversalBanner;
            if (banner != null)
            {
                banner.RestartTimer();
            }
        }

        private static void OnVisualPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            UniversalBanner banner = sender as UniversalBanner;
            if (banner != null)
            {
                banner.Refresh(false);
            }
        }

        private void Refresh(bool animate)
        {
            if (Items == null || Items.Count == 0)
            {
                _viewport.Children.Clear();
                _indicatorPanel.Children.Clear();
                _currentElement = null;
                _currentIndex = -1;
                StopTimer();
                return;
            }

            int safeIndex = SelectedIndex;
            if (safeIndex < 0 || safeIndex >= Items.Count)
            {
                safeIndex = 0;
                SetSelectedIndex(safeIndex, false);
            }

            ShowIndex(safeIndex, animate);
            UpdateIndicators();
            RestartTimer();
        }

        private void SetSelectedIndex(int index, bool animate)
        {
            if (Items == null || Items.Count == 0)
            {
                return;
            }

            if (index < 0)
            {
                index = 0;
            }

            if (index >= Items.Count)
            {
                index = Items.Count - 1;
            }

            if (SelectedIndex != index)
            {
                _ignoreSelectedIndexChange = true;
                SelectedIndex = index;
                _ignoreSelectedIndexChange = false;
            }

            ShowIndex(index, animate);
            RestartTimer();
        }

        private void ShowIndex(int index, bool animate)
        {
            if (Items == null || Items.Count == 0 || index < 0 || index >= Items.Count)
            {
                return;
            }

            if (_isAnimating)
            {
                animate = false;
            }

            if (_currentIndex == index && _currentElement != null)
            {
                UpdateIndicators();
                return;
            }

            FrameworkElement newElement = CreateItemElement(Items[index], index);

            if (_currentElement == null || !animate || !SlideAnimationEnabled || SlideAnimationDuration <= 0)
            {
                _viewport.Children.Clear();
                _viewport.Children.Add(newElement);
                _currentElement = newElement;
                _currentIndex = index;
                UpdateIndicators();
                return;
            }

            FrameworkElement oldElement = _currentElement;
            double offset = GetSlideOffset();

            CompositeTransform oldTransform = new CompositeTransform();
            CompositeTransform newTransform = new CompositeTransform();
            oldElement.RenderTransform = oldTransform;
            newElement.RenderTransform = newTransform;
            oldElement.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
            newElement.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);

            if (ScrollDirection == UniversalBannerScrollDirection.Vertical)
            {
                newTransform.TranslateY = offset;
            }
            else
            {
                newTransform.TranslateX = offset;
            }

            _viewport.Children.Add(newElement);
            _isAnimating = true;

            Storyboard storyboard = new Storyboard();
            if (ScrollDirection == UniversalBannerScrollDirection.Vertical)
            {
                storyboard.Children.Add(CreateSlideAnimation(oldTransform, -offset, "TranslateY"));
                storyboard.Children.Add(CreateSlideAnimation(newTransform, 0, "TranslateY"));
            }
            else
            {
                storyboard.Children.Add(CreateSlideAnimation(oldTransform, -offset, "TranslateX"));
                storyboard.Children.Add(CreateSlideAnimation(newTransform, 0, "TranslateX"));
            }

            storyboard.Children.Add(CreateOpacityAnimation(oldElement, 0.0));
            storyboard.Children.Add(CreateOpacityAnimation(newElement, 1.0));
            storyboard.Completed += delegate
            {
                _viewport.Children.Remove(oldElement);
                oldElement.Opacity = 1.0;
                _currentElement = newElement;
                _currentIndex = index;
                _isAnimating = false;
                UpdateIndicators();
            };
            storyboard.Begin();
        }

        private double GetSlideOffset()
        {
            double offset = ScrollDirection == UniversalBannerScrollDirection.Vertical ? ActualHeight : ActualWidth;
            if (offset <= 0)
            {
                offset = ScrollDirection == UniversalBannerScrollDirection.Vertical ? Height : Width;
            }
            if (double.IsNaN(offset) || offset <= 0)
            {
                offset = 300;
            }
            return offset;
        }

        private DoubleAnimation CreateSlideAnimation(DependencyObject target, double to, string propertyPath)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.To = to;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(Math.Max(0, SlideAnimationDuration)));
            animation.EnableDependentAnimation = true;
            animation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, propertyPath);
            return animation;
        }

        private DoubleAnimation CreateOpacityAnimation(DependencyObject target, double to)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.To = to;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(Math.Max(0, SlideAnimationDuration)));
            animation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, "Opacity");
            return animation;
        }

        private FrameworkElement CreateItemElement(UniversalBannerItem item, int index)
        {
            Grid root = new Grid();
            root.HorizontalAlignment = HorizontalAlignment.Stretch;
            root.VerticalAlignment = VerticalAlignment.Stretch;
            root.Tag = new BannerElementTag(index, item);
            root.Tapped += OnItemTapped;

            Image image = new Image();
            image.Stretch = ImageStretch;
            image.HorizontalAlignment = HorizontalAlignment.Stretch;
            image.VerticalAlignment = VerticalAlignment.Stretch;

            if (item != null && !string.IsNullOrWhiteSpace(item.ImagePath))
            {
                try
                {
                    image.Source = new BitmapImage(new Uri(item.ImagePath, UriKind.RelativeOrAbsolute));
                }
                catch
                {
                }
            }

            root.Children.Add(image);

            if (BackgroundOverlayEnabled)
            {
                Grid overlay = CreateOverlay();
                root.Children.Add(overlay);
            }

            StackPanel textPanel = new StackPanel();
            textPanel.VerticalAlignment = VerticalAlignment.Bottom;
            textPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
            textPanel.Margin = TextPadding;

            TextBlock primaryText = new TextBlock();
            primaryText.Text = LimitText(item != null ? item.PrimaryText : string.Empty, PrimaryTextMaxLength);
            primaryText.Foreground = item != null && item.PrimaryTextForeground != null ? item.PrimaryTextForeground : GetDefaultPrimaryTextBrush();
            primaryText.FontSize = PrimaryTextFontSize;
            primaryText.TextAlignment = TextAlignment.Center;
            primaryText.TextWrapping = TextWrapping.NoWrap;
            primaryText.TextTrimming = TextTrimming.CharacterEllipsis;

            TextBlock secondaryText = new TextBlock();
            secondaryText.Text = LimitText(item != null ? item.SecondaryText : string.Empty, SecondaryTextMaxLength);
            secondaryText.Foreground = item != null && item.SecondaryTextForeground != null ? item.SecondaryTextForeground : GetDefaultSecondaryTextBrush();
            secondaryText.FontSize = SecondaryTextFontSize;
            secondaryText.Margin = new Thickness(0, 4, 0, 0);
            secondaryText.TextAlignment = TextAlignment.Center;
            secondaryText.TextWrapping = TextWrapping.NoWrap;
            secondaryText.TextTrimming = TextTrimming.CharacterEllipsis;
            secondaryText.Visibility = string.IsNullOrEmpty(secondaryText.Text) ? Visibility.Collapsed : Visibility.Visible;

            if (!string.IsNullOrEmpty(primaryText.Text))
            {
                textPanel.Children.Add(primaryText);
            }
            textPanel.Children.Add(secondaryText);
            root.Children.Add(textPanel);

            return root;
        }

        private Grid CreateOverlay()
        {
            Grid overlay = new Grid();
            overlay.Height = Math.Max(0, TextOverlayHeight);
            overlay.VerticalAlignment = VerticalAlignment.Bottom;
            LinearGradientBrush brush = new LinearGradientBrush();
            brush.StartPoint = new Windows.Foundation.Point(0, 0);
            brush.EndPoint = new Windows.Foundation.Point(0, 1);
            brush.GradientStops.Add(new GradientStop() { Offset = 0, Color = Color.FromArgb(0, 17, 19, 16) });
            brush.GradientStops.Add(new GradientStop() { Offset = 0.5, Color = Color.FromArgb(204, 17, 19, 16) });
            brush.GradientStops.Add(new GradientStop() { Offset = 1, Color = Color.FromArgb(255, 17, 19, 16) });
            overlay.Background = brush;
            return overlay;
        }

        private string LimitText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || maxLength <= 0 || text.Length <= maxLength)
            {
                return text ?? string.Empty;
            }

            if (maxLength <= 3)
            {
                return new string('.', maxLength);
            }

            return text.Substring(0, maxLength - 3) + "...";
        }

        private Brush GetDefaultPrimaryTextBrush()
        {
            if (PrimaryTextForeground != null)
            {
                return PrimaryTextForeground;
            }

            return new SolidColorBrush(Colors.White);
        }

        private Brush GetDefaultSecondaryTextBrush()
        {
            if (SecondaryTextForeground != null)
            {
                return SecondaryTextForeground;
            }

            return new SolidColorBrush(Color.FromArgb(153, 255, 255, 255));
        }

        private void OnItemTapped(object sender, TappedRoutedEventArgs args)
        {
            FrameworkElement element = sender as FrameworkElement;
            BannerElementTag tag = element != null ? element.Tag as BannerElementTag : null;
            if (tag == null || tag.Item == null)
            {
                return;
            }

            UniversalBannerItemClickEventArgs clickArgs = new UniversalBannerItemClickEventArgs(tag.Index, tag.Item, tag.Item.ClickAction);
            RaiseItemClick(clickArgs);
            if (clickArgs.Handled)
            {
                return;
            }

            UniversalActionInvoker.Invoke(this, ActionTarget, ActionTargetType, clickArgs.ClickAction, clickArgs, tag.Item, tag.Index);
        }

        private void RaiseItemClick(UniversalBannerItemClickEventArgs args)
        {
            EventHandler<UniversalBannerItemClickEventArgs> handler = ItemClick;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void UpdateIndicators()
        {
            _indicatorPanel.Children.Clear();

            if (!ShowIndicators || Items == null || Items.Count == 0)
            {
                _indicatorPanel.Visibility = Visibility.Collapsed;
                return;
            }

            _indicatorPanel.Visibility = Visibility.Visible;
            ApplyIndicatorPlacement();

            for (int i = 0; i < Items.Count; i++)
            {
                Border dot = new Border();
                double size = Math.Max(0, IndicatorSize);
                dot.Width = size;
                dot.Height = size;
                dot.CornerRadius = new CornerRadius(size / 2.0);
                dot.Background = i == _currentIndex ? GetActiveIndicatorBrush() : GetInactiveIndicatorBrush();
                dot.Margin = GetIndicatorDotMargin();
                dot.Tag = i;
                dot.Tapped += OnIndicatorTapped;
                _indicatorPanel.Children.Add(dot);
            }
        }

        private void ApplyIndicatorPlacement()
        {
            switch (IndicatorPlacement)
            {
                case UniversalBannerIndicatorPlacement.Top:
                    _indicatorPanel.Orientation = Orientation.Horizontal;
                    _indicatorPanel.HorizontalAlignment = HorizontalAlignment.Center;
                    _indicatorPanel.VerticalAlignment = VerticalAlignment.Top;
                    _indicatorPanel.Margin = new Thickness(0, 12, 0, 0);
                    break;
                case UniversalBannerIndicatorPlacement.Left:
                    _indicatorPanel.Orientation = Orientation.Vertical;
                    _indicatorPanel.HorizontalAlignment = HorizontalAlignment.Left;
                    _indicatorPanel.VerticalAlignment = VerticalAlignment.Center;
                    _indicatorPanel.Margin = new Thickness(12, 0, 0, 0);
                    break;
                case UniversalBannerIndicatorPlacement.Right:
                    _indicatorPanel.Orientation = Orientation.Vertical;
                    _indicatorPanel.HorizontalAlignment = HorizontalAlignment.Right;
                    _indicatorPanel.VerticalAlignment = VerticalAlignment.Center;
                    _indicatorPanel.Margin = new Thickness(0, 0, 12, 0);
                    break;
                default:
                    _indicatorPanel.Orientation = Orientation.Horizontal;
                    _indicatorPanel.HorizontalAlignment = HorizontalAlignment.Center;
                    _indicatorPanel.VerticalAlignment = VerticalAlignment.Bottom;
                    _indicatorPanel.Margin = new Thickness(0, 0, 0, 12);
                    break;
            }
        }

        private Thickness GetIndicatorDotMargin()
        {
            double spacing = Math.Max(0, IndicatorSpacing) / 2.0;
            if (IndicatorPlacement == UniversalBannerIndicatorPlacement.Left || IndicatorPlacement == UniversalBannerIndicatorPlacement.Right)
            {
                return new Thickness(0, spacing, 0, spacing);
            }

            return new Thickness(spacing, 0, spacing, 0);
        }

        private Brush GetActiveIndicatorBrush()
        {
            if (IndicatorActiveBrush != null)
            {
                return IndicatorActiveBrush;
            }

            return new SolidColorBrush(Colors.White);
        }

        private Brush GetInactiveIndicatorBrush()
        {
            if (IndicatorInactiveBrush != null)
            {
                return IndicatorInactiveBrush;
            }

            return new SolidColorBrush(Color.FromArgb(120, 255, 255, 255));
        }

        private void OnIndicatorTapped(object sender, TappedRoutedEventArgs args)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null || !(element.Tag is int))
            {
                return;
            }

            SetSelectedIndex((int)element.Tag, true);
        }

        private void RestartTimer()
        {
            StopTimer();

            if (!AutoScrollEnabled || Items == null || Items.Count <= 1 || ItemDisplayDuration <= 0)
            {
                return;
            }

            _timer.Interval = TimeSpan.FromMilliseconds(Math.Max(50, ItemDisplayDuration));
            _timer.Start();
        }

        private void StopTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
        }

        private void OnTimerTick(object sender, object args)
        {
            Next();
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.OldItems != null)
            {
                foreach (object oldObject in args.OldItems)
                {
                    UniversalBannerItem oldItem = oldObject as UniversalBannerItem;
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
                    UniversalBannerItem newItem = newObject as UniversalBannerItem;
                    if (newItem != null)
                    {
                        newItem.Changed += OnItemChanged;
                    }
                }
            }

            Refresh(false);
        }

        private void OnItemChanged(object sender, EventArgs args)
        {
            Refresh(false);
        }

        private sealed class BannerElementTag
        {
            public BannerElementTag(int index, UniversalBannerItem item)
            {
                Index = index;
                Item = item;
            }

            public int Index { get; private set; }
            public UniversalBannerItem Item { get; private set; }
        }
    }
}
