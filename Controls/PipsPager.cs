using System;
using LegacyProjects.UI.UniversalWindowsPlatform.Events;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Controls
{
    /// <summary>
    /// A row (or column) of selectable dots that represent pages, matching the WinUI 2.6 PipsPager,
    /// which does not exist in the Windows 10 SDK 14393. Useful as a carousel or wizard page indicator.
    /// </summary>
    public sealed class PipsPager : UserControl
    {
        public static readonly DependencyProperty NumberOfPagesProperty = DependencyProperty.Register(
            "NumberOfPages", typeof(int), typeof(PipsPager), new PropertyMetadata(0, OnStructureChanged));

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex", typeof(int), typeof(PipsPager), new PropertyMetadata(0, OnSelectedIndexChanged));

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof(Orientation), typeof(PipsPager), new PropertyMetadata(Orientation.Horizontal, OnStructureChanged));

        public static readonly DependencyProperty PipSizeProperty = DependencyProperty.Register(
            "PipSize", typeof(double), typeof(PipsPager), new PropertyMetadata(8.0, OnStructureChanged));

        public static readonly DependencyProperty PipSpacingProperty = DependencyProperty.Register(
            "PipSpacing", typeof(double), typeof(PipsPager), new PropertyMetadata(8.0, OnStructureChanged));

        public static readonly DependencyProperty SelectedPipBrushProperty = DependencyProperty.Register(
            "SelectedPipBrush", typeof(Brush), typeof(PipsPager), new PropertyMetadata(null, OnStructureChanged));

        public static readonly DependencyProperty PipBrushProperty = DependencyProperty.Register(
            "PipBrush", typeof(Brush), typeof(PipsPager), new PropertyMetadata(null, OnStructureChanged));

        private readonly StackPanel _panel;

        /// <summary>Raised when the selected page changes, whether by click or code.</summary>
        public event EventHandler<PipsPagerSelectedIndexChangedEventArgs> SelectedIndexChanged;

        public PipsPager()
        {
            _panel = new StackPanel();
            _panel.Orientation = Orientation.Horizontal;
            _panel.HorizontalAlignment = HorizontalAlignment.Center;
            _panel.VerticalAlignment = VerticalAlignment.Center;
            Content = _panel;
            Loaded += OnLoaded;
        }

        /// <summary>Total number of pages (dots).</summary>
        public int NumberOfPages
        {
            get { return (int)GetValue(NumberOfPagesProperty); }
            set { SetValue(NumberOfPagesProperty, value); }
        }

        /// <summary>Zero-based index of the selected page.</summary>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>Layout direction of the dots.</summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>Diameter of each dot. Default 8.</summary>
        public double PipSize
        {
            get { return (double)GetValue(PipSizeProperty); }
            set { SetValue(PipSizeProperty, value); }
        }

        /// <summary>Gap between dots. Default 8.</summary>
        public double PipSpacing
        {
            get { return (double)GetValue(PipSpacingProperty); }
            set { SetValue(PipSpacingProperty, value); }
        }

        /// <summary>Brush of the selected dot. Falls back to the accent brush.</summary>
        public Brush SelectedPipBrush
        {
            get { return (Brush)GetValue(SelectedPipBrushProperty); }
            set { SetValue(SelectedPipBrushProperty, value); }
        }

        /// <summary>Brush of unselected dots. Falls back to a subtle gray.</summary>
        public Brush PipBrush
        {
            get { return (Brush)GetValue(PipBrushProperty); }
            set { SetValue(PipBrushProperty, value); }
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            Rebuild();
        }

        private static void OnStructureChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            PipsPager pager = sender as PipsPager;
            if (pager != null)
            {
                pager.Rebuild();
            }
        }

        private static void OnSelectedIndexChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            PipsPager pager = sender as PipsPager;
            if (pager == null)
            {
                return;
            }

            int oldIndex = (int)args.OldValue;
            int newIndex = pager.NormalizeIndex((int)args.NewValue);
            if (newIndex != (int)args.NewValue)
            {
                pager.SelectedIndex = newIndex;
                return;
            }

            pager.Rebuild();

            EventHandler<PipsPagerSelectedIndexChangedEventArgs> handler = pager.SelectedIndexChanged;
            if (handler != null)
            {
                handler(pager, new PipsPagerSelectedIndexChangedEventArgs(oldIndex, newIndex));
            }
        }

        private int NormalizeIndex(int index)
        {
            int count = NumberOfPages;
            if (count <= 0)
            {
                return 0;
            }

            if (index < 0)
            {
                return 0;
            }

            if (index >= count)
            {
                return count - 1;
            }

            return index;
        }

        private void Rebuild()
        {
            _panel.Orientation = Orientation;
            _panel.Children.Clear();

            int count = NumberOfPages;
            if (count <= 0)
            {
                return;
            }

            double size = Math.Max(2.0, PipSize);
            double spacing = Math.Max(0.0, PipSpacing);
            Brush selectedBrush = SelectedPipBrush != null ? SelectedPipBrush : ResolveThemeBrush("SystemControlHighlightAccentBrush", new SolidColorBrush(Color.FromArgb(255, 0, 120, 215)));
            Brush normalBrush = PipBrush != null ? PipBrush : ResolveThemeBrush("SystemControlForegroundBaseMediumLowBrush", new SolidColorBrush(Color.FromArgb(120, 128, 128, 128)));

            int selected = NormalizeIndex(SelectedIndex);

            for (int i = 0; i < count; i++)
            {
                bool isSelected = i == selected;

                Ellipse dot = new Ellipse();
                dot.Width = size;
                dot.Height = size;
                dot.Fill = isSelected ? selectedBrush : normalBrush;

                Button button = new Button();
                button.Padding = new Thickness(0);
                button.BorderThickness = new Thickness(0);
                button.Background = new SolidColorBrush(Colors.Transparent);
                button.Content = dot;
                button.Tag = i;
                button.Click += OnPipClick;

                double half = spacing / 2.0;
                if (Orientation == Orientation.Horizontal)
                {
                    button.Margin = new Thickness(i == 0 ? 0 : half, 0, i == count - 1 ? 0 : half, 0);
                }
                else
                {
                    button.Margin = new Thickness(0, i == 0 ? 0 : half, 0, i == count - 1 ? 0 : half);
                }

                _panel.Children.Add(button);
            }
        }

        private void OnPipClick(object sender, RoutedEventArgs args)
        {
            FrameworkElement element = sender as FrameworkElement;
            if (element == null || element.Tag == null)
            {
                return;
            }

            SelectedIndex = (int)element.Tag;
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
