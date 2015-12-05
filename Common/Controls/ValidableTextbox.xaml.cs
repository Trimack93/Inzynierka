using System;
using System.Collections.Generic;
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

        //----------------------------------------------

        public static readonly DependencyProperty TekstProperty = DependencyProperty.Register(
            "Tekst", typeof(string), typeof(ValidableTextbox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(TextPropertyChanged)));

        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(ValidableTextbox), new UIPropertyMetadata(null));

        private static void TextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ValidableTextbox textBox = (ValidableTextbox)sender;
            textBox.Tekst = (string)e.NewValue;
        }

        //----------------------------------------------

        private bool isLimitReached = false;                                        // after user types last character, KeyUp animation will fire, but KeyDown not. This prevents it

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            string x = (sender as TextBox).Text;

            if (x.Length == MaxLength)
            {
                Storyboard s = (Storyboard)this.Resources["KeyDownStoryboard"];
                s.Begin();

                isLimitReached = true;
            }
        }
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            string x = (sender as TextBox).Text;

            if (isLimitReached && x.Length == MaxLength)
            {
                Storyboard s = (Storyboard)this.Resources["KeyUpStoryboard"];
                s.Begin();

                isLimitReached = false;
            }
        }
    }
}
