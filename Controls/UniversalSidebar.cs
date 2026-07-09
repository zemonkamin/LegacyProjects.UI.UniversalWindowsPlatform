using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using LegacyProjects.UI.UniversalWindowsPlatform.Enums;
using LegacyProjects.UI.UniversalWindowsPlatform.Events;
using LegacyProjects.UI.UniversalWindowsPlatform.Helpers;
using LegacyProjects.UI.UniversalWindowsPlatform.Models;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Controls
{
    [ContentProperty(Name = "Items")]
    public sealed class UniversalSidebar : UserControl
    {
        public static readonly DependencyProperty IsPaneOpenProperty = DependencyProperty.Register(
            "IsPaneOpen",
            typeof(bool),
            typeof(UniversalSidebar),
            new PropertyMetadata(false, OnPaneStatePropertyChanged));

        public static readonly DependencyProperty OpenPaneWidthProperty = DependencyProperty.Register(
            "OpenPaneWidth",
            typeof(double),
            typeof(UniversalSidebar),
            new PropertyMetadata(300.0, OnLayoutPropertyChanged));

        public static readonly DependencyProperty CompactPaneWidthProperty = DependencyProperty.Register(
            "CompactPaneWidth",
            typeof(double),
            typeof(UniversalSidebar),
            new PropertyMetadata(56.0, OnLayoutPropertyChanged));

        public static readonly DependencyProperty HorizontalBreakpointProperty = DependencyProperty.Register(
            "HorizontalBreakpoint",
            typeof(double),
            typeof(UniversalSidebar),
            new PropertyMetadata(768.0, OnLayoutPropertyChanged));

        public static readonly DependencyProperty ShowIconsWhenClosedInHorizontalModeProperty = DependencyProperty.Register(
            "ShowIconsWhenClosedInHorizontalMode",
            typeof(bool),
            typeof(UniversalSidebar),
            new PropertyMetadata(false, OnLayoutPropertyChanged));

        public static readonly DependencyProperty ShowIconsProperty = DependencyProperty.Register(
            "ShowIcons",
            typeof(bool),
            typeof(UniversalSidebar),
            new PropertyMetadata(true, OnVisualPropertyChanged));

        public static readonly DependencyProperty BackgroundOpacityProperty = DependencyProperty.Register(
            "BackgroundOpacity",
            typeof(double),
            typeof(UniversalSidebar),
            new PropertyMetadata(0.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty TopItemsPinnedProperty = DependencyProperty.Register(
            "TopItemsPinned",
            typeof(bool),
            typeof(UniversalSidebar),
            new PropertyMetadata(true, OnVisualPropertyChanged));

        public static readonly DependencyProperty BottomItemsPinnedProperty = DependencyProperty.Register(
            "BottomItemsPinned",
            typeof(bool),
            typeof(UniversalSidebar),
            new PropertyMetadata(true, OnVisualPropertyChanged));

        public static readonly DependencyProperty TopOffsetProperty = DependencyProperty.Register(
            "TopOffset",
            typeof(double),
            typeof(UniversalSidebar),
            new PropertyMetadata(0.0, OnLayoutPropertyChanged));

        public static readonly DependencyProperty BottomOffsetProperty = DependencyProperty.Register(
            "BottomOffset",
            typeof(double),
            typeof(UniversalSidebar),
            new PropertyMetadata(0.0, OnLayoutPropertyChanged));

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex",
            typeof(int),
            typeof(UniversalSidebar),
            new PropertyMetadata(-1, OnSelectedIndexChanged));

        public static readonly DependencyProperty SelectedKeyProperty = DependencyProperty.Register(
            "SelectedKey",
            typeof(string),
            typeof(UniversalSidebar),
            new PropertyMetadata(string.Empty, OnVisualPropertyChanged));

        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register(
            "ItemHeight",
            typeof(double),
            typeof(UniversalSidebar),
            new PropertyMetadata(56.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty ItemPaddingProperty = DependencyProperty.Register(
            "ItemPadding",
            typeof(Thickness),
            typeof(UniversalSidebar),
            new PropertyMetadata(new Thickness(16, 0, 16, 0), OnVisualPropertyChanged));

        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(
            "IconSize",
            typeof(double),
            typeof(UniversalSidebar),
            new PropertyMetadata(18.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty IconTextSpacingProperty = DependencyProperty.Register(
            "IconTextSpacing",
            typeof(double),
            typeof(UniversalSidebar),
            new PropertyMetadata(16.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty LabelFontSizeProperty = DependencyProperty.Register(
            "LabelFontSize",
            typeof(double),
            typeof(UniversalSidebar),
            new PropertyMetadata(16.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty SelectedItemBackgroundProperty = DependencyProperty.Register(
            "SelectedItemBackground",
            typeof(Brush),
            typeof(UniversalSidebar),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty UnselectedItemBackgroundProperty = DependencyProperty.Register(
            "UnselectedItemBackground",
            typeof(Brush),
            typeof(UniversalSidebar),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty DisabledItemBackgroundProperty = DependencyProperty.Register(
            "DisabledItemBackground",
            typeof(Brush),
            typeof(UniversalSidebar),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty SelectedItemForegroundProperty = DependencyProperty.Register(
            "SelectedItemForeground",
            typeof(Brush),
            typeof(UniversalSidebar),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty UnselectedItemForegroundProperty = DependencyProperty.Register(
            "UnselectedItemForeground",
            typeof(Brush),
            typeof(UniversalSidebar),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty DisabledItemForegroundProperty = DependencyProperty.Register(
            "DisabledItemForeground",
            typeof(Brush),
            typeof(UniversalSidebar),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty DisabledItemOpacityProperty = DependencyProperty.Register(
            "DisabledItemOpacity",
            typeof(double),
            typeof(UniversalSidebar),
            new PropertyMetadata(0.45, OnVisualPropertyChanged));

        public static readonly DependencyProperty SeparatorEnabledProperty = DependencyProperty.Register(
            "SeparatorEnabled",
            typeof(bool),
            typeof(UniversalSidebar),
            new PropertyMetadata(true, OnVisualPropertyChanged));

        public static readonly DependencyProperty SeparatorBrushProperty = DependencyProperty.Register(
            "SeparatorBrush",
            typeof(Brush),
            typeof(UniversalSidebar),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty SeparatorThicknessProperty = DependencyProperty.Register(
            "SeparatorThickness",
            typeof(double),
            typeof(UniversalSidebar),
            new PropertyMetadata(1.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty SidebarBorderBrushProperty = DependencyProperty.Register(
            "SidebarBorderBrush",
            typeof(Brush),
            typeof(UniversalSidebar),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        public static readonly DependencyProperty SidebarBorderThicknessProperty = DependencyProperty.Register(
            "SidebarBorderThickness",
            typeof(Thickness),
            typeof(UniversalSidebar),
            new PropertyMetadata(new Thickness(0, 0, 1, 0), OnVisualPropertyChanged));

        public static readonly DependencyProperty SideSeparatorEnabledProperty = DependencyProperty.Register(
            "SideSeparatorEnabled",
            typeof(bool),
            typeof(UniversalSidebar),
            new PropertyMetadata(true, OnVisualPropertyChanged));

        public static readonly DependencyProperty AnimationEnabledProperty = DependencyProperty.Register(
            "AnimationEnabled",
            typeof(bool),
            typeof(UniversalSidebar),
            new PropertyMetadata(true));

        public static readonly DependencyProperty AnimationDurationProperty = DependencyProperty.Register(
            "AnimationDuration",
            typeof(double),
            typeof(UniversalSidebar),
            new PropertyMetadata(220.0));

        public static readonly DependencyProperty AnimationEasingModeProperty = DependencyProperty.Register(
            "AnimationEasingMode",
            typeof(string),
            typeof(UniversalSidebar),
            new PropertyMetadata("EaseOut"));

        public static readonly DependencyProperty AnimationEasingFunctionProperty = DependencyProperty.Register(
            "AnimationEasingFunction",
            typeof(string),
            typeof(UniversalSidebar),
            new PropertyMetadata("Cubic"));


        public static readonly DependencyProperty PressAnimationEnabledProperty = DependencyProperty.Register(
            "PressAnimationEnabled",
            typeof(bool),
            typeof(UniversalSidebar),
            new PropertyMetadata(true, OnVisualPropertyChanged));

        public static readonly DependencyProperty PressAnimationScaleProperty = DependencyProperty.Register(
            "PressAnimationScale",
            typeof(double),
            typeof(UniversalSidebar),
            new PropertyMetadata(0.96));

        public static readonly DependencyProperty PressAnimationDurationProperty = DependencyProperty.Register(
            "PressAnimationDuration",
            typeof(double),
            typeof(UniversalSidebar),
            new PropertyMetadata(90.0));

        public static readonly DependencyProperty ActionTargetProperty = DependencyProperty.Register(
            "ActionTarget",
            typeof(object),
            typeof(UniversalSidebar),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ActionTargetTypeProperty = DependencyProperty.Register(
            "ActionTargetType",
            typeof(string),
            typeof(UniversalSidebar),
            new PropertyMetadata(string.Empty));

        private readonly Grid _root;
        private readonly Border _clipBorder;
        private readonly Border _paneBorder;
        private readonly Grid _paneGrid;
        private readonly StackPanel _topPinnedPanel;
        private readonly StackPanel _scrollPanel;
        private readonly StackPanel _bottomPinnedPanel;
        private readonly ScrollViewer _scrollViewer;
        private Storyboard _widthStoryboard;
        private bool _isHorizontalMode;
        private bool _isLoaded;
        private double _currentPaneWidth;
        private int _lastSelectedIndex;

        public event EventHandler<UniversalSidebarItemClickEventArgs> ItemClick;
        public event EventHandler<UniversalSidebarSelectionChangedEventArgs> SelectionChanged;
        public event EventHandler PaneStateChanged;

        public UniversalSidebar()
        {
            TopItems = new ObservableCollection<UniversalSidebarItem>();
            Items = new ObservableCollection<UniversalSidebarItem>();
            BottomItems = new ObservableCollection<UniversalSidebarItem>();

            TopItems.CollectionChanged += OnItemsCollectionChanged;
            Items.CollectionChanged += OnItemsCollectionChanged;
            BottomItems.CollectionChanged += OnItemsCollectionChanged;

            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Stretch;

            _root = new Grid();
            _root.HorizontalAlignment = HorizontalAlignment.Left;
            _root.VerticalAlignment = VerticalAlignment.Stretch;

            _clipBorder = new Border();
            _clipBorder.HorizontalAlignment = HorizontalAlignment.Left;
            _clipBorder.VerticalAlignment = VerticalAlignment.Stretch;
            _clipBorder.SizeChanged += OnClipBorderSizeChanged;

            _paneBorder = new Border();
            _paneBorder.HorizontalAlignment = HorizontalAlignment.Left;
            _paneBorder.VerticalAlignment = VerticalAlignment.Stretch;

            _paneGrid = new Grid();
            _paneGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            _paneGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            _paneGrid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            _topPinnedPanel = new StackPanel();
            _scrollPanel = new StackPanel();
            _bottomPinnedPanel = new StackPanel();

            _scrollViewer = new ScrollViewer();
            _scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            _scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            _scrollViewer.VerticalScrollMode = ScrollMode.Enabled;
            _scrollViewer.HorizontalScrollMode = ScrollMode.Disabled;
            _scrollViewer.Content = _scrollPanel;

            Grid.SetRow(_topPinnedPanel, 0);
            Grid.SetRow(_scrollViewer, 1);
            Grid.SetRow(_bottomPinnedPanel, 2);
            _paneGrid.Children.Add(_topPinnedPanel);
            _paneGrid.Children.Add(_scrollViewer);
            _paneGrid.Children.Add(_bottomPinnedPanel);

            _paneBorder.Child = _paneGrid;
            _clipBorder.Child = _paneBorder;
            _root.Children.Add(_clipBorder);
            Content = _root;

            _lastSelectedIndex = SelectedIndex;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
            RegisterPropertyChangedCallback(Control.BackgroundProperty, OnBackgroundPropertyChanged);
        }

        private void OnBackgroundPropertyChanged(DependencyObject sender, DependencyProperty property)
        {
            UpdateVisuals();
        }

        public ObservableCollection<UniversalSidebarItem> TopItems { get; private set; }
        public ObservableCollection<UniversalSidebarItem> Items { get; private set; }
        public ObservableCollection<UniversalSidebarItem> BottomItems { get; private set; }

        public bool IsPaneOpen
        {
            get { return (bool)GetValue(IsPaneOpenProperty); }
            set { SetValue(IsPaneOpenProperty, value); }
        }

        public double OpenPaneWidth
        {
            get { return (double)GetValue(OpenPaneWidthProperty); }
            set { SetValue(OpenPaneWidthProperty, value); }
        }

        public double CompactPaneWidth
        {
            get { return (double)GetValue(CompactPaneWidthProperty); }
            set { SetValue(CompactPaneWidthProperty, value); }
        }

        public double HorizontalBreakpoint
        {
            get { return (double)GetValue(HorizontalBreakpointProperty); }
            set { SetValue(HorizontalBreakpointProperty, value); }
        }

        public bool ShowIconsWhenClosedInHorizontalMode
        {
            get { return (bool)GetValue(ShowIconsWhenClosedInHorizontalModeProperty); }
            set { SetValue(ShowIconsWhenClosedInHorizontalModeProperty, value); }
        }

        public bool ShowIcons
        {
            get { return (bool)GetValue(ShowIconsProperty); }
            set { SetValue(ShowIconsProperty, value); }
        }

        public double BackgroundOpacity
        {
            get { return (double)GetValue(BackgroundOpacityProperty); }
            set { SetValue(BackgroundOpacityProperty, value); }
        }

        public bool TopItemsPinned
        {
            get { return (bool)GetValue(TopItemsPinnedProperty); }
            set { SetValue(TopItemsPinnedProperty, value); }
        }

        public bool BottomItemsPinned
        {
            get { return (bool)GetValue(BottomItemsPinnedProperty); }
            set { SetValue(BottomItemsPinnedProperty, value); }
        }

        public double TopOffset
        {
            get { return (double)GetValue(TopOffsetProperty); }
            set { SetValue(TopOffsetProperty, value); }
        }

        public double BottomOffset
        {
            get { return (double)GetValue(BottomOffsetProperty); }
            set { SetValue(BottomOffsetProperty, value); }
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public string SelectedKey
        {
            get { return (string)GetValue(SelectedKeyProperty); }
            set { SetValue(SelectedKeyProperty, value); }
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

        public Brush SelectedItemBackground
        {
            get { return (Brush)GetValue(SelectedItemBackgroundProperty); }
            set { SetValue(SelectedItemBackgroundProperty, value); }
        }

        public Brush UnselectedItemBackground
        {
            get { return (Brush)GetValue(UnselectedItemBackgroundProperty); }
            set { SetValue(UnselectedItemBackgroundProperty, value); }
        }

        public Brush DisabledItemBackground
        {
            get { return (Brush)GetValue(DisabledItemBackgroundProperty); }
            set { SetValue(DisabledItemBackgroundProperty, value); }
        }

        public Brush SelectedItemForeground
        {
            get { return (Brush)GetValue(SelectedItemForegroundProperty); }
            set { SetValue(SelectedItemForegroundProperty, value); }
        }

        public Brush UnselectedItemForeground
        {
            get { return (Brush)GetValue(UnselectedItemForegroundProperty); }
            set { SetValue(UnselectedItemForegroundProperty, value); }
        }

        public Brush DisabledItemForeground
        {
            get { return (Brush)GetValue(DisabledItemForegroundProperty); }
            set { SetValue(DisabledItemForegroundProperty, value); }
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

        public Brush SidebarBorderBrush
        {
            get { return (Brush)GetValue(SidebarBorderBrushProperty); }
            set { SetValue(SidebarBorderBrushProperty, value); }
        }

        public Thickness SidebarBorderThickness
        {
            get { return (Thickness)GetValue(SidebarBorderThicknessProperty); }
            set { SetValue(SidebarBorderThicknessProperty, value); }
        }

        public bool SideSeparatorEnabled
        {
            get { return (bool)GetValue(SideSeparatorEnabledProperty); }
            set { SetValue(SideSeparatorEnabledProperty, value); }
        }

        public Brush SideSeparatorBrush
        {
            get { return SidebarBorderBrush; }
            set { SidebarBorderBrush = value; }
        }

        public double SideSeparatorThickness
        {
            get { return SidebarBorderThickness.Right; }
            set { SidebarBorderThickness = new Thickness(0, 0, Math.Max(0, value), 0); }
        }

        public bool AnimationEnabled
        {
            get { return (bool)GetValue(AnimationEnabledProperty); }
            set { SetValue(AnimationEnabledProperty, value); }
        }

        public double AnimationDuration
        {
            get { return (double)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        public string AnimationEasingMode
        {
            get { return (string)GetValue(AnimationEasingModeProperty); }
            set { SetValue(AnimationEasingModeProperty, value); }
        }

        public string AnimationEasingFunction
        {
            get { return (string)GetValue(AnimationEasingFunctionProperty); }
            set { SetValue(AnimationEasingFunctionProperty, value); }
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

        public void Toggle()
        {
            IsPaneOpen = !IsPaneOpen;
        }

        public void Open()
        {
            IsPaneOpen = true;
        }

        public void Close()
        {
            IsPaneOpen = false;
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            _isLoaded = true;
            if (Window.Current != null)
            {
                Window.Current.SizeChanged += OnWindowSizeChanged;
            }

            UpdateLayoutState(false);
            UpdateVisuals();
        }

        private void OnUnloaded(object sender, RoutedEventArgs args)
        {
            _isLoaded = false;
            if (Window.Current != null)
            {
                Window.Current.SizeChanged -= OnWindowSizeChanged;
            }
        }

        private void OnWindowSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs args)
        {
            UpdateLayoutState(true);
            UpdateVisuals();
        }

        private void OnClipBorderSizeChanged(object sender, SizeChangedEventArgs args)
        {
            if (_clipBorder != null)
            {
                UpdateClipGeometry(_clipBorder.ActualWidth);
            }
        }

        private static void OnPaneStatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            UniversalSidebar sidebar = sender as UniversalSidebar;
            if (sidebar != null)
            {
                sidebar.UpdateLayoutState(true);
                sidebar.UpdateVisuals();
                sidebar.RaisePaneStateChanged();
            }
        }

        private static void OnLayoutPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            UniversalSidebar sidebar = sender as UniversalSidebar;
            if (sidebar != null)
            {
                sidebar.UpdateLayoutState(true);
                sidebar.UpdateVisuals();
            }
        }

        private static void OnVisualPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            UniversalSidebar sidebar = sender as UniversalSidebar;
            if (sidebar != null)
            {
                sidebar.UpdateVisuals();
            }
        }

        private static void OnSelectedIndexChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            UniversalSidebar sidebar = sender as UniversalSidebar;
            if (sidebar != null)
            {
                int oldIndex = args.OldValue is int ? (int)args.OldValue : -1;
                int newIndex = args.NewValue is int ? (int)args.NewValue : -1;
                sidebar.RaiseSelectionChanged(oldIndex, newIndex);
                sidebar._lastSelectedIndex = newIndex;
                sidebar.UpdateVisuals();
            }
        }

        private void UpdateLayoutState(bool animate)
        {
            if (_root == null || _clipBorder == null || _paneBorder == null)
            {
                return;
            }

            double width = 0;
            if (Window.Current != null)
            {
                width = Window.Current.Bounds.Width;
            }
            else
            {
                width = ActualWidth;
            }

            _isHorizontalMode = width > HorizontalBreakpoint;

            double openWidth = Math.Max(0, OpenPaneWidth);
            double targetWidth = IsPaneOpen ? openWidth : GetClosedPaneWidth();

            _paneBorder.Width = openWidth;
            _root.Margin = new Thickness(0, Math.Max(0, TopOffset), 0, Math.Max(0, BottomOffset));

            if (!_isLoaded || !animate || !AnimationEnabled || Math.Max(0, AnimationDuration) <= 0)
            {
                SetVisiblePaneWidth(targetWidth);
                return;
            }

            AnimateVisiblePaneWidth(targetWidth);
        }

        private double GetClosedPaneWidth()
        {
            if (_isHorizontalMode && ShowIconsWhenClosedInHorizontalMode)
            {
                return Math.Max(0, CompactPaneWidth);
            }

            return 0;
        }

        private void SetVisiblePaneWidth(double width)
        {
            double safeWidth = Math.Max(0, width);
            StopWidthAnimation();
            _currentPaneWidth = safeWidth;
            Width = safeWidth;
            _root.Width = safeWidth;
            _clipBorder.Width = safeWidth;
            UpdateClipGeometry(safeWidth);
        }

        private void AnimateVisiblePaneWidth(double targetWidth)
        {
            double safeTargetWidth = Math.Max(0, targetWidth);
            double from = GetCurrentVisiblePaneWidth();

            if (Math.Abs(from - safeTargetWidth) < 0.1)
            {
                SetVisiblePaneWidth(safeTargetWidth);
                return;
            }

            StopWidthAnimation();

            double outerWidth = Math.Max(from, safeTargetWidth);
            Width = outerWidth;
            _root.Width = from;
            _clipBorder.Width = from;
            _currentPaneWidth = from;
            UpdateClipGeometry(from);

            _widthStoryboard = new Storyboard();
            _widthStoryboard.Children.Add(CreateWidthAnimation(_root, from, safeTargetWidth));
            _widthStoryboard.Children.Add(CreateWidthAnimation(_clipBorder, from, safeTargetWidth));
            _widthStoryboard.Completed += delegate
            {
                _currentPaneWidth = safeTargetWidth;
                Width = safeTargetWidth;
                _root.Width = safeTargetWidth;
                _clipBorder.Width = safeTargetWidth;
                UpdateClipGeometry(safeTargetWidth);
                _widthStoryboard = null;
            };

            _widthStoryboard.Begin();
        }

        private double GetCurrentVisiblePaneWidth()
        {
            double from = _clipBorder != null ? _clipBorder.Width : 0;

            if (double.IsNaN(from) || from < 0)
            {
                from = _clipBorder != null ? _clipBorder.ActualWidth : 0;
            }

            if (double.IsNaN(from) || from < 0)
            {
                from = _currentPaneWidth;
            }

            if (double.IsNaN(from) || from < 0)
            {
                from = Width;
            }

            if (double.IsNaN(from) || from < 0)
            {
                from = 0;
            }

            return Math.Max(0, from);
        }

        private DoubleAnimation CreateWidthAnimation(DependencyObject target, double from, double to)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = from;
            animation.To = to;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(Math.Max(0, AnimationDuration)));
            animation.EasingFunction = CreateEasingFunction();
            animation.EnableDependentAnimation = true;
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, "Width");
            return animation;
        }

        private void StopWidthAnimation()
        {
            if (_widthStoryboard != null)
            {
                try
                {
                    _widthStoryboard.Stop();
                }
                catch
                {
                }

                _widthStoryboard = null;
            }
        }

        private EasingFunctionBase CreateEasingFunction()
        {
            string functionName = AnimationEasingFunction;
            EasingFunctionBase function;

            if (StringEquals(functionName, "Quadratic"))
            {
                function = new QuadraticEase();
            }
            else if (StringEquals(functionName, "Back"))
            {
                function = new BackEase();
            }
            else if (StringEquals(functionName, "Circle"))
            {
                function = new CircleEase();
            }
            else if (StringEquals(functionName, "Exponential"))
            {
                function = new ExponentialEase();
            }
            else
            {
                function = new CubicEase();
            }

            if (StringEquals(AnimationEasingMode, "EaseIn"))
            {
                function.EasingMode = EasingMode.EaseIn;
            }
            else if (StringEquals(AnimationEasingMode, "EaseInOut"))
            {
                function.EasingMode = EasingMode.EaseInOut;
            }
            else
            {
                function.EasingMode = EasingMode.EaseOut;
            }

            return function;
        }

        private bool StringEquals(string left, string right)
        {
            return string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
        }

        private void UpdateClipGeometry(double width)
        {
            double safeWidth = Math.Max(0, width);
            double height = _clipBorder != null ? _clipBorder.ActualHeight : 0;
            if (height <= 0 && Window.Current != null)
            {
                height = Math.Max(0, Window.Current.Bounds.Height - Math.Max(0, TopOffset) - Math.Max(0, BottomOffset));
            }

            RectangleGeometry geometry = new RectangleGeometry();
            geometry.Rect = new Rect(0, 0, safeWidth, Math.Max(0, height));
            _clipBorder.Clip = geometry;
        }

        private void UpdateVisuals()
        {
            if (_paneBorder == null)
            {
                return;
            }

            Brush background = Background != null ? Background : ResolveThemeBrush("ApplicationPageBackgroundThemeBrush", new SolidColorBrush(Color.FromArgb(255, 17, 19, 16)));
            _paneBorder.Background = ApplyBrushOpacity(background, BackgroundOpacity);

            if (SideSeparatorEnabled && SideSeparatorThickness > 0)
            {
                _paneBorder.BorderBrush = SidebarBorderBrush != null ? SidebarBorderBrush : ResolveThemeBrush("SystemControlForegroundBaseLowBrush", new SolidColorBrush(Color.FromArgb(255, 51, 54, 52)));
                _paneBorder.BorderThickness = new Thickness(0, 0, Math.Max(0, SideSeparatorThickness), 0);
            }
            else
            {
                _paneBorder.BorderBrush = SidebarBorderBrush;
                _paneBorder.BorderThickness = new Thickness(0);
            }

            _paneBorder.Width = Math.Max(0, OpenPaneWidth);

            RebuildPanels();
        }

        private void RebuildPanels()
        {
            _topPinnedPanel.Children.Clear();
            _scrollPanel.Children.Clear();
            _bottomPinnedPanel.Children.Clear();

            int index = 0;

            if (TopItemsPinned)
            {
                AddItemsToPanel(_topPinnedPanel, TopItems, UniversalSidebarItemSection.Top, ref index);
                AddSeparator(_topPinnedPanel, TopItems.Count > 0 && (Items.Count > 0 || BottomItems.Count > 0));
            }
            else
            {
                AddItemsToPanel(_scrollPanel, TopItems, UniversalSidebarItemSection.Top, ref index);
                AddSeparator(_scrollPanel, TopItems.Count > 0 && (Items.Count > 0 || BottomItems.Count > 0));
            }

            AddItemsToPanel(_scrollPanel, Items, UniversalSidebarItemSection.Middle, ref index);

            if (BottomItemsPinned)
            {
                AddSeparator(_bottomPinnedPanel, BottomItems.Count > 0 && (TopItems.Count > 0 || Items.Count > 0));
                AddItemsToPanel(_bottomPinnedPanel, BottomItems, UniversalSidebarItemSection.Bottom, ref index);
            }
            else
            {
                AddSeparator(_scrollPanel, BottomItems.Count > 0 && (TopItems.Count > 0 || Items.Count > 0));
                AddItemsToPanel(_scrollPanel, BottomItems, UniversalSidebarItemSection.Bottom, ref index);
            }
        }

        private void AddItemsToPanel(StackPanel panel, ObservableCollection<UniversalSidebarItem> collection, UniversalSidebarItemSection section, ref int index)
        {
            if (panel == null || collection == null)
            {
                return;
            }

            for (int i = 0; i < collection.Count; i++)
            {
                UniversalSidebarItem item = collection[i];
                panel.Children.Add(CreateButton(item, section, index));
                index++;
            }
        }

        private void AddSeparator(StackPanel panel, bool visible)
        {
            if (!SeparatorEnabled || !visible || panel == null)
            {
                return;
            }

            Border border = new Border();
            border.Height = Math.Max(0, SeparatorThickness);
            border.Background = SeparatorBrush != null ? SeparatorBrush : ResolveThemeBrush("SystemControlForegroundBaseLowBrush", new SolidColorBrush(Color.FromArgb(255, 51, 54, 52)));
            border.Margin = IsCompactClosed() ? new Thickness(0, 8, 0, 8) : new Thickness(16, 8, 16, 8);
            panel.Children.Add(border);
        }

        private Button CreateButton(UniversalSidebarItem item, UniversalSidebarItemSection section, int index)
        {
            bool isEnabled = item == null || item.IsEnabled;
            bool isSelected = IsItemSelected(item, index);
            bool compactClosed = IsCompactClosed();
            Brush foreground = GetItemForeground(item, isSelected, isEnabled);

            Button button = new Button();
            button.Height = Math.Max(0, ItemHeight);
            button.MinWidth = 0;
            button.HorizontalAlignment = HorizontalAlignment.Stretch;
            button.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            button.VerticalContentAlignment = VerticalAlignment.Center;
            button.Padding = new Thickness(0);
            button.BorderThickness = new Thickness(0);
            button.Background = GetItemBackground(item, isSelected, isEnabled);
            button.Foreground = foreground;
            button.IsHitTestVisible = isEnabled;
            button.Opacity = isEnabled ? 1.0 : Clamp01(DisabledItemOpacity);
            button.Tag = new SidebarButtonTag(index, section, item);
            button.Click += OnButtonClick;
            AttachPressAnimation(button);

            Grid contentGrid = new Grid();
            contentGrid.Height = Math.Max(0, ItemHeight);
            contentGrid.HorizontalAlignment = HorizontalAlignment.Stretch;
            contentGrid.VerticalAlignment = VerticalAlignment.Center;
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(Math.Max(0, CompactPaneWidth)) });
            contentGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            FrameworkElement icon = CreateIcon(item, foreground);
            if (icon != null && ShowIcons)
            {
                icon.HorizontalAlignment = HorizontalAlignment.Center;
                icon.VerticalAlignment = VerticalAlignment.Center;
                icon.Margin = new Thickness(0);
                Grid.SetColumn(icon, 0);
                contentGrid.Children.Add(icon);
            }

            if (!compactClosed)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = item != null ? item.Text : string.Empty;
                textBlock.FontSize = LabelFontSize;
                textBlock.Foreground = foreground;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.TextTrimming = TextTrimming.CharacterEllipsis;
                textBlock.Margin = ShowIcons ? new Thickness(Math.Max(0, IconTextSpacing), 0, Math.Max(0, ItemPadding.Right), 0) : ItemPadding;
                Grid.SetColumn(textBlock, ShowIcons ? 1 : 0);
                if (!ShowIcons)
                {
                    Grid.SetColumnSpan(textBlock, 2);
                }
                contentGrid.Children.Add(textBlock);
            }

            button.Content = contentGrid;
            return button;
        }

        private FrameworkElement CreateIcon(UniversalSidebarItem item, Brush foreground)
        {
            if (item == null)
            {
                return null;
            }

            if (item.IconType == UniversalSidebarIconType.Image && !string.IsNullOrWhiteSpace(item.ImagePath))
            {
                Image image = new Image();
                image.Width = Math.Max(0, IconSize);
                image.Height = Math.Max(0, IconSize);
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
            icon.FontSize = Math.Max(0, IconSize);
            icon.Foreground = foreground;
            return icon;
        }

        private bool IsCompactClosed()
        {
            return _isHorizontalMode && !IsPaneOpen && ShowIconsWhenClosedInHorizontalMode;
        }

        private bool IsItemSelected(UniversalSidebarItem item, int index)
        {
            string selectedKey = SelectedKey;
            if (item != null && !string.IsNullOrWhiteSpace(selectedKey) && item.Key == selectedKey)
            {
                return true;
            }

            return index == SelectedIndex;
        }

        private Brush GetItemBackground(UniversalSidebarItem item, bool isSelected, bool isEnabled)
        {
            if (!isEnabled)
            {
                return FirstBrush(item != null ? item.DisabledBackground : null, DisabledItemBackground, new SolidColorBrush(Colors.Transparent));
            }

            if (isSelected)
            {
                return FirstBrush(item != null ? item.SelectedBackground : null, SelectedItemBackground, ResolveThemeBrush("SystemControlHighlightAccentBrush", new SolidColorBrush(Color.FromArgb(255, 0, 120, 215))));
            }

            return FirstBrush(item != null ? item.UnselectedBackground : null, UnselectedItemBackground, new SolidColorBrush(Colors.Transparent));
        }

        private Brush GetItemForeground(UniversalSidebarItem item, bool isSelected, bool isEnabled)
        {
            if (!isEnabled)
            {
                return FirstBrush(item != null ? item.DisabledForeground : null, DisabledItemForeground, new SolidColorBrush(Color.FromArgb(255, 128, 128, 128)));
            }

            if (isSelected)
            {
                return FirstBrush(item != null ? item.SelectedForeground : null, SelectedItemForeground, new SolidColorBrush(Colors.White));
            }

            return FirstBrush(item != null ? item.UnselectedForeground : null, UnselectedItemForeground, ResolveThemeBrush("SystemControlForegroundBaseHighBrush", new SolidColorBrush(Colors.White)));
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

        private Brush FirstBrush(Brush first, Brush second, Brush fallback)
        {
            if (first != null)
            {
                return first;
            }

            if (second != null)
            {
                return second;
            }

            return fallback;
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
            button.RenderTransformOrigin = new Point(0.5, 0.5);
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
                button.RenderTransformOrigin = new Point(0.5, 0.5);
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
            animation.EasingFunction = CreateEasingFunction();
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, propertyPath);
            return animation;
        }

        private void OnButtonClick(object sender, RoutedEventArgs args)
        {
            FrameworkElement element = sender as FrameworkElement;
            SidebarButtonTag tag = element != null ? element.Tag as SidebarButtonTag : null;
            if (tag == null || tag.Item == null || !tag.Item.IsEnabled)
            {
                return;
            }

            UniversalSidebarItemClickEventArgs clickArgs = new UniversalSidebarItemClickEventArgs(tag.Index, tag.Section, tag.Item, tag.Item.ClickAction);
            RaiseItemClick(clickArgs);
            if (clickArgs.Handled)
            {
                return;
            }

            if (SelectedIndex != tag.Index)
            {
                SelectedIndex = tag.Index;
            }

            if (!string.IsNullOrWhiteSpace(tag.Item.Key))
            {
                SelectedKey = tag.Item.Key;
            }

            UniversalActionInvoker.Invoke(this, ActionTarget, ActionTargetType, clickArgs.ClickAction, clickArgs, tag.Item, tag.Index);
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.OldItems != null)
            {
                foreach (object oldObject in args.OldItems)
                {
                    UniversalSidebarItem oldItem = oldObject as UniversalSidebarItem;
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
                    UniversalSidebarItem newItem = newObject as UniversalSidebarItem;
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

        private void RaiseItemClick(UniversalSidebarItemClickEventArgs args)
        {
            EventHandler<UniversalSidebarItemClickEventArgs> handler = ItemClick;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void RaiseSelectionChanged(int oldIndex, int newIndex)
        {
            if (oldIndex == newIndex)
            {
                return;
            }

            EventHandler<UniversalSidebarSelectionChangedEventArgs> handler = SelectionChanged;
            if (handler != null)
            {
                handler(this, new UniversalSidebarSelectionChangedEventArgs(oldIndex, newIndex, GetItemByIndex(oldIndex), GetItemByIndex(newIndex)));
            }
        }

        private void RaisePaneStateChanged()
        {
            EventHandler handler = PaneStateChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private UniversalSidebarItem GetItemByIndex(int index)
        {
            if (index < 0)
            {
                return null;
            }

            List<UniversalSidebarItem> allItems = new List<UniversalSidebarItem>();
            allItems.AddRange(TopItems);
            allItems.AddRange(Items);
            allItems.AddRange(BottomItems);

            if (index >= allItems.Count)
            {
                return null;
            }

            return allItems[index];
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

        private sealed class SidebarButtonTag
        {
            public SidebarButtonTag(int index, UniversalSidebarItemSection section, UniversalSidebarItem item)
            {
                Index = index;
                Section = section;
                Item = item;
            }

            public int Index { get; private set; }
            public UniversalSidebarItemSection Section { get; private set; }
            public UniversalSidebarItem Item { get; private set; }
        }
    }
}
