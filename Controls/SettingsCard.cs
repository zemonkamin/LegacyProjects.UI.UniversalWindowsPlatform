using System;
using LegacyProjects.UI.UniversalWindowsPlatform.Enums;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace LegacyProjects.UI.UniversalWindowsPlatform.Controls
{
    /// <summary>
    /// A settings row card: an optional leading icon, a header and description, and an action element
    /// (a toggle, a button, a combo box, and so on) placed on the right or below the text. Mirrors the
    /// WinUI Community Toolkit SettingsCard, which is not in the Windows 10 SDK 14393. The action element
    /// is the XAML content: <code>&lt;ui:SettingsCard Header="Theme"&gt;&lt;ToggleSwitch/&gt;&lt;/ui:SettingsCard&gt;</code>
    /// </summary>
    [ContentProperty(Name = "ActionContent")]
    public sealed class SettingsCard : UserControl
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(string), typeof(SettingsCard), new PropertyMetadata(string.Empty, OnVisualChanged));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            "Description", typeof(string), typeof(SettingsCard), new PropertyMetadata(string.Empty, OnVisualChanged));

        public static readonly DependencyProperty IconGlyphProperty = DependencyProperty.Register(
            "IconGlyph", typeof(string), typeof(SettingsCard), new PropertyMetadata(string.Empty, OnVisualChanged));

        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register(
            "IconFontFamily", typeof(string), typeof(SettingsCard), new PropertyMetadata("Segoe MDL2 Assets", OnVisualChanged));

        public static readonly DependencyProperty IconImageSourceProperty = DependencyProperty.Register(
            "IconImageSource", typeof(ImageSource), typeof(SettingsCard), new PropertyMetadata(null, OnVisualChanged));

        public static readonly DependencyProperty ActionContentProperty = DependencyProperty.Register(
            "ActionContent", typeof(object), typeof(SettingsCard), new PropertyMetadata(null, OnActionChanged));

        public static readonly DependencyProperty ActionPlacementProperty = DependencyProperty.Register(
            "ActionPlacement", typeof(SettingsCardActionPlacement), typeof(SettingsCard), new PropertyMetadata(SettingsCardActionPlacement.Right, OnVisualChanged));

        public static readonly DependencyProperty IsClickableProperty = DependencyProperty.Register(
            "IsClickable", typeof(bool), typeof(SettingsCard), new PropertyMetadata(false, OnVisualChanged));

        public static readonly DependencyProperty CardBackgroundProperty = DependencyProperty.Register(
            "CardBackground", typeof(Brush), typeof(SettingsCard), new PropertyMetadata(null, OnVisualChanged));

        public static readonly DependencyProperty CornerRadiusValueProperty = DependencyProperty.Register(
            "CornerRadiusValue", typeof(CornerRadius), typeof(SettingsCard), new PropertyMetadata(new CornerRadius(0), OnVisualChanged));

        private readonly Border _card;
        private readonly StackPanel _outer;
        private readonly Grid _topRow;
        private readonly StackPanel _bottomHost;
        private readonly FontIcon _icon;
        private readonly Image _iconImage;
        private readonly TextBlock _header;
        private readonly TextBlock _description;
        private readonly StackPanel _textStack;
        private readonly ContentPresenter _actionPresenter;
        private readonly FontIcon _chevron;

        /// <summary>Raised when <see cref="IsClickable"/> is true and the card is tapped.</summary>
        public event EventHandler Click;

        public SettingsCard()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;

            _icon = new FontIcon();
            _icon.FontFamily = new FontFamily("Segoe MDL2 Assets");
            _icon.FontSize = 20;
            _icon.VerticalAlignment = VerticalAlignment.Center;
            _icon.Margin = new Thickness(0, 0, 16, 0);
            _icon.Visibility = Visibility.Collapsed;

            _iconImage = new Image();
            _iconImage.Width = 30;
            _iconImage.Height = 30;
            _iconImage.Stretch = Stretch.Uniform;
            _iconImage.VerticalAlignment = VerticalAlignment.Center;
            _iconImage.Margin = new Thickness(0, 0, 16, 0);
            _iconImage.Visibility = Visibility.Collapsed;

            _header = new TextBlock();
            _header.FontSize = 15;
            _header.TextWrapping = TextWrapping.Wrap;
            _header.VerticalAlignment = VerticalAlignment.Center;

            _description = new TextBlock();
            _description.FontSize = 12;
            _description.Opacity = 0.75;
            _description.TextWrapping = TextWrapping.Wrap;
            _description.Margin = new Thickness(0, 2, 0, 0);
            _description.Visibility = Visibility.Collapsed;

            _textStack = new StackPanel();
            _textStack.VerticalAlignment = VerticalAlignment.Center;
            _textStack.Children.Add(_header);
            _textStack.Children.Add(_description);

            _actionPresenter = new ContentPresenter();
            _actionPresenter.VerticalAlignment = VerticalAlignment.Center;

            _chevron = new FontIcon();
            _chevron.FontFamily = new FontFamily("Segoe MDL2 Assets");
            _chevron.Glyph = ""; // ChevronRight
            _chevron.FontSize = 12;
            _chevron.Margin = new Thickness(12, 0, 0, 0);
            _chevron.VerticalAlignment = VerticalAlignment.Center;
            _chevron.Visibility = Visibility.Collapsed;

            _topRow = new Grid();
            _topRow.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            _topRow.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            _topRow.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            _topRow.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            Grid.SetColumn(_icon, 0);
            Grid.SetColumn(_iconImage, 0);
            Grid.SetColumn(_textStack, 1);
            Grid.SetColumn(_chevron, 3);
            _topRow.Children.Add(_icon);
            _topRow.Children.Add(_iconImage);
            _topRow.Children.Add(_textStack);
            _topRow.Children.Add(_chevron);

            _bottomHost = new StackPanel();
            _bottomHost.HorizontalAlignment = HorizontalAlignment.Left;
            _bottomHost.Margin = new Thickness(0, 12, 0, 0);
            _bottomHost.Visibility = Visibility.Collapsed;

            _outer = new StackPanel();
            _outer.Children.Add(_topRow);
            _outer.Children.Add(_bottomHost);

            _card = new Border();
            _card.Padding = new Thickness(16, 14, 16, 14);
            _card.MinHeight = 64;
            _card.Child = _outer;
            _card.Tapped += OnCardTapped;

            Content = _card;
            Loaded += OnLoaded;
        }

        /// <summary>Primary title of the row.</summary>
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>Secondary description under the header. Hidden when empty.</summary>
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        /// <summary>Optional leading icon glyph (from <see cref="IconFontFamily"/>).</summary>
        public string IconGlyph
        {
            get { return (string)GetValue(IconGlyphProperty); }
            set { SetValue(IconGlyphProperty, value); }
        }

        /// <summary>Font family for <see cref="IconGlyph"/>. Default "Segoe MDL2 Assets".</summary>
        public string IconFontFamily
        {
            get { return (string)GetValue(IconFontFamilyProperty); }
            set { SetValue(IconFontFamilyProperty, value); }
        }

        /// <summary>Optional leading image icon. When set it takes priority over <see cref="IconGlyph"/>.</summary>
        public ImageSource IconImageSource
        {
            get { return (ImageSource)GetValue(IconImageSourceProperty); }
            set { SetValue(IconImageSourceProperty, value); }
        }

        /// <summary>Trailing/bottom action element. This is the XAML content property.</summary>
        public object ActionContent
        {
            get { return GetValue(ActionContentProperty); }
            set { SetValue(ActionContentProperty, value); }
        }

        /// <summary>Whether the action element sits to the right or below the text.</summary>
        public SettingsCardActionPlacement ActionPlacement
        {
            get { return (SettingsCardActionPlacement)GetValue(ActionPlacementProperty); }
            set { SetValue(ActionPlacementProperty, value); }
        }

        /// <summary>When true the whole card behaves like a button and shows a chevron; raises <see cref="Click"/>.</summary>
        public bool IsClickable
        {
            get { return (bool)GetValue(IsClickableProperty); }
            set { SetValue(IsClickableProperty, value); }
        }

        /// <summary>Background brush of the card. When null a theme-neutral translucent fill is used.</summary>
        public Brush CardBackground
        {
            get { return (Brush)GetValue(CardBackgroundProperty); }
            set { SetValue(CardBackgroundProperty, value); }
        }

        /// <summary>Corner radius of the card.</summary>
        public CornerRadius CornerRadiusValue
        {
            get { return (CornerRadius)GetValue(CornerRadiusValueProperty); }
            set { SetValue(CornerRadiusValueProperty, value); }
        }

        private static void OnVisualChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            SettingsCard card = sender as SettingsCard;
            if (card != null)
            {
                card.ApplyVisuals();
            }
        }

        private static void OnActionChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            SettingsCard card = sender as SettingsCard;
            if (card != null)
            {
                card._actionPresenter.Content = card.ActionContent;
                card.ApplyVisuals();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs args)
        {
            _actionPresenter.Content = ActionContent;
            ApplyVisuals();
        }

        private void OnCardTapped(object sender, TappedRoutedEventArgs args)
        {
            if (!IsClickable)
            {
                return;
            }

            EventHandler handler = Click;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void ApplyVisuals()
        {
            _header.Text = Header != null ? Header : string.Empty;

            bool hasDescription = !string.IsNullOrEmpty(Description);
            _description.Text = hasDescription ? Description : string.Empty;
            _description.Visibility = hasDescription ? Visibility.Visible : Visibility.Collapsed;

            if (IconImageSource != null)
            {
                _iconImage.Source = IconImageSource;
                _iconImage.Visibility = Visibility.Visible;
                _icon.Visibility = Visibility.Collapsed;
            }
            else
            {
                _iconImage.Visibility = Visibility.Collapsed;

                bool hasIcon = !string.IsNullOrEmpty(IconGlyph);
                if (hasIcon)
                {
                    _icon.Glyph = IconGlyph;
                    _icon.FontFamily = new FontFamily(string.IsNullOrEmpty(IconFontFamily) ? "Segoe MDL2 Assets" : IconFontFamily);
                }
                _icon.Visibility = hasIcon ? Visibility.Visible : Visibility.Collapsed;
            }

            _chevron.Visibility = IsClickable ? Visibility.Visible : Visibility.Collapsed;

            PlaceActionElement();

            // Theme-neutral translucent fill so the card looks correct in both light and dark themes
            // (the header/description TextBlocks inherit the element theme foreground automatically).
            _card.Background = CardBackground != null ? CardBackground : new SolidColorBrush(Color.FromArgb(18, 128, 128, 128));
            _card.BorderBrush = new SolidColorBrush(Color.FromArgb(38, 128, 128, 128));
            _card.BorderThickness = new Thickness(1);
            _card.CornerRadius = CornerRadiusValue;
        }

        private void PlaceActionElement()
        {
            // Detach from whichever host currently holds it. Using Contains is more reliable than
            // Parent for a ContentPresenter and avoids "Element is already the child of another element".
            if (_topRow.Children.Contains(_actionPresenter))
            {
                _topRow.Children.Remove(_actionPresenter);
            }

            if (_bottomHost.Children.Contains(_actionPresenter))
            {
                _bottomHost.Children.Remove(_actionPresenter);
            }

            if (ActionPlacement == SettingsCardActionPlacement.Bottom)
            {
                _actionPresenter.HorizontalAlignment = HorizontalAlignment.Left;
                _actionPresenter.Margin = new Thickness(0);
                _bottomHost.Children.Add(_actionPresenter);
                _bottomHost.Visibility = Visibility.Visible;
            }
            else
            {
                _actionPresenter.HorizontalAlignment = HorizontalAlignment.Right;
                _actionPresenter.Margin = new Thickness(16, 0, 0, 0);
                Grid.SetColumn(_actionPresenter, 2);
                _topRow.Children.Add(_actionPresenter);
                _bottomHost.Visibility = Visibility.Collapsed;
            }
        }
    }
}
