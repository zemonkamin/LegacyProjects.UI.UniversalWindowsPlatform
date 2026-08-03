using System;
using System.Globalization;
using LegacyProjects.UI.UniversalWindowsPlatform.Enums;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Controls
{
    /// <summary>
    /// A numeric input field with optional increment/decrement spin buttons, range clamping and
    /// parsing. Modeled on the WinUI 2.3 NumberBox, which is not available in the Windows 10 SDK 14393.
    /// </summary>
    public sealed class NumberBox : UserControl
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(string), typeof(NumberBox), new PropertyMetadata(string.Empty, OnHeaderChanged));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(double), typeof(NumberBox), new PropertyMetadata(double.NaN, OnValueChanged));

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(double), typeof(NumberBox), new PropertyMetadata(double.NegativeInfinity));

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(double), typeof(NumberBox), new PropertyMetadata(double.PositiveInfinity));

        public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register(
            "SmallChange", typeof(double), typeof(NumberBox), new PropertyMetadata(1.0));

        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
            "PlaceholderText", typeof(string), typeof(NumberBox), new PropertyMetadata(string.Empty, OnPlaceholderChanged));

        public static readonly DependencyProperty SpinButtonPlacementModeProperty = DependencyProperty.Register(
            "SpinButtonPlacementMode", typeof(NumberBoxSpinButtonPlacement), typeof(NumberBox), new PropertyMetadata(NumberBoxSpinButtonPlacement.Inline, OnPlacementChanged));

        public static readonly DependencyProperty NumberFormatProperty = DependencyProperty.Register(
            "NumberFormat", typeof(string), typeof(NumberBox), new PropertyMetadata(null, OnValueChanged));

        private readonly StackPanel _root;
        private readonly TextBlock _headerBlock;
        private readonly Grid _inputGrid;
        private readonly TextBox _textBox;
        private readonly RepeatButton _downButton;
        private readonly RepeatButton _upButton;
        private bool _isUpdatingText;

        /// <summary>Raised when <see cref="Value"/> changes after parsing or a spin action.</summary>
        public event EventHandler ValueChanged;

        public NumberBox()
        {
            _headerBlock = new TextBlock();
            _headerBlock.Margin = new Thickness(0, 0, 0, 4);
            _headerBlock.Visibility = Visibility.Collapsed;

            _textBox = new TextBox();
            _textBox.InputScope = MakeNumberScope();
            _textBox.TextAlignment = TextAlignment.Left;
            _textBox.LostFocus += OnTextBoxLostFocus;
            _textBox.KeyDown += OnTextBoxKeyDown;

            _downButton = CreateSpinButton(""); // Remove / minus
            _upButton = CreateSpinButton("");   // Add / plus
            _downButton.Click += OnDownClick;
            _upButton.Click += OnUpClick;

            _inputGrid = new Grid();
            _inputGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            _inputGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            _inputGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            Grid.SetColumn(_textBox, 0);
            Grid.SetColumn(_downButton, 1);
            Grid.SetColumn(_upButton, 2);
            _inputGrid.Children.Add(_textBox);
            _inputGrid.Children.Add(_downButton);
            _inputGrid.Children.Add(_upButton);

            _root = new StackPanel();
            _root.Children.Add(_headerBlock);
            _root.Children.Add(_inputGrid);
            Content = _root;

            Loaded += OnLoaded;
        }

        /// <summary>Optional header label shown above the input.</summary>
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>The numeric value. <see cref="double.NaN"/> means empty.</summary>
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>Lower bound applied when parsing or spinning. Default negative infinity.</summary>
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>Upper bound applied when parsing or spinning. Default positive infinity.</summary>
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>Step applied by the spin buttons and arrow keys. Default 1.</summary>
        public double SmallChange
        {
            get { return (double)GetValue(SmallChangeProperty); }
            set { SetValue(SmallChangeProperty, value); }
        }

        /// <summary>Placeholder shown when the field is empty.</summary>
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        /// <summary>Whether spin buttons are inline or hidden.</summary>
        public NumberBoxSpinButtonPlacement SpinButtonPlacementMode
        {
            get { return (NumberBoxSpinButtonPlacement)GetValue(SpinButtonPlacementModeProperty); }
            set { SetValue(SpinButtonPlacementModeProperty, value); }
        }

        /// <summary>Optional .NET numeric format string (for example "F2") used when displaying the value.</summary>
        public string NumberFormat
        {
            get { return (string)GetValue(NumberFormatProperty); }
            set { SetValue(NumberFormatProperty, value); }
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            UpdateHeader();
            UpdatePlaceholder();
            UpdatePlacement();
            UpdateTextFromValue();
        }

        private static void OnHeaderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            NumberBox box = sender as NumberBox;
            if (box != null)
            {
                box.UpdateHeader();
            }
        }

        private static void OnPlaceholderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            NumberBox box = sender as NumberBox;
            if (box != null)
            {
                box.UpdatePlaceholder();
            }
        }

        private static void OnPlacementChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            NumberBox box = sender as NumberBox;
            if (box != null)
            {
                box.UpdatePlacement();
            }
        }

        private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            NumberBox box = sender as NumberBox;
            if (box == null || box._isUpdatingText)
            {
                return;
            }

            box.UpdateTextFromValue();
            box.RaiseValueChanged();
        }

        private void UpdateHeader()
        {
            bool hasHeader = !string.IsNullOrEmpty(Header);
            _headerBlock.Text = hasHeader ? Header : string.Empty;
            _headerBlock.Visibility = hasHeader ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdatePlaceholder()
        {
            _textBox.PlaceholderText = PlaceholderText != null ? PlaceholderText : string.Empty;
        }

        private void UpdatePlacement()
        {
            Visibility visibility = SpinButtonPlacementMode == NumberBoxSpinButtonPlacement.Inline ? Visibility.Visible : Visibility.Collapsed;
            _downButton.Visibility = visibility;
            _upButton.Visibility = visibility;
        }

        private void UpdateTextFromValue()
        {
            _isUpdatingText = true;
            try
            {
                if (double.IsNaN(Value))
                {
                    _textBox.Text = string.Empty;
                }
                else
                {
                    string format = NumberFormat;
                    _textBox.Text = string.IsNullOrEmpty(format)
                        ? Value.ToString(CultureInfo.CurrentCulture)
                        : Value.ToString(format, CultureInfo.CurrentCulture);
                }
            }
            finally
            {
                _isUpdatingText = false;
            }
        }

        private void OnTextBoxLostFocus(object sender, RoutedEventArgs args)
        {
            CommitText();
        }

        private void OnTextBoxKeyDown(object sender, KeyRoutedEventArgs args)
        {
            if (args.Key == VirtualKey.Enter)
            {
                CommitText();
                args.Handled = true;
            }
            else if (args.Key == VirtualKey.Up)
            {
                Step(GetEffectiveStep());
                args.Handled = true;
            }
            else if (args.Key == VirtualKey.Down)
            {
                Step(-GetEffectiveStep());
                args.Handled = true;
            }
        }

        private void CommitText()
        {
            string text = _textBox.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                SetValueInternal(double.NaN);
                UpdateTextFromValue();
                return;
            }

            double parsed;
            if (double.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out parsed) ||
                double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out parsed))
            {
                SetValueInternal(Clamp(parsed));
            }

            UpdateTextFromValue();
        }

        private void OnUpClick(object sender, RoutedEventArgs args)
        {
            Step(GetEffectiveStep());
        }

        private void OnDownClick(object sender, RoutedEventArgs args)
        {
            Step(-GetEffectiveStep());
        }

        private double GetEffectiveStep()
        {
            double step = SmallChange;
            if (double.IsNaN(step) || step == 0)
            {
                return 1.0;
            }

            return step;
        }

        private void Step(double delta)
        {
            double current = double.IsNaN(Value) ? 0 : Value;
            SetValueInternal(Clamp(current + delta));
            UpdateTextFromValue();
        }

        private void SetValueInternal(double value)
        {
            if (Value.Equals(value))
            {
                return;
            }

            Value = value;
        }

        private double Clamp(double value)
        {
            if (double.IsNaN(value))
            {
                return value;
            }

            if (value < Minimum)
            {
                return Minimum;
            }

            if (value > Maximum)
            {
                return Maximum;
            }

            return value;
        }

        private void RaiseValueChanged()
        {
            EventHandler handler = ValueChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private RepeatButton CreateSpinButton(string glyph)
        {
            RepeatButton button = new RepeatButton();
            button.Width = 34;
            button.MinWidth = 34;
            button.Padding = new Thickness(0);
            button.BorderThickness = new Thickness(0);
            button.Background = new SolidColorBrush(Colors.Transparent);
            button.VerticalAlignment = VerticalAlignment.Stretch;

            FontIcon icon = new FontIcon();
            icon.FontFamily = new FontFamily("Segoe MDL2 Assets");
            icon.FontSize = 12;
            icon.Glyph = glyph;
            button.Content = icon;
            return button;
        }

        private static InputScope MakeNumberScope()
        {
            InputScope scope = new InputScope();
            InputScopeName name = new InputScopeName();
            name.NameValue = InputScopeNameValue.Number;
            scope.Names.Add(name);
            return scope;
        }
    }
}
