//------------------------------------------
// ArrowLine.cs (c) 2007 by Charles Petzold
//------------------------------------------
using System;
using System.Windows;
using System.Windows.Media;

namespace Common.Controls.Arrowheads
{
    /// <summary>
    ///     Draws a straight line between two points with 
    ///     optional arrows on the ends.
    /// </summary>
    public class ArrowLine : ArrowLineBase
    {
        public double X1
        {
            set { SetValue(X1Property, value); }
            get { return (double)GetValue(X1Property); }
        }
        public double Y1
        {
            set { SetValue(Y1Property, value); }
            get { return (double)GetValue(Y1Property); }
        }
        public double X2
        {
            set { SetValue(X2Property, value); }
            get { return (double)GetValue(X2Property); }
        }
        public double Y2
        {
            set { SetValue(Y2Property, value); }
            get { return (double)GetValue(Y2Property); }
        }

        //---------------------------------

        public static readonly DependencyProperty X1Property = DependencyProperty.Register(
            "X1", typeof(double), typeof(ArrowLine), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty Y1Property = DependencyProperty.Register(
            "Y1", typeof(double), typeof(ArrowLine), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty X2Property = DependencyProperty.Register(
            "X2", typeof(double), typeof(ArrowLine), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty Y2Property = DependencyProperty.Register(
            "Y2", typeof(double), typeof(ArrowLine), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        //---------------------------------

        /// <summary>
        ///     Gets a value that represents the Geometry of the ArrowLine.
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                // Clear out the PathGeometry.
                pathgeo.Figures.Clear();

                // Define a single PathFigure with the points.
                pathfigLine.StartPoint = new Point(X1, Y1);
                polysegLine.Points.Clear();
                polysegLine.Points.Add(new Point(X2, Y2));
                pathgeo.Figures.Add(pathfigLine);

                // Call the base property to add arrows on the ends.
                return base.DefiningGeometry;
            }
        }
    }
}
