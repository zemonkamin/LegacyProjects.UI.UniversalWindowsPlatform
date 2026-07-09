using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using LegacyProjects.UI.UniversalWindowsPlatform.Enums;
using LegacyProjects.UI.UniversalWindowsPlatform.Events;
using LegacyProjects.UI.UniversalWindowsPlatform.Models;
using Windows.Foundation;
using Windows.Graphics.Effects;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Input;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Controls
{
    [ContentProperty(Name = "Items")]
    public sealed class UniversalTabBar : UserControl
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

        public static readonly DependencyProperty BackgroundOpacityProperty = DependencyProperty.Register(
            "BackgroundOpacity",
            typeof(double),
            typeof(UniversalTabBar),
            new PropertyMetadata(0.0, OnVisualPropertyChanged));

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

        public static readonly DependencyProperty PressEffectEnabledProperty = DependencyProperty.Register(
            "PressEffectEnabled",
            typeof(bool),
            typeof(UniversalTabBar),
            new PropertyMetadata(true, OnVisualPropertyChanged));

        public static readonly DependencyProperty GlassEffectEnabledProperty = DependencyProperty.Register(
            "GlassEffectEnabled",
            typeof(bool),
            typeof(UniversalTabBar),
            new PropertyMetadata(false, OnVisualPropertyChanged));

        public static readonly DependencyProperty GlassTintColorProperty = DependencyProperty.Register(
            "GlassTintColor",
            typeof(Color),
            typeof(UniversalTabBar),
            new PropertyMetadata(Colors.Transparent, OnVisualPropertyChanged));

        public static readonly DependencyProperty GlassTintAmountProperty = DependencyProperty.Register(
            "GlassTintAmount",
            typeof(double),
            typeof(UniversalTabBar),
            new PropertyMetadata(0.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty GlassBlurAmountProperty = DependencyProperty.Register(
            "GlassBlurAmount",
            typeof(double),
            typeof(UniversalTabBar),
            new PropertyMetadata(0.0, OnVisualPropertyChanged));

        public static readonly DependencyProperty GlassShaderPathProperty = DependencyProperty.Register(
            "GlassShaderPath",
            typeof(string),
            typeof(UniversalTabBar),
            new PropertyMetadata("Shaders/GlassEffect.fx", OnVisualPropertyChanged));

        public static readonly DependencyProperty DisabledItemOpacityProperty = DependencyProperty.Register(
            "DisabledItemOpacity",
            typeof(double),
            typeof(UniversalTabBar),
            new PropertyMetadata(0.45, OnVisualPropertyChanged));

        public static readonly DependencyProperty ActionTargetProperty = DependencyProperty.Register(
            "ActionTarget",
            typeof(object),
            typeof(UniversalTabBar),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ActionTargetTypeProperty = DependencyProperty.Register(
            "ActionTargetType",
            typeof(string),
            typeof(UniversalTabBar),
            new PropertyMetadata(string.Empty));

        private readonly Border _rootBorder;
        private readonly Grid _layoutRoot;
        private readonly Grid _glassEffectHost;
        private readonly Border _glassTintLayer;
        private readonly Grid _itemsGrid;
        private SpriteVisual _glassSpriteVisual;
        private bool _softwareGlassUpdateInProgress;
        private bool _softwareGlassUpdateRequested;
        private double _lastSoftwareGlassWidth;
        private double _lastSoftwareGlassHeight;
        private object _cachedActionTarget;
        private string _cachedActionTargetTypeName;

        public event EventHandler<UniversalTabBarSelectionChangedEventArgs> SelectionChanged;
        public event EventHandler<UniversalTabBarItemClickEventArgs> ItemClick;

        public UniversalTabBar()
        {
            Items = new ObservableCollection<UniversalTabBarItem>();
            Items.CollectionChanged += OnItemsCollectionChanged;

            _itemsGrid = new Grid();
            _glassEffectHost = new Grid();
            _glassTintLayer = new Border();
            _layoutRoot = new Grid();
            _layoutRoot.Children.Add(_glassEffectHost);
            _layoutRoot.Children.Add(_glassTintLayer);
            _layoutRoot.Children.Add(_itemsGrid);

            _rootBorder = new Border();
            _rootBorder.Child = _layoutRoot;
            Content = _rootBorder;

            Loaded += OnControlLoaded;
            SizeChanged += OnControlSizeChanged;
            RegisterPropertyChangedCallback(Control.BackgroundProperty, OnBackgroundPropertyChanged);

            ApplyDefaultBrushes();
            UpdateControl();
        }

        private void OnBackgroundPropertyChanged(DependencyObject sender, DependencyProperty property)
        {
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
            get { return GetItemByIndex(SelectedIndex); }
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

        public double BackgroundOpacity
        {
            get { return (double)GetValue(BackgroundOpacityProperty); }
            set { SetValue(BackgroundOpacityProperty, value); }
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

        public bool PressEffectEnabled
        {
            get { return (bool)GetValue(PressEffectEnabledProperty); }
            set { SetValue(PressEffectEnabledProperty, value); }
        }

        public bool GlassEffectEnabled
        {
            get { return (bool)GetValue(GlassEffectEnabledProperty); }
            set { SetValue(GlassEffectEnabledProperty, value); }
        }

        public Color GlassTintColor
        {
            get { return (Color)GetValue(GlassTintColorProperty); }
            set { SetValue(GlassTintColorProperty, value); }
        }

        public double GlassTintAmount
        {
            get { return (double)GetValue(GlassTintAmountProperty); }
            set { SetValue(GlassTintAmountProperty, value); }
        }

        public double GlassBlurAmount
        {
            get { return (double)GetValue(GlassBlurAmountProperty); }
            set { SetValue(GlassBlurAmountProperty, value); }
        }

        public string GlassShaderPath
        {
            get { return (string)GetValue(GlassShaderPathProperty); }
            set { SetValue(GlassShaderPathProperty, value); }
        }

        public double DisabledItemOpacity
        {
            get { return (double)GetValue(DisabledItemOpacityProperty); }
            set { SetValue(DisabledItemOpacityProperty, value); }
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
            return brush != null ? brush : fallbackBrush;
        }

        private void UpdateControl()
        {
            if (_rootBorder == null || _itemsGrid == null || _glassEffectHost == null || _glassTintLayer == null)
            {
                return;
            }

            UpdateBackgroundAndGlassEffect();
            UpdateLayoutProperties();
            UpdateSeparator();
            UpdateItems();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateControl();
        }

        private void OnControlLoaded(object sender, RoutedEventArgs args)
        {
            UpdateBackgroundAndGlassEffect();
        }

        private void OnControlSizeChanged(object sender, SizeChangedEventArgs args)
        {
            ResizeGlassVisual();

            if (GlassEffectEnabled && Clamp01(GlassBlurAmount) > 0)
            {
                if (!TryApplyCompositionGlass())
                {
                    ScheduleSoftwareGlassUpdate(false);
                }
            }
        }

        private void UpdateBackgroundAndGlassEffect()
        {
            if (!GlassEffectEnabled)
            {
                _rootBorder.Background = ApplyBrushOpacity(Background, BackgroundOpacity);
                _glassEffectHost.Visibility = Visibility.Collapsed;
                _glassTintLayer.Visibility = Visibility.Collapsed;
                ClearCompositionGlass();
                ClearSoftwareGlass();
                return;
            }

            _rootBorder.Background = new SolidColorBrush(Colors.Transparent);

            double tintAmount = Clamp01(GlassTintAmount);
            if (tintAmount > 0)
            {
                SolidColorBrush tintBrush = new SolidColorBrush(GlassTintColor);
                tintBrush.Opacity = tintAmount;
                _glassTintLayer.Background = tintBrush;
                _glassTintLayer.Visibility = Visibility.Visible;
            }
            else
            {
                _glassTintLayer.Background = null;
                _glassTintLayer.Visibility = Visibility.Collapsed;
            }

            if (Clamp01(GlassBlurAmount) > 0)
            {
                _glassEffectHost.Visibility = Visibility.Visible;
                if (!TryApplyCompositionGlass())
                {
                    ScheduleSoftwareGlassUpdate(false);
                }
            }
            else
            {
                _glassEffectHost.Visibility = Visibility.Collapsed;
                ClearCompositionGlass();
                ClearSoftwareGlass();
            }
        }

        public void RefreshGlassEffect()
        {
            if (!GlassEffectEnabled || Clamp01(GlassBlurAmount) <= 0)
            {
                return;
            }

            if (!TryApplyCompositionGlass())
            {
                ScheduleSoftwareGlassUpdate(true);
            }
        }

        private bool TryApplyCompositionGlass()
        {
            if (_glassEffectHost == null || _glassEffectHost.Visibility != Visibility.Visible)
            {
                return false;
            }

            if (_glassEffectHost.ActualWidth <= 0 || _glassEffectHost.ActualHeight <= 0)
            {
                return false;
            }

            try
            {
                Visual hostVisual = ElementCompositionPreview.GetElementVisual(_glassEffectHost);
                if (hostVisual == null || hostVisual.Compositor == null)
                {
                    ClearCompositionGlass();
                    return false;
                }

                Compositor compositor = hostVisual.Compositor;
                CompositionBrush backdropBrush = CreateBackdropBrush(compositor);
                if (backdropBrush == null)
                {
                    ClearCompositionGlass();
                    return false;
                }

                IGraphicsEffect blurEffect = CreateGaussianBlurEffect((float)(Clamp01(GlassBlurAmount) * 30.0));
                if (blurEffect == null)
                {
                    ClearCompositionGlass();
                    return false;
                }

                CompositionEffectFactory effectFactory = compositor.CreateEffectFactory(blurEffect);
                CompositionEffectBrush effectBrush = effectFactory.CreateBrush();
                effectBrush.SetSourceParameter("backdrop", backdropBrush);

                SpriteVisual spriteVisual = compositor.CreateSpriteVisual();
                spriteVisual.Brush = effectBrush;
                spriteVisual.Size = new Vector2((float)_glassEffectHost.ActualWidth, (float)_glassEffectHost.ActualHeight);

                ClearSoftwareGlass();
                ElementCompositionPreview.SetElementChildVisual(_glassEffectHost, spriteVisual);
                _glassSpriteVisual = spriteVisual;
                return true;
            }
            catch
            {
                ClearCompositionGlass();
                return false;
            }
        }

        private CompositionBrush CreateBackdropBrush(Compositor compositor)
        {
            if (compositor == null)
            {
                return null;
            }

            try
            {
                MethodInfo method = compositor.GetType().GetRuntimeMethod("CreateBackdropBrush", new Type[0]);
                if (method == null)
                {
                    return null;
                }

                object brush = method.Invoke(compositor, null);
                return brush as CompositionBrush;
            }
            catch
            {
                return null;
            }
        }

        private IGraphicsEffect CreateGaussianBlurEffect(float blurAmount)
        {
            Type effectType = ResolveType(new string[]
            {
                "Microsoft.Graphics.Canvas.Effects.GaussianBlurEffect, Microsoft.Graphics.Canvas",
                "Microsoft.Graphics.Canvas.Effects.GaussianBlurEffect, Microsoft.Graphics.Canvas.Interop"
            });

            if (effectType == null)
            {
                return null;
            }

            try
            {
                object effect = Activator.CreateInstance(effectType);
                SetProperty(effect, "Name", "UniversalTabBarGlassBlur");
                SetProperty(effect, "BlurAmount", blurAmount);
                SetProperty(effect, "Source", new CompositionEffectSourceParameter("backdrop"));
                SetEnumProperty(effect, "BorderMode", "Hard");
                SetEnumProperty(effect, "Optimization", "Balanced");
                return effect as IGraphicsEffect;
            }
            catch
            {
                return null;
            }
        }

        private Type ResolveType(string[] typeNames)
        {
            if (typeNames == null)
            {
                return null;
            }

            for (int i = 0; i < typeNames.Length; i++)
            {
                Type type = Type.GetType(typeNames[i]);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }

        private void SetProperty(object target, string propertyName, object value)
        {
            if (target == null || string.IsNullOrEmpty(propertyName))
            {
                return;
            }

            PropertyInfo property = target.GetType().GetRuntimeProperty(propertyName);
            if (property == null || !property.CanWrite)
            {
                return;
            }

            try
            {
                property.SetValue(target, value);
            }
            catch
            {
            }
        }

        private void SetEnumProperty(object target, string propertyName, string enumValueName)
        {
            if (target == null || string.IsNullOrEmpty(propertyName) || string.IsNullOrEmpty(enumValueName))
            {
                return;
            }

            PropertyInfo property = target.GetType().GetRuntimeProperty(propertyName);
            if (property == null || !property.CanWrite || property.PropertyType == null || !property.PropertyType.GetTypeInfo().IsEnum)
            {
                return;
            }

            try
            {
                object enumValue = Enum.Parse(property.PropertyType, enumValueName);
                property.SetValue(target, enumValue);
            }
            catch
            {
            }
        }

        private void ResizeGlassVisual()
        {
            if (_glassSpriteVisual == null || _glassEffectHost == null)
            {
                return;
            }

            _glassSpriteVisual.Size = new Vector2((float)_glassEffectHost.ActualWidth, (float)_glassEffectHost.ActualHeight);
        }

        private void ClearCompositionGlass()
        {
            if (_glassEffectHost == null)
            {
                return;
            }

            try
            {
                ElementCompositionPreview.SetElementChildVisual(_glassEffectHost, null);
            }
            catch
            {
            }

            _glassSpriteVisual = null;
        }
        private void ScheduleSoftwareGlassUpdate(bool force)
        {
            if (!GlassEffectEnabled || Clamp01(GlassBlurAmount) <= 0)
            {
                return;
            }

            if (!force && !ShouldUpdateSoftwareGlass())
            {
                return;
            }

            if (_softwareGlassUpdateInProgress)
            {
                _softwareGlassUpdateRequested = true;
                return;
            }

            Task updateTask = ApplySoftwareGlassAsync();
        }

        private async Task ApplySoftwareGlassAsync()
        {
            if (_glassEffectHost == null || _glassEffectHost.Visibility != Visibility.Visible)
            {
                return;
            }

            _softwareGlassUpdateInProgress = true;

            try
            {
                await Task.Delay(50);

                if (!GlassEffectEnabled || Clamp01(GlassBlurAmount) <= 0)
                {
                    ClearSoftwareGlass();
                    return;
                }

                FrameworkElement rootElement = null;
                if (Window.Current != null)
                {
                    rootElement = Window.Current.Content as FrameworkElement;
                }

                if (rootElement == null || rootElement.ActualWidth <= 0 || rootElement.ActualHeight <= 0 || ActualWidth <= 0 || ActualHeight <= 0)
                {
                    return;
                }

                Point position = TransformToVisual(rootElement).TransformPoint(new Point(0, 0));
                RenderTargetBitmap renderTarget = new RenderTargetBitmap();

                double previousOpacity = Opacity;
                bool previousHitTest = IsHitTestVisible;

                try
                {
                    Opacity = 0;
                    IsHitTestVisible = false;
                    await renderTarget.RenderAsync(rootElement);
                }
                finally
                {
                    Opacity = previousOpacity;
                    IsHitTestVisible = previousHitTest;
                }

                if (renderTarget.PixelWidth <= 0 || renderTarget.PixelHeight <= 0)
                {
                    return;
                }

                byte[] sourcePixels = (await renderTarget.GetPixelsAsync()).ToArray();
                if (sourcePixels == null || sourcePixels.Length == 0)
                {
                    return;
                }

                double scaleX = rootElement.ActualWidth > 0 ? renderTarget.PixelWidth / rootElement.ActualWidth : 1.0;
                double scaleY = rootElement.ActualHeight > 0 ? renderTarget.PixelHeight / rootElement.ActualHeight : 1.0;

                int cropX = (int)Math.Round(position.X * scaleX);
                int cropY = (int)Math.Round(position.Y * scaleY);
                int cropWidth = (int)Math.Round(ActualWidth * scaleX);
                int cropHeight = (int)Math.Round(ActualHeight * scaleY);

                NormalizeCrop(renderTarget.PixelWidth, renderTarget.PixelHeight, ref cropX, ref cropY, ref cropWidth, ref cropHeight);

                if (cropWidth <= 0 || cropHeight <= 0)
                {
                    return;
                }

                byte[] croppedPixels = CropBgraPixels(sourcePixels, renderTarget.PixelWidth, renderTarget.PixelHeight, cropX, cropY, cropWidth, cropHeight);
                int blurRadius = Math.Max(1, (int)Math.Round(Clamp01(GlassBlurAmount) * 18.0));
                byte[] blurredPixels = BoxBlurBgra(croppedPixels, cropWidth, cropHeight, blurRadius);

                WriteableBitmap bitmap = new WriteableBitmap(cropWidth, cropHeight);
                using (Stream stream = bitmap.PixelBuffer.AsStream())
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.Write(blurredPixels, 0, blurredPixels.Length);
                }

                bitmap.Invalidate();

                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = bitmap;
                imageBrush.Stretch = Stretch.Fill;
                _glassEffectHost.Background = imageBrush;
                _lastSoftwareGlassWidth = ActualWidth;
                _lastSoftwareGlassHeight = ActualHeight;
            }
            catch
            {
                ClearSoftwareGlass();
            }
            finally
            {
                _softwareGlassUpdateInProgress = false;

                if (_softwareGlassUpdateRequested)
                {
                    _softwareGlassUpdateRequested = false;
                    ScheduleSoftwareGlassUpdate(false);
                }
            }
        }

        private bool ShouldUpdateSoftwareGlass()
        {
            if (_glassEffectHost == null || ActualWidth <= 0 || ActualHeight <= 0)
            {
                return false;
            }

            if (_glassEffectHost.Background == null)
            {
                return true;
            }

            return Math.Abs(ActualWidth - _lastSoftwareGlassWidth) > 1.0 ||
                   Math.Abs(ActualHeight - _lastSoftwareGlassHeight) > 1.0;
        }

        private void NormalizeCrop(int sourceWidth, int sourceHeight, ref int x, ref int y, ref int width, ref int height)
        {
            if (x < 0)
            {
                width += x;
                x = 0;
            }

            if (y < 0)
            {
                height += y;
                y = 0;
            }

            if (x + width > sourceWidth)
            {
                width = sourceWidth - x;
            }

            if (y + height > sourceHeight)
            {
                height = sourceHeight - y;
            }
        }

        private byte[] CropBgraPixels(byte[] sourcePixels, int sourceWidth, int sourceHeight, int x, int y, int width, int height)
        {
            byte[] result = new byte[width * height * 4];
            int rowLength = width * 4;

            for (int row = 0; row < height; row++)
            {
                int sourceIndex = ((y + row) * sourceWidth + x) * 4;
                int targetIndex = row * rowLength;
                Buffer.BlockCopy(sourcePixels, sourceIndex, result, targetIndex, rowLength);
            }

            return result;
        }

        private byte[] BoxBlurBgra(byte[] sourcePixels, int width, int height, int radius)
        {
            if (sourcePixels == null || sourcePixels.Length == 0 || width <= 0 || height <= 0 || radius <= 0)
            {
                return sourcePixels;
            }

            byte[] horizontalPixels = new byte[sourcePixels.Length];
            byte[] resultPixels = new byte[sourcePixels.Length];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int sumB = 0;
                    int sumG = 0;
                    int sumR = 0;
                    int sumA = 0;
                    int count = 0;

                    int minX = Math.Max(0, x - radius);
                    int maxX = Math.Min(width - 1, x + radius);

                    for (int sampleX = minX; sampleX <= maxX; sampleX++)
                    {
                        int sampleIndex = (y * width + sampleX) * 4;
                        sumB += sourcePixels[sampleIndex];
                        sumG += sourcePixels[sampleIndex + 1];
                        sumR += sourcePixels[sampleIndex + 2];
                        sumA += sourcePixels[sampleIndex + 3];
                        count++;
                    }

                    int targetIndex = (y * width + x) * 4;
                    horizontalPixels[targetIndex] = (byte)(sumB / count);
                    horizontalPixels[targetIndex + 1] = (byte)(sumG / count);
                    horizontalPixels[targetIndex + 2] = (byte)(sumR / count);
                    horizontalPixels[targetIndex + 3] = (byte)(sumA / count);
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int sumB = 0;
                    int sumG = 0;
                    int sumR = 0;
                    int sumA = 0;
                    int count = 0;

                    int minY = Math.Max(0, y - radius);
                    int maxY = Math.Min(height - 1, y + radius);

                    for (int sampleY = minY; sampleY <= maxY; sampleY++)
                    {
                        int sampleIndex = (sampleY * width + x) * 4;
                        sumB += horizontalPixels[sampleIndex];
                        sumG += horizontalPixels[sampleIndex + 1];
                        sumR += horizontalPixels[sampleIndex + 2];
                        sumA += horizontalPixels[sampleIndex + 3];
                        count++;
                    }

                    int targetIndex = (y * width + x) * 4;
                    resultPixels[targetIndex] = (byte)(sumB / count);
                    resultPixels[targetIndex + 1] = (byte)(sumG / count);
                    resultPixels[targetIndex + 2] = (byte)(sumR / count);
                    resultPixels[targetIndex + 3] = (byte)(sumA / count);
                }
            }

            return resultPixels;
        }

        private void ClearSoftwareGlass()
        {
            if (_glassEffectHost != null)
            {
                _glassEffectHost.Background = null;
            }

            _lastSoftwareGlassWidth = 0;
            _lastSoftwareGlassHeight = 0;
        }


        private double Clamp01(double value)
        {
            if (double.IsNaN(value) || value < 0)
            {
                return 0;
            }

            if (value > 1)
            {
                return 1;
            }

            return value;
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
                _rootBorder.BorderThickness = new Thickness(0);
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

            _rootBorder.BorderBrush = SeparatorBrush;
            _rootBorder.BorderThickness = new Thickness(left, top, right, bottom);
        }

        private void UpdateItems()
        {
            if (_itemsGrid == null)
            {
                return;
            }

            _itemsGrid.Children.Clear();
            _itemsGrid.RowDefinitions.Clear();
            _itemsGrid.ColumnDefinitions.Clear();

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
                FrameworkElement button = CreateButton(item, i);

                if (Orientation == Windows.UI.Xaml.Controls.Orientation.Horizontal)
                {
                    GridLength width = HorizontalPlacement == UniversalTabBarHorizontalPlacement.Stretch
                        ? new GridLength(1, GridUnitType.Star)
                        : GridLength.Auto;

                    _itemsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = width });
                    Grid.SetColumn(button, i);
                }
                else
                {
                    _itemsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    Grid.SetRow(button, i);
                }

                _itemsGrid.Children.Add(button);
            }
        }

        private FrameworkElement CreateButton(UniversalTabBarItem item, int index)
        {
            bool selected = index == SelectedIndex;
            bool enabled = IsItemEnabled(item);
            double disabledOpacity = Clamp01(DisabledItemOpacity);
            Brush buttonForeground = selected ? SelectedTabForeground : UnselectedTabForeground;
            Brush iconForeground = GetItemIconForeground(item, selected);
            Brush textForeground = GetItemTextForeground(item, selected);

            StackPanel contentPanel = new StackPanel();
            contentPanel.Orientation = Windows.UI.Xaml.Controls.Orientation.Vertical;
            contentPanel.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            contentPanel.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            contentPanel.Opacity = enabled ? 1.0 : disabledOpacity;

            FrameworkElement icon = CreateIcon(item, iconForeground);
            TextBlock text = CreateText(item, textForeground);

            if (TextPosition == UniversalTabBarTextPosition.AboveIcon)
            {
                AddTextAndIcon(contentPanel, text, icon);
            }
            else
            {
                AddIconAndText(contentPanel, icon, text);
            }

            if (IsItemPressEffectEnabled(item))
            {
                Button button = new Button();
                button.Tag = index;
                button.IsEnabled = enabled;
                button.Padding = ButtonPadding;
                button.MinWidth = ItemMinWidth;
                button.MinHeight = BarHeight > 0 ? BarHeight : 0;
                button.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
                button.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
                button.HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
                button.VerticalContentAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
                button.Foreground = buttonForeground;
                button.Background = new SolidColorBrush(Colors.Transparent);
                button.BorderThickness = new Thickness(0);
                button.Margin = GetItemMargin();
                button.Click += OnButtonClick;
                button.Content = contentPanel;
                return button;
            }

            Grid paddingGrid = new Grid();
            paddingGrid.Margin = ButtonPadding;
            paddingGrid.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center;
            paddingGrid.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Center;
            paddingGrid.Children.Add(contentPanel);

            Border buttonWithoutEffect = new Border();
            buttonWithoutEffect.Tag = index;
            buttonWithoutEffect.MinWidth = ItemMinWidth;
            buttonWithoutEffect.MinHeight = BarHeight > 0 ? BarHeight : 0;
            buttonWithoutEffect.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            buttonWithoutEffect.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
            buttonWithoutEffect.Background = new SolidColorBrush(Colors.Transparent);
            buttonWithoutEffect.Margin = GetItemMargin();
            buttonWithoutEffect.Child = paddingGrid;

            if (enabled)
            {
                buttonWithoutEffect.IsTapEnabled = true;
                buttonWithoutEffect.Tapped += OnButtonTapped;
            }
            else
            {
                buttonWithoutEffect.IsHitTestVisible = false;
            }

            return buttonWithoutEffect;
        }

        private bool IsItemEnabled(UniversalTabBarItem item)
        {
            return item == null || item.IsEnabled;
        }

        private bool IsItemPressEffectEnabled(UniversalTabBarItem item)
        {
            if (item != null)
            {
                if (item.PressEffectMode == UniversalTabBarPressEffectMode.Enabled)
                {
                    return true;
                }

                if (item.PressEffectMode == UniversalTabBarPressEffectMode.Disabled)
                {
                    return false;
                }
            }

            return PressEffectEnabled;
        }

        private Brush GetItemIconForeground(UniversalTabBarItem item, bool selected)
        {
            if (item != null)
            {
                if (selected && item.SelectedIconForeground != null)
                {
                    return item.SelectedIconForeground;
                }

                if (!selected && item.UnselectedIconForeground != null)
                {
                    return item.UnselectedIconForeground;
                }

                if (item.IconForeground != null)
                {
                    return item.IconForeground;
                }
            }

            return selected ? SelectedTabForeground : UnselectedTabForeground;
        }

        private Brush GetItemTextForeground(UniversalTabBarItem item, bool selected)
        {
            if (item != null)
            {
                if (selected && item.SelectedTextForeground != null)
                {
                    return item.SelectedTextForeground;
                }

                if (!selected && item.UnselectedTextForeground != null)
                {
                    return item.UnselectedTextForeground;
                }

                if (item.TextForeground != null)
                {
                    return item.TextForeground;
                }
            }

            return selected ? SelectedTabForeground : UnselectedTabForeground;
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

        private void AddIconAndText(StackPanel contentPanel, FrameworkElement icon, TextBlock text)
        {
            if (ShowIcons && icon != null)
            {
                contentPanel.Children.Add(icon);
            }

            if (ShowText && text != null)
            {
                contentPanel.Children.Add(text);
            }
        }

        private void AddTextAndIcon(StackPanel contentPanel, TextBlock text, FrameworkElement icon)
        {
            if (ShowText && text != null)
            {
                contentPanel.Children.Add(text);
            }

            if (ShowIcons && icon != null)
            {
                contentPanel.Children.Add(icon);
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
            SelectItemFromSender(sender);
        }

        private void OnButtonTapped(object sender, TappedRoutedEventArgs args)
        {
            SelectItemFromSender(sender);
            args.Handled = true;
        }

        private void SelectItemFromSender(object sender)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null || element.Tag == null)
            {
                return;
            }

            int index = (int)element.Tag;
            UniversalTabBarItem item = GetItemByIndex(index);
            if (!IsItemEnabled(item))
            {
                return;
            }

            UniversalTabBarItemClickEventArgs clickArgs = new UniversalTabBarItemClickEventArgs(index, item, item != null ? item.ClickAction : string.Empty);
            RaiseItemClick(clickArgs);
            if (clickArgs.Handled)
            {
                return;
            }

            if (index != SelectedIndex)
            {
                SelectedIndex = index;
            }

            InvokeClickAction(clickArgs);
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
                selectedIndex = Items.Count - 1;
            }

            if (IsItemEnabled(GetItemByIndex(selectedIndex)))
            {
                return selectedIndex;
            }

            for (int i = selectedIndex + 1; i < Items.Count; i++)
            {
                if (IsItemEnabled(GetItemByIndex(i)))
                {
                    return i;
                }
            }

            for (int i = selectedIndex - 1; i >= 0; i--)
            {
                if (IsItemEnabled(GetItemByIndex(i)))
                {
                    return i;
                }
            }

            return -1;
        }

        private UniversalTabBarItem GetItemByIndex(int index)
        {
            if (Items == null || index < 0 || index >= Items.Count)
            {
                return null;
            }

            return Items[index];
        }


        private void RaiseItemClick(UniversalTabBarItemClickEventArgs args)
        {
            EventHandler<UniversalTabBarItemClickEventArgs> handler = ItemClick;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void InvokeClickAction(UniversalTabBarItemClickEventArgs clickArgs)
        {
            if (clickArgs == null || string.IsNullOrWhiteSpace(clickArgs.ClickAction))
            {
                return;
            }

            try
            {
                Type targetType = null;
                object target = ActionTarget;

                if (target != null)
                {
                    targetType = target.GetType();
                }
                else
                {
                    targetType = ResolveActionTargetType();
                }

                if (targetType == null)
                {
                    return;
                }

                MethodInfo method = FindActionMethod(targetType, clickArgs.ClickAction, clickArgs);
                if (method == null)
                {
                    return;
                }

                if (!method.IsStatic && target == null)
                {
                    target = GetActionTargetInstance(targetType);
                }

                if (!method.IsStatic && target == null)
                {
                    return;
                }

                object[] parameters;
                if (!TryBuildActionParameters(method, clickArgs, out parameters))
                {
                    return;
                }

                method.Invoke(method.IsStatic ? null : target, parameters);
            }
            catch
            {
            }
        }

        private Type ResolveActionTargetType()
        {
            string typeName = ActionTargetType;
            if (string.IsNullOrWhiteSpace(typeName))
            {
                return null;
            }

            Type type = Type.GetType(typeName);
            if (type != null)
            {
                return type;
            }

            if (Application.Current != null)
            {
                Assembly appAssembly = Application.Current.GetType().GetTypeInfo().Assembly;
                if (appAssembly != null)
                {
                    type = appAssembly.GetType(typeName);
                    if (type != null)
                    {
                        return type;
                    }
                }
            }

            return null;
        }

        private object GetActionTargetInstance(Type targetType)
        {
            if (targetType == null)
            {
                return null;
            }

            string typeName = targetType.FullName;
            if (_cachedActionTarget != null && _cachedActionTargetTypeName == typeName)
            {
                return _cachedActionTarget;
            }

            try
            {
                _cachedActionTarget = Activator.CreateInstance(targetType);
                _cachedActionTargetTypeName = typeName;
                return _cachedActionTarget;
            }
            catch
            {
                return null;
            }
        }

        private MethodInfo FindActionMethod(Type targetType, string actionName, UniversalTabBarItemClickEventArgs clickArgs)
        {
            if (targetType == null || string.IsNullOrWhiteSpace(actionName))
            {
                return null;
            }

            MethodInfo fallbackMethod = null;

            foreach (MethodInfo method in targetType.GetRuntimeMethods())
            {
                if (method == null || method.Name != actionName)
                {
                    continue;
                }

                object[] parameters;
                if (TryBuildActionParameters(method, clickArgs, out parameters))
                {
                    return method;
                }

                if (fallbackMethod == null)
                {
                    fallbackMethod = method;
                }
            }

            return fallbackMethod;
        }

        private bool TryBuildActionParameters(MethodInfo method, UniversalTabBarItemClickEventArgs clickArgs, out object[] values)
        {
            values = null;

            if (method == null)
            {
                return false;
            }

            ParameterInfo[] parameters = method.GetParameters();
            if (parameters == null || parameters.Length == 0)
            {
                values = new object[0];
                return true;
            }

            if (parameters.Length == 1)
            {
                object value;
                if (TryGetActionParameterValue(parameters[0].ParameterType, clickArgs, false, out value))
                {
                    values = new object[] { value };
                    return true;
                }
            }

            if (parameters.Length == 2)
            {
                object senderValue;
                object argsValue;
                if (TryGetActionParameterValue(parameters[0].ParameterType, clickArgs, true, out senderValue) &&
                    TryGetActionParameterValue(parameters[1].ParameterType, clickArgs, false, out argsValue))
                {
                    values = new object[] { senderValue, argsValue };
                    return true;
                }
            }

            return false;
        }

        private bool TryGetActionParameterValue(Type parameterType, UniversalTabBarItemClickEventArgs clickArgs, bool senderParameter, out object value)
        {
            value = null;

            if (parameterType == null)
            {
                return false;
            }

            if (senderParameter)
            {
                value = this;
                return IsTypeAssignable(parameterType, value);
            }

            if (clickArgs != null && IsTypeAssignable(parameterType, clickArgs))
            {
                value = clickArgs;
                return true;
            }

            if (clickArgs != null && IsTypeAssignable(parameterType, clickArgs.Item))
            {
                value = clickArgs.Item;
                return true;
            }

            if (parameterType == typeof(int))
            {
                value = clickArgs != null ? clickArgs.Index : -1;
                return true;
            }

            if (parameterType == typeof(string))
            {
                value = clickArgs != null ? clickArgs.ClickAction : string.Empty;
                return true;
            }

            if (parameterType == typeof(object))
            {
                value = clickArgs;
                return true;
            }

            return false;
        }

        private bool IsTypeAssignable(Type targetType, object value)
        {
            if (targetType == null || value == null)
            {
                return false;
            }

            return targetType.GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo());
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
