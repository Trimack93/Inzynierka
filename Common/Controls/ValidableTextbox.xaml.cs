using Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Common.Controls
{
    /// <summary>
    /// Interaction logic for KurwaMac.xaml
    /// </summary>
    public partial class ValidableTextbox : UserControl
    {
        public ValidableTextbox()
        {
            InitializeComponent();
        }

        //----------------------------------------------

        public string Tekst
        {
            get { return (string)GetValue(TekstProperty); }
            set { SetValue(TekstProperty, value); }
        }
        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }
        public bool IsNumeric
        {
            get { return (bool)GetValue(IsNumericProperty); }
            set { SetValue(IsNumericProperty, value); }
        }
        public bool IsNotEmpty
        {
            get { return (bool)GetValue(IsNotEmptyProperty); }
            set { SetValue(IsNotEmptyProperty, value); }
        }
        //----------------------------------------------

        public static readonly DependencyProperty TekstProperty = DependencyProperty.Register(
            "Tekst", typeof(string), typeof(ValidableTextbox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(TextPropertyChanged)));

        public static readonly DependencyProperty MaxLengthProperty = DependencyProperty.Register(
            "MaxLength", typeof(int), typeof(ValidableTextbox), new UIPropertyMetadata(null));

        public static readonly DependencyProperty IsNumericProperty = DependencyProperty.Register(
            "IsNumeric", typeof(bool), typeof(ValidableTextbox), new UIPropertyMetadata(false));

        public static readonly DependencyProperty IsNotEmptyProperty = DependencyProperty.Register(
            "IsNotEmpty", typeof(bool), typeof(ValidableTextbox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private static void TextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ValidableTextbox textBox = (ValidableTextbox)sender;
            textBox.Tekst = (string)e.NewValue;
        }

        //----------------------------------------------

        private bool _errorOccured = false;                                        // after user types last character, KeyUp animation will fire, but KeyDown not. This prevents it

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            string x = textBox.Text;

            // If the limit of characters is met and there is less than 1 character selected in textbox.
            // Also if user didn't pressed a button like Tab or Shift
            if ( x.Length == MaxLength && textBox.SelectionLength <= 1 &&
                (InputHelper.IsKeyAChar(e.Key) || InputHelper.IsKeyADigit(e.Key)) )
            {
                Storyboard s = (Storyboard)this.Resources["KeyDownStoryboard"];
                s.Begin();

                _errorOccured = true;
            }

            // If user provided a letter when textbox is numeric
            if ( this.IsNumeric && InputHelper.IsKeyAChar(e.Key) )
            {
                // If letter was pressed when the limit of characters wasn't met
                if (_errorOccured == false)
                {
                    Storyboard s = (Storyboard)this.Resources["KeyDownStoryboard"];
                    s.Begin();

                    _errorOccured = true;
                }

                e.Handled = true;                                       // Block the letter from being added into textbox
            }
        }
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            string text = (sender as TextBox).Text;

            // If error occured (and animation to red handled), go back into transparent border state
            if (_errorOccured)
            {
                Storyboard s = (Storyboard)this.Resources["KeyUpStoryboard"];
                s.Begin();

                _errorOccured = false;
            }

            if (text.Length == 0)
                SetValue(IsNotEmptyProperty, false);
            else
                SetValue(IsNotEmptyProperty, true);
        }
    }
}
