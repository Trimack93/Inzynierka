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
    public class NodeControl : Control
    {
        static NodeControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeControl), new FrameworkPropertyMetadata(typeof(NodeControl)));
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

        public static readonly DependencyProperty NameHorizontalAlignmentProperty = DependencyProperty.Register(
            "NameHorizontalAlignment", typeof(HorizontalAlignment), typeof(NodeControl), new UIPropertyMetadata(null));

        public static readonly DependencyProperty NameVerticalAlignmentProperty = DependencyProperty.Register(
            "NameVerticalAlignment", typeof(VerticalAlignment), typeof(NodeControl), new UIPropertyMetadata(null));
    }
}
