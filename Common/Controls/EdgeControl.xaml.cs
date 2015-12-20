using Common.Models;
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
    /// Interaction logic for EdgeControl.xaml
    /// </summary>
    public partial class EdgeControl : UserControl
    {
        public EdgeControl()
        {
            InitializeComponent();

            // TODO: Change to triggers
            this.MouseEnter += (sender, e) =>
            {
                if (Edge.Value != null)
                {
                    this.Edge.Thickness = 3;
                    this.ValueTextblock.FontWeight = FontWeights.Bold;
                }
            };

            this.MouseLeave += (sender, e) =>
            {
                if (Edge.Value != null)
                {
                    this.Edge.Thickness = 2;
                    this.ValueTextblock.FontWeight = FontWeights.Normal;
                }
            };
        }

        public Edge Edge
        {
            get { return (Edge)GetValue(EdgeProperty); }
            set { SetValue(EdgeProperty, value); }
        }

        public double X1
        {
            get { return (double)GetValue(X1Property); }
            set { SetValue(X1Property, value); }
        }
        public double Y1
        {
            get { return (double)GetValue(Y1Property); }
            set { SetValue(Y1Property, value); }
        }
        public double X2
        {
            get { return (double)GetValue(X2Property); }
            set { SetValue(X2Property, value); }
        }
        public double Y2
        {
            get { return (double)GetValue(Y2Property); }
            set { SetValue(Y2Property, value); }
        }

        //---------------------------------

        public static readonly DependencyProperty EdgeProperty = DependencyProperty.Register(
            "Edge", typeof(Edge), typeof(EdgeControl), new UIPropertyMetadata(null));

        public static readonly DependencyProperty X1Property = DependencyProperty.Register(
            "X1", typeof(double), typeof(EdgeControl), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty Y1Property = DependencyProperty.Register(
            "Y1", typeof(double), typeof(EdgeControl), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty X2Property = DependencyProperty.Register(
            "X2", typeof(double), typeof(EdgeControl), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty Y2Property = DependencyProperty.Register(
            "Y2", typeof(double), typeof(EdgeControl), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        //---------------------------------

        // Metoda wywoływana zawsze wtedy, gdy wartość w bindingu Edge.Value się zmieni
        private void ValueTextblock_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            SetTextblockPosition();
        }

        private void SetTextblockPosition()
        {
            double Y2tmp = Y1 + (Y1 - Y2);                              // korekta na (*&^& Canvas

            //---------------------
            // Długość strzałki

            double nawias1 = Math.Pow(X2 - X1, 2);
            double nawias2 = Math.Pow(Y2tmp - Y1, 2);
            double length = Math.Sqrt(nawias1 + nawias2);

            //---------------------
            // Współczynnik kierunkowy narysowanego odcinka

            double a = (Y2tmp - Y1) / (X2 - X1);

            //---------------------
            // Kąt nachylenia strzałki do osi X

            double alfaRadian = Math.Atan(a);
            double alfa = alfaRadian * 180 / Math.PI;

            //---------------------
            // Promień figury okręgopodobnej, po której porusza się TextBlock podczas obracania strzałki

            double textBlockCircleRadius = (length / 2.0);
            double left = 0, top = 0;

            //---------------------
            // http://i1.memy.pl/big/c65d3c_140031762003.59.jpg

            if (X1 <= X2)
            {
                left = X1 + Math.Cos(alfaRadian) * textBlockCircleRadius + 10 * Math.Sin(-alfaRadian);
                top = Y1 + Math.Sin(-alfaRadian) * textBlockCircleRadius - 20 + 10 * Math.Sin(-alfaRadian);

                if (Y1 <= Y2tmp)
                {
                    left = X1 + Math.Cos(alfaRadian) * textBlockCircleRadius + 20 * Math.Sin(-alfaRadian);
                    top = Y1 + Math.Sin(-alfaRadian) * textBlockCircleRadius - 20 + 10 * Math.Sin(alfaRadian);
                }
            }

            else if (X1 >= X2)
            {
                left = X2 + Math.Cos(alfaRadian) * textBlockCircleRadius + 10 * Math.Sin(alfaRadian);
                top = Y2 + Math.Sin(-alfaRadian) * textBlockCircleRadius - 10 + 10 * Math.Cos(-alfaRadian);

                if (Y1 <= Y2tmp)
                {
                    left = X2 + Math.Cos(alfaRadian) * textBlockCircleRadius + 20 * Math.Sin(alfaRadian);
                    top = Y2 + Math.Sin(-alfaRadian) * textBlockCircleRadius - 10 + 10 * Math.Cos(alfaRadian);
                }
            }

            //---------------------
            // Ustawienie miejsca TextBlock'a

            Canvas.SetLeft(this.ValueTextblock, left);
            Canvas.SetTop(this.ValueTextblock, top);

        }
    }
}
