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
    /// <summary>
    /// Interaction logic for NodeControl.xaml
    /// </summary>
    public partial class NodeControl : UserControl
    {
        public NodeControl()
        {
            InitializeComponent();
        }

        public string NodeName
        {
            get { return (string)GetValue(NodeNameProperty); }
            set { SetValue(NodeNameProperty, value); }
        }
        public string NodeValue
        {
            get { return (string)GetValue(NodeValueProperty); }
            set { SetValue(NodeValueProperty, value); }
        }

        public SolidColorBrush NodeColor
        {
            get { return (SolidColorBrush)GetValue(NodeColorProperty); }
            set { SetValue(NodeColorProperty, value); }
        }
        public double NodeThickness
        {
            get { return (double)GetValue(NodeThicknessProperty); }
            set { SetValue(NodeThicknessProperty, value); }
        }

        public HorizontalAlignment NameHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(NameHorizontalAlignmentProperty); }
            set { SetValue(NameHorizontalAlignmentProperty, value); }
        }
        public VerticalAlignment NameVerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(NameVerticalAlignmentProperty); }
            set { SetValue(NameVerticalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty NodeNameProperty = DependencyProperty.Register(
            "NodeName", typeof(string), typeof(NodeControl), new UIPropertyMetadata(null));

        public static readonly DependencyProperty NodeValueProperty = DependencyProperty.Register(
            "NodeValue", typeof(string), typeof(NodeControl), new UIPropertyMetadata(null));

        public static readonly DependencyProperty NodeColorProperty = DependencyProperty.Register(
            "NodeColor", typeof(SolidColorBrush), typeof(NodeControl), new UIPropertyMetadata(null));

        public static readonly DependencyProperty NodeThicknessProperty = DependencyProperty.Register(
            "NodeThickness", typeof(double), typeof(NodeControl), new UIPropertyMetadata(null));

        public static readonly DependencyProperty NameHorizontalAlignmentProperty = DependencyProperty.Register(
            "NameHorizontalAlignment", typeof(HorizontalAlignment), typeof(NodeControl), new UIPropertyMetadata(null));

        public static readonly DependencyProperty NameVerticalAlignmentProperty = DependencyProperty.Register(
            "NameVerticalAlignment", typeof(VerticalAlignment), typeof(NodeControl), new UIPropertyMetadata(null));

        private void TextBlock_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            
            if (textBlock.Text.Length <= 3)
                textBlock.FontSize = 14;
            else if (textBlock.Text.Length == 4)
                textBlock.FontSize = 12;
            else
                textBlock.FontSize = 10;
        }
    }
}
