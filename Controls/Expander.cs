using System;
using LegacyProjects.UI.UniversalWindowsPlatform.Enums;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Controls
{
    /// <summary>
    /// A header with a collapsible content region, similar to the WinUI 2.6 Expander (which does not
    /// exist in the Windows 10 SDK 14393). Put the collapsible content directly inside the element:
    /// <code>&lt;ui:Expander Header="Advanced"&gt;&lt;TextBlock Text="..."/&gt;&lt;/ui:Expander&gt;</code>
    /// </summary>
    [ContentProperty(Name = "ExpanderContent")]
    public sealed class Expander : UserControl
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(object), typeof(Expander), new PropertyMetadata(null, OnHeaderChanged));

        public static readonly DependencyProperty ExpanderContentProperty = DependencyProperty.Register(
            "ExpanderContent", typeof(object), typeof(Expander), new PropertyMetadata(null, OnContentChanged));

        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register(
            "IsExpanded", typeof(bool), typeof(Expander), new PropertyMetadata(false, OnIsExpandedChanged));

        public static readonly DependencyProperty ExpandDirectionProperty = DependencyProperty.Register(
            "ExpandDirection", typeof(ExpandDirection), typeof(Expander), new PropertyMetadata(ExpandDirection.Down, OnExpandDirectionChanged));

        public static readonly DependencyProperty HeaderIconGlyphProperty = DependencyProperty.Register(
            "HeaderIconGlyph", typeof(string), typeof(Expander), new PropertyMetadata(null, OnHeaderChanged));

        public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.Register(
            "HeaderBackground", typeof(Brush), typeof(Expander), new PropertyMetadata(null, OnVisualChanged));

        public static readonly DependencyProperty ContentBackgroundProperty = DependencyProperty.Register(
            "ContentBackground", typeof(Brush), typeof(Expander), new PropertyMetadata(null, OnVisualChanged));

        public static readonly DependencyProperty CornerRadiusValueProperty = DependencyProperty.Register(
            "CornerRadiusValue", typeof(CornerRadius), typeof(Expander), new PropertyMetadata(new CornerRadius(0), OnVisualChanged));

        private readonly Grid _root;
        private readonly Border _headerBorder;
        private readonly Grid _headerGrid;
        private readonly FontIcon _headerIcon;
        private readonly ContentPresenter _headerPresenter;
        private readonly FontIcon _chevron;
        private readonly RotateTransform _chevronTransform;
        private readonly Border _contentBorder;
        private readonly ContentPresenter _contentPresenter;

        /// <summary>Raised when the control expands.</summary>
        public event EventHandler Expanded;

        /// <summary>Raised when the control collapses.</summary>
        public event EventHandler Collapsed;

        public Expander()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;

            // Match SettingsCard's icon metrics (20px + 16 right margin) so header text lines up
            // across both controls when they are stacked in a settings list.
            _headerIcon = new FontIcon();
            _headerIcon.FontFamily = new FontFamily("Segoe MDL2 Assets");
            _headerIcon.FontSize = 20;
            _headerIcon.VerticalAlignment = VerticalAlignment.Center;
            _headerIcon.Margin = new Thickness(0, 0, 16, 0);
            _headerIcon.Visibility = Visibility.Collapsed;

            _headerPresenter = new ContentPresenter();
            _headerPresenter.VerticalAlignment = VerticalAlignment.Center;
            _headerPresenter.HorizontalAlignment = HorizontalAlignment.Left;

            // Rotation is centered via RenderTransformOrigin (0.5, 0.5); do NOT also set CenterX/CenterY
            // or the two offsets combine and the chevron orbits an off-center point.
            _chevronTransform = new RotateTransform();

            _chevron = new FontIcon();
            _chevron.FontFamily = new FontFamily("Segoe MDL2 Assets");
            _chevron.Glyph = ""; // ChevronDown
            _chevron.FontSize = 12;
            _chevron.VerticalAlignment = VerticalAlignment.Center;
            _chevron.HorizontalAlignment = HorizontalAlignment.Right;
            _chevron.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
            _chevron.RenderTransform = _chevronTransform;

            _headerGrid = new Grid();
            _headerGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            _headerGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            _headerGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            Grid.SetColumn(_headerIcon, 0);
            Grid.SetColumn(_headerPresenter, 1);
            Grid.SetColumn(_chevron, 2);
            _headerGrid.Children.Add(_headerIcon);
            _headerGrid.Children.Add(_headerPresenter);
            _headerGrid.Children.Add(_chevron);

            _headerBorder = new Border();
            _headerBorder.Padding = new Thickness(16, 12, 16, 12);
            _headerBorder.Child = _headerGrid;
            _headerBorder.Tapped += OnHeaderTapped;

            _contentPresenter = new ContentPresenter();
            _contentPresenter.HorizontalContentAlignment = HorizontalAlignment.Stretch;

            _contentBorder = new Border();
            _contentBorder.Padding = new Thickness(16, 12, 16, 12);
            _contentBorder.Child = _contentPresenter;
            _contentBorder.Visibility = Visibility.Collapsed;

            _root = new Grid();
            _root.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            _root.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            _root.Children.Add(_headerBorder);
            _root.Children.Add(_contentBorder);

            Content = _root;

            Loaded += OnLoaded;
        }

        /// <summary>Header content. Accepts a string or any UIElement.</summary>
        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>The collapsible content. This is the XAML content property.</summary>
        public object ExpanderContent
        {
            get { return GetValue(ExpanderContentProperty); }
            set { SetValue(ExpanderContentProperty, value); }
        }

        /// <summary>Whether the content region is currently shown.</summary>
        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        /// <summary>Whether content opens below (Down) or above (Up) the header.</summary>
        public ExpandDirection ExpandDirection
        {
            get { return (ExpandDirection)GetValue(ExpandDirectionProperty); }
            set { SetValue(ExpandDirectionProperty, value); }
        }

        /// <summary>Optional Segoe MDL2 Assets glyph shown at the start of the header.</summary>
        public string HeaderIconGlyph
        {
            get { return (string)GetValue(HeaderIconGlyphProperty); }
            set { SetValue(HeaderIconGlyphProperty, value); }
        }

        /// <summary>Background brush of the header row.</summary>
        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }

        /// <summary>Background brush of the content region.</summary>
        public Brush ContentBackground
        {
            get { return (Brush)GetValue(ContentBackgroundProperty); }
            set { SetValue(ContentBackgroundProperty, value); }
        }

        /// <summary>Corner radius applied to the outer frame.</summary>
        public CornerRadius CornerRadiusValue
        {
            get { return (CornerRadius)GetValue(CornerRadiusValueProperty); }
            set { SetValue(CornerRadiusValueProperty, value); }
        }

        private static void OnHeaderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            Expander expander = sender as Expander;
            if (expander != null)
            {
                expander.UpdateHeader();
            }
        }

        private static void OnContentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            Expander expander = sender as Expander;
            if (expander != null)
            {
                expander._contentPresenter.Content = expander.ExpanderContent;
            }
        }

        private static void OnIsExpandedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            Expander expander = sender as Expander;
            if (expander != null)
            {
                expander.UpdateExpansion(true);
            }
        }

        private static void OnExpandDirectionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            Expander expander = sender as Expander;
            if (expander != null)
            {
                expander.ApplyDirection();
                expander.UpdateExpansion(false);
            }
        }

        private static void OnVisualChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            Expander expander = sender as Expander;
            if (expander != null)
            {
                expander.ApplyVisuals();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            UpdateHeader();
            _contentPresenter.Content = ExpanderContent;
            ApplyDirection();
            ApplyVisuals();
            UpdateExpansion(false);
        }

        private void OnHeaderTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs args)
        {
            IsExpanded = !IsExpanded;
        }

        private void UpdateHeader()
        {
            if (!string.IsNullOrEmpty(HeaderIconGlyph))
            {
                _headerIcon.Glyph = HeaderIconGlyph;
                _headerIcon.Visibility = Visibility.Visible;
            }
            else
            {
                _headerIcon.Visibility = Visibility.Collapsed;
            }

            object header = Header;
            string headerText = header as string;
            if (headerText != null)
            {
                TextBlock block = new TextBlock();
                block.Text = headerText;
                block.FontSize = 15;
                block.VerticalAlignment = VerticalAlignment.Center;
                _headerPresenter.Content = block;
            }
            else
            {
                _headerPresenter.Content = header;
            }
        }

        private void ApplyDirection()
        {
            _root.Children.Clear();
            if (ExpandDirection == ExpandDirection.Up)
            {
                Grid.SetRow(_contentBorder, 0);
                Grid.SetRow(_headerBorder, 1);
            }
            else
            {
                Grid.SetRow(_headerBorder, 0);
                Grid.SetRow(_contentBorder, 1);
            }

            _root.Children.Add(_headerBorder);
            _root.Children.Add(_contentBorder);
        }

        private void ApplyVisuals()
        {
            // Theme-neutral translucent fills so the control recolors correctly when the element theme
            // changes at runtime; text inside inherits the element theme foreground automatically.
            Brush headerBg = HeaderBackground != null ? HeaderBackground : new SolidColorBrush(Color.FromArgb(18, 128, 128, 128));
            Brush contentBg = ContentBackground != null ? ContentBackground : new SolidColorBrush(Color.FromArgb(10, 128, 128, 128));
            Brush border = new SolidColorBrush(Color.FromArgb(38, 128, 128, 128));

            _headerBorder.Background = headerBg;
            _contentBorder.Background = contentBg;
            _headerBorder.BorderBrush = border;
            _contentBorder.BorderBrush = border;
            _headerBorder.BorderThickness = new Thickness(1);
            _contentBorder.BorderThickness = new Thickness(1, 0, 1, 1);
            _headerBorder.CornerRadius = CornerRadiusValue;
        }

        private void UpdateExpansion(bool animate)
        {
            bool expanded = IsExpanded;
            double chevronTarget = expanded ? 180 : 0;

            if (!animate)
            {
                _chevronTransform.Angle = chevronTarget;
                _contentBorder.Visibility = expanded ? Visibility.Visible : Visibility.Collapsed;
                _contentBorder.Height = double.NaN;
                _contentBorder.Opacity = 1;
                RaiseExpandedOrCollapsed(expanded);
                return;
            }

            AnimateChevron(chevronTarget);

            if (expanded)
            {
                _contentBorder.Visibility = Visibility.Visible;
                _contentBorder.Height = double.NaN;
                _contentBorder.Opacity = 0;

                double availableWidth = ActualWidth > 0 ? ActualWidth : double.PositiveInfinity;
                _contentBorder.Measure(new Size(availableWidth, double.PositiveInfinity));
                double targetHeight = _contentBorder.DesiredSize.Height;

                AnimateContent(0, targetHeight, 0, 1, true);
            }
            else
            {
                double fromHeight = _contentBorder.ActualHeight;
                AnimateContent(fromHeight, 0, 1, 0, false);
            }

            RaiseExpandedOrCollapsed(expanded);
        }

        private void AnimateChevron(double target)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.To = target;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(160));
            animation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, _chevronTransform);
            Storyboard.SetTargetProperty(animation, "Angle");
            storyboard.Begin();
        }

        private void AnimateContent(double fromHeight, double toHeight, double fromOpacity, double toOpacity, bool clearHeightWhenDone)
        {
            DoubleAnimation heightAnimation = new DoubleAnimation();
            heightAnimation.From = fromHeight;
            heightAnimation.To = toHeight;
            heightAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(180));
            heightAnimation.EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut };
            heightAnimation.EnableDependentAnimation = true; // Height is a layout property
            Storyboard.SetTarget(heightAnimation, _contentBorder);
            Storyboard.SetTargetProperty(heightAnimation, "Height");

            DoubleAnimation opacityAnimation = new DoubleAnimation();
            opacityAnimation.From = fromOpacity;
            opacityAnimation.To = toOpacity;
            opacityAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(180));
            Storyboard.SetTarget(opacityAnimation, _contentBorder);
            Storyboard.SetTargetProperty(opacityAnimation, "Opacity");

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(heightAnimation);
            storyboard.Children.Add(opacityAnimation);
            storyboard.Completed += (s, e) =>
            {
                if (clearHeightWhenDone)
                {
                    _contentBorder.Height = double.NaN; // back to Auto after opening
                }
                else
                {
                    _contentBorder.Visibility = Visibility.Collapsed;
                    _contentBorder.Height = double.NaN;
                    _contentBorder.Opacity = 1;
                }
            };
            storyboard.Begin();
        }

        private void RaiseExpandedOrCollapsed(bool expanded)
        {
            EventHandler handler = expanded ? Expanded : Collapsed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
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
    }
}
