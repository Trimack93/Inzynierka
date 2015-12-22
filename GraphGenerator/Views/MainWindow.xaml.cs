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

using GraphGenerator.ViewModels;

namespace GraphGenerator.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Canvas canvas = this.FindName("DrawingCanvas") as Canvas;
        }

        // change to native WPF commands with event args?
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double x = e.GetPosition(this.CanvasItemsControl).X;
                double y = e.GetPosition(this.CanvasItemsControl).Y;

                (this.DataContext as MainViewModel).AddEdge(x, y);
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double x = e.GetPosition(this.CanvasItemsControl).X;
                double y = e.GetPosition(this.CanvasItemsControl).Y;

                (this.DataContext as MainViewModel).UpdateEdgePosition(x, y);
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            double x = e.GetPosition(this.CanvasItemsControl).X;
            double y = e.GetPosition(this.CanvasItemsControl).Y;

            (this.DataContext as MainViewModel).AddEdgeFinish(x, y);
        }
    }
}
