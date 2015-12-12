﻿//----------------------------------------------
// ArrowLineBase.cs (c) 2007 by Charles Petzold
//----------------------------------------------
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Common.Controls.Arrowheads
{
    /// <summary>
    ///     Indicates which end of the line has an arrow.
    /// </summary>
    [Flags]
    public enum ArrowEnds
    {
        None = 0,
        Start = 1,
        End = 2,
        Both = 3
    }

    /// <summary>
    ///     Provides a base class for ArrowLine and ArrowPolyline.
    ///     This class is abstract.
    /// </summary>
    public abstract class ArrowLineBase : Shape
    {
        protected PathGeometry pathgeo;
        protected PathFigure pathfigLine;
        protected PolyLineSegment polysegLine;

        PathFigure pathfigHead1;
        PolyLineSegment polysegHead1;
        PathFigure pathfigHead2;
        PolyLineSegment polysegHead2;

        //---------------------------------

        /// <summary>
        ///     Gets or sets the angle between the two sides of the arrowhead.
        /// </summary>
        public double ArrowAngle
        {
            set { SetValue(ArrowAngleProperty, value); }
            get { return (double)GetValue(ArrowAngleProperty); }
        }

        /// <summary>
        ///     Gets or sets the length of the two sides of the arrowhead.
        /// </summary>
        public double ArrowLength
        {
            set { SetValue(ArrowLengthProperty, value); }
            get { return (double)GetValue(ArrowLengthProperty); }
        }

        /// <summary>
        ///     Gets or sets the property that determines which ends of the
        ///     line have arrows.
        /// </summary>
        public ArrowEnds ArrowEnds
        {
            set { SetValue(ArrowEndsProperty, value); }
            get { return (ArrowEnds)GetValue(ArrowEndsProperty); }
        }

        /// <summary>
        ///     Gets or sets the property that determines if the arrow head
        ///     is closed to resemble a triangle.
        /// </summary>
        public bool IsArrowClosed
        {
            set { SetValue(IsArrowClosedProperty, value); }
            get { return (bool)GetValue(IsArrowClosedProperty); }
        }

        //---------------------------------

        public static readonly DependencyProperty ArrowAngleProperty = DependencyProperty.Register(
            "ArrowAngle", typeof(double), typeof(ArrowLineBase), new FrameworkPropertyMetadata(45.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ArrowLengthProperty = DependencyProperty.Register(
            "ArrowLength", typeof(double), typeof(ArrowLineBase), new FrameworkPropertyMetadata(12.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty ArrowEndsProperty = DependencyProperty.Register(
            "ArrowEnds", typeof(ArrowEnds), typeof(ArrowLineBase), new FrameworkPropertyMetadata(ArrowEnds.End, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public static readonly DependencyProperty IsArrowClosedProperty = DependencyProperty.Register(
            "IsArrowClosed", typeof(bool), typeof(ArrowLineBase), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsMeasure));

        //---------------------------------

        public ArrowLineBase()
        {
            pathgeo = new PathGeometry();

            pathfigLine = new PathFigure();
            polysegLine = new PolyLineSegment();
            pathfigLine.Segments.Add(polysegLine);

            pathfigHead1 = new PathFigure();
            polysegHead1 = new PolyLineSegment();
            pathfigHead1.Segments.Add(polysegHead1);

            pathfigHead2 = new PathFigure();
            polysegHead2 = new PolyLineSegment();
            pathfigHead2.Segments.Add(polysegHead2);
        }

        //---------------------------------

        /// <summary>
        ///     Gets a value that represents the Geometry of the ArrowLine.
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                int count = polysegLine.Points.Count;

                if (count > 0)
                {
                    // Draw the arrow at the start of the line.
                    if ((ArrowEnds & ArrowEnds.Start) == ArrowEnds.Start)
                    {
                        Point pt1 = pathfigLine.StartPoint;
                        Point pt2 = polysegLine.Points[0];
                        pathgeo.Figures.Add(CalculateArrow(pathfigHead1, pt2, pt1));
                    }

                    // Draw the arrow at the end of the line.
                    if ((ArrowEnds & ArrowEnds.End) == ArrowEnds.End)
                    {
                        Point pt1 = count == 1 ? pathfigLine.StartPoint :
                                                 polysegLine.Points[count - 2];
                        Point pt2 = polysegLine.Points[count - 1];
                        pathgeo.Figures.Add(CalculateArrow(pathfigHead2, pt1, pt2));
                    }
                }
                return pathgeo;
            }
        }

        PathFigure CalculateArrow(PathFigure pathfig, Point pt1, Point pt2)
        {
            Matrix matx = new Matrix();
            Vector vect = pt1 - pt2;
            vect.Normalize();
            vect *= ArrowLength;

            PolyLineSegment polyseg = pathfig.Segments[0] as PolyLineSegment;
            polyseg.Points.Clear();
            matx.Rotate(ArrowAngle / 2);
            pathfig.StartPoint = pt2 + vect * matx;
            polyseg.Points.Add(pt2);

            matx.Rotate(-ArrowAngle);
            polyseg.Points.Add(pt2 + vect * matx);
            pathfig.IsClosed = IsArrowClosed;

            return pathfig;
        }
    }
}
