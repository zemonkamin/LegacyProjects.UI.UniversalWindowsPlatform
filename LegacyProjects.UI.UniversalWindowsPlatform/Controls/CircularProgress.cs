using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Controls
{
    /// <summary>
    /// A ring-shaped progress control. Supports a determinate mode (an arc that fills from the
    /// 12 o'clock position based on <see cref="Value"/>) and an indeterminate mode (a rotating arc).
    /// The Windows 10 SDK 14393 <c>ProgressRing</c> is indeterminate only; this control adds the
    /// determinate ring found in WinUI 2/3, plus optional centered text.
    /// </summary>
    public sealed class CircularProgress : UserControl
    {
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(double), typeof(CircularProgress), new PropertyMetadata(0.0, OnValueChanged));

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(double), typeof(CircularProgress), new PropertyMetadata(100.0, OnValueChanged));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(CircularProgress), new PropertyMetadata(0.0, OnValueChanged));

        public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(
            "IsIndeterminate", typeof(bool), typeof(CircularProgress), new PropertyMetadata(false, OnModeChanged));

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
            "StrokeThickness", typeof(double), typeof(CircularProgress), new PropertyMetadata(4.0, OnLayoutChanged));

        public static readonly DependencyProperty ProgressBrushProperty = DependencyProperty.Register(
            "ProgressBrush", typeof(Brush), typeof(CircularProgress), new PropertyMetadata(null, OnLayoutChanged));

        public static readonly DependencyProperty TrackBrushProperty = DependencyProperty.Register(
            "TrackBrush", typeof(Brush), typeof(CircularProgress), new PropertyMetadata(null, OnLayoutChanged));

        public static readonly DependencyProperty RoundCapsProperty = DependencyProperty.Register(
            "RoundCaps", typeof(bool), typeof(CircularProgress), new PropertyMetadata(true, OnLayoutChanged));

        public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register(
            "ShowText", typeof(bool), typeof(CircularProgress), new PropertyMetadata(false, OnLayoutChanged));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(CircularProgress), new PropertyMetadata(null, OnTextChanged));

        public static readonly DependencyProperty TextForegroundProperty = DependencyProperty.Register(
            "TextForeground", typeof(Brush), typeof(CircularProgress), new PropertyMetadata(null, OnTextChanged));

        public static readonly DependencyProperty TextFontSizeProperty = DependencyProperty.Register(
            "TextFontSize", typeof(double), typeof(CircularProgress), new PropertyMetadata(double.NaN, OnTextChanged));

        public static readonly DependencyProperty IndeterminateArcLengthProperty = DependencyProperty.Register(
            "IndeterminateArcLength", typeof(double), typeof(CircularProgress), new PropertyMetadata(0.25, OnModeChanged));

        public static readonly DependencyProperty IndeterminateDurationProperty = DependencyProperty.Register(
            "IndeterminateDuration", typeof(double), typeof(CircularProgress), new PropertyMetadata(1100.0, OnModeChanged));

        private readonly Grid _root;
        private readonly Ellipse _track;
        private readonly Path _progressPath;
        private readonly PathFigure _figure;
        private readonly ArcSegment _arc;
        private readonly RotateTransform _spinTransform;
        private readonly TextBlock _centerText;
        private Storyboard _spinStoryboard;

        public CircularProgress()
        {
            Width = 48;
            Height = 48;

            _track = new Ellipse();
            _track.HorizontalAlignment = HorizontalAlignment.Stretch;
            _track.VerticalAlignment = VerticalAlignment.Stretch;

            _arc = new ArcSegment();
            _arc.SweepDirection = SweepDirection.Clockwise;
            _arc.RotationAngle = 0;

            _figure = new PathFigure();
            _figure.IsClosed = false;
            _figure.Segments.Add(_arc);

            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(_figure);

            _spinTransform = new RotateTransform();

            _progressPath = new Path();
            _progressPath.Data = geometry;
            _progressPath.RenderTransform = _spinTransform;
            // The arc geometry is drawn in absolute 0..size coordinates, so pin the path to the
            // top-left of the (square) ring host instead of letting layout re-center it.
            _progressPath.HorizontalAlignment = HorizontalAlignment.Left;
            _progressPath.VerticalAlignment = VerticalAlignment.Top;

            _centerText = new TextBlock();
            _centerText.HorizontalAlignment = HorizontalAlignment.Center;
            _centerText.VerticalAlignment = VerticalAlignment.Center;
            _centerText.TextAlignment = TextAlignment.Center;
            _centerText.Visibility = Visibility.Collapsed;

            // A square host centered inside the control. The track ellipse and the progress arc share
            // this exact square, so they always line up regardless of the control's aspect ratio.
            _root = new Grid();
            _root.HorizontalAlignment = HorizontalAlignment.Center;
            _root.VerticalAlignment = VerticalAlignment.Center;
            _root.Children.Add(_track);
            _root.Children.Add(_progressPath);
            _root.Children.Add(_centerText);
            Content = _root;

            SizeChanged += OnSizeChanged;
            Loaded += OnLoaded;
        }

        /// <summary>The value corresponding to an empty ring. Default 0.</summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>The value corresponding to a full ring. Default 100.</summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>The current progress value between <see cref="Minimum"/> and <see cref="Maximum"/>.</summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>When true, the ring shows a continuously rotating arc instead of a fixed value.</summary>
        public bool IsIndeterminate
        {
            get { return (bool)GetValue(IsIndeterminateProperty); }
            set { SetValue(IsIndeterminateProperty, value); }
        }

        /// <summary>Thickness of the ring stroke. Default 4.</summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>Brush of the progress arc. Falls back to the system accent brush.</summary>
        public Brush ProgressBrush
        {
            get { return (Brush)GetValue(ProgressBrushProperty); }
            set { SetValue(ProgressBrushProperty, value); }
        }

        /// <summary>Brush of the background track ring. Falls back to a subtle gray.</summary>
        public Brush TrackBrush
        {
            get { return (Brush)GetValue(TrackBrushProperty); }
            set { SetValue(TrackBrushProperty, value); }
        }

        /// <summary>When true, the arc ends are rounded. Default true.</summary>
        public bool RoundCaps
        {
            get { return (bool)GetValue(RoundCapsProperty); }
            set { SetValue(RoundCapsProperty, value); }
        }

        /// <summary>When true, text is shown in the center of the ring (either <see cref="Text"/> or the percentage).</summary>
        public bool ShowText
        {
            get { return (bool)GetValue(ShowTextProperty); }
            set { SetValue(ShowTextProperty, value); }
        }

        /// <summary>Explicit center text. If null and <see cref="ShowText"/> is true, the percentage is shown.</summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>Brush of the center text. Falls back to the control foreground.</summary>
        public Brush TextForeground
        {
            get { return (Brush)GetValue(TextForegroundProperty); }
            set { SetValue(TextForegroundProperty, value); }
        }

        /// <summary>Font size of the center text. Defaults to roughly a third of the diameter.</summary>
        public double TextFontSize
        {
            get { return (double)GetValue(TextFontSizeProperty); }
            set { SetValue(TextFontSizeProperty, value); }
        }

        /// <summary>Length of the rotating arc in indeterminate mode, as a fraction 0..1. Default 0.25.</summary>
        public double IndeterminateArcLength
        {
            get { return (double)GetValue(IndeterminateArcLengthProperty); }
            set { SetValue(IndeterminateArcLengthProperty, value); }
        }

        /// <summary>Time in milliseconds for a full rotation in indeterminate mode. Default 1100.</summary>
        public double IndeterminateDuration
        {
            get { return (double)GetValue(IndeterminateDurationProperty); }
            set { SetValue(IndeterminateDurationProperty, value); }
        }

        private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircularProgress control = sender as CircularProgress;
            if (control != null)
            {
                control.Render();
            }
        }

        private static void OnLayoutChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircularProgress control = sender as CircularProgress;
            if (control != null)
            {
                control.Render();
            }
        }

        private static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircularProgress control = sender as CircularProgress;
            if (control != null)
            {
                control.UpdateText();
            }
        }

        private static void OnModeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircularProgress control = sender as CircularProgress;
            if (control != null)
            {
                control.Render();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            Render();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs args)
        {
            Render();
        }

        private double Diameter
        {
            get { return Math.Min(ActualWidth, ActualHeight); }
        }

        private void Render()
        {
            double diameter = Diameter;
            double thickness = Math.Max(0.5, StrokeThickness);

            Brush progressBrush = ProgressBrush != null ? ProgressBrush : ResolveThemeBrush("SystemControlHighlightAccentBrush", new SolidColorBrush(Color.FromArgb(255, 0, 120, 215)));
            Brush trackBrush = TrackBrush != null ? TrackBrush : ResolveThemeBrush("SystemControlBackgroundBaseLowBrush", new SolidColorBrush(Color.FromArgb(40, 128, 128, 128)));

            PenLineCap cap = RoundCaps ? PenLineCap.Round : PenLineCap.Flat;

            _track.Stroke = trackBrush;
            _track.StrokeThickness = thickness;
            _track.Fill = new SolidColorBrush(Colors.Transparent);
            _track.Margin = new Thickness(thickness / 2.0);

            _progressPath.Stroke = progressBrush;
            _progressPath.StrokeThickness = thickness;
            _progressPath.StrokeStartLineCap = cap;
            _progressPath.StrokeEndLineCap = cap;

            if (diameter <= 0)
            {
                return;
            }

            double radius = (diameter - thickness) / 2.0;
            if (radius <= 0)
            {
                _progressPath.Visibility = Visibility.Collapsed;
                return;
            }

            // Size the square host to the smaller dimension; it is centered inside the control.
            _root.Width = diameter;
            _root.Height = diameter;

            Point center = new Point(diameter / 2.0, diameter / 2.0);
            _progressPath.Width = diameter;
            _progressPath.Height = diameter;
            _spinTransform.CenterX = center.X;
            _spinTransform.CenterY = center.Y;

            if (IsIndeterminate)
            {
                double fraction = Clamp01(IndeterminateArcLength);
                if (fraction <= 0)
                {
                    fraction = 0.25;
                }

                BuildArc(center, radius, fraction);
                _progressPath.Visibility = Visibility.Visible;
                StartSpin();
            }
            else
            {
                StopSpin();
                _spinTransform.Angle = 0;

                double fraction = GetProgressFraction();
                if (fraction <= 0)
                {
                    _progressPath.Visibility = Visibility.Collapsed;
                }
                else
                {
                    _progressPath.Visibility = Visibility.Visible;
                    BuildArc(center, radius, fraction);
                }
            }

            UpdateText();
        }

        private double GetProgressFraction()
        {
            double range = Maximum - Minimum;
            if (range <= 0)
            {
                return 0;
            }

            double fraction = (Value - Minimum) / range;
            return Clamp01(fraction);
        }

        private void BuildArc(Point center, double radius, double fraction)
        {
            // A single ArcSegment cannot express a full 360 degree circle (its start point would equal
            // its end point), so a near-complete ring is approximated by sweeping just under 360 degrees.
            // With round caps this reads as a closed ring.
            if (fraction >= 0.999)
            {
                fraction = 0.99972; // ~359.9 degrees
            }

            double sweep = fraction * 2.0 * Math.PI;
            double start = -Math.PI / 2.0;
            double end = start + sweep;

            _figure.StartPoint = new Point(center.X + radius * Math.Cos(start), center.Y + radius * Math.Sin(start));
            _arc.Point = new Point(center.X + radius * Math.Cos(end), center.Y + radius * Math.Sin(end));
            _arc.Size = new Size(radius, radius);
            _arc.IsLargeArc = fraction > 0.5;
        }

        private void UpdateText()
        {
            bool show = ShowText;
            if (!show)
            {
                _centerText.Visibility = Visibility.Collapsed;
                return;
            }

            string content = Text;
            if (string.IsNullOrEmpty(content))
            {
                int percent = (int)Math.Round(GetProgressFraction() * 100.0);
                content = percent + "%";
            }

            _centerText.Text = content;
            if (TextForeground != null)
            {
                _centerText.Foreground = TextForeground;
            }
            else if (Foreground != null)
            {
                _centerText.Foreground = Foreground;
            }
            else
            {
                // Inherit the ambient (element-theme) foreground instead of snapshotting an app-theme brush.
                _centerText.ClearValue(TextBlock.ForegroundProperty);
            }
            _centerText.FontSize = !double.IsNaN(TextFontSize) && TextFontSize > 0 ? TextFontSize : Math.Max(9.0, Diameter / 3.2);
            _centerText.Visibility = Visibility.Visible;
        }

        private void StartSpin()
        {
            StopSpin();

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 360;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(Math.Max(100, IndeterminateDuration)));
            animation.RepeatBehavior = RepeatBehavior.Forever;

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, _spinTransform);
            Storyboard.SetTargetProperty(animation, "Angle");

            _spinStoryboard = storyboard;
            storyboard.Begin();
        }

        private void StopSpin()
        {
            if (_spinStoryboard != null)
            {
                _spinStoryboard.Stop();
                _spinStoryboard = null;
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

        private static double Clamp01(double value)
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
    }
}
