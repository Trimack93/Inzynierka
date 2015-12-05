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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Common.Controls
{
    public class ValidableTextboxObsolete : TextBox
    {
        static ValidableTextboxObsolete()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ValidableTextboxObsolete), new FrameworkPropertyMetadata(typeof(ValidableTextboxObsolete)));
        }

        public bool IsFull { get; set; } = false;

        public string ValidableText
        {
            get { return (string)GetValue(ValidableTextProperty); }
            set { SetValue(ValidableTextProperty, value); }
        }

        //public static readonly DependencyProperty ValidableTextProperty = DependencyProperty.Register(
        //    "ValidableText", typeof(string), typeof(ValidableTextbox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty ValidableTextProperty = DependencyProperty.Register(
            "ValidableText", typeof(string), typeof(ValidableTextboxObsolete), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(ValidableTextPropertyChanged)));

        private static void ValidableTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ValidableTextboxObsolete textBox = (ValidableTextboxObsolete)sender;
            textBox.ValidableText = (string)e.NewValue;
            
            if (textBox.ValidableText.Length == textBox.MaxLength)
                textBox.IsFull = true;
            else
                textBox.IsFull = false;
        }

        public static readonly RoutedEvent PressEvent =
    EventManager.RegisterRoutedEvent("Click", RoutingStrategy.Bubble,
    typeof(RoutedEventHandler), typeof(ValidableTextboxObsolete));

        public event RoutedEventHandler Click
        {
            add { AddHandler(PressEvent, value); }
            remove { RemoveHandler(PressEvent, value); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(PressEvent));
        }
    }
}
