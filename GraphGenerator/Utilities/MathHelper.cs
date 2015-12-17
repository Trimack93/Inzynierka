using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraphGenerator.Utilities
{
    /// <summary>
    /// Helper class for mathematical operations.
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Calculates length of segment with provided coordinates.
        /// </summary>
        /// <param name="x1">X1 coordinate of segment.</param>
        /// <param name="y1">Y1 coordinate of segment.</param>
        /// <param name="x2">X2 coordinate of segment.</param>
        /// <param name="y2">Y2 coordinate of segment.</param>
        /// <returns>Calculated length of segment.</returns>
        public static double CalculateSegmentLength(double x1, double y1, double x2, double y2)
        {
            double bracket1 = Math.Pow(x2 - x1, 2);
            double bracket2 = Math.Pow(y2 - y1, 2);

            return Math.Sqrt(bracket1 + bracket2);
        }

        /// <summary>
        /// Gets the angle between the line and X axis.
        /// </summary>
        /// <param name="x1">X-coordinate of the first point which lies on the line.</param>
        /// <param name="y1">Y-coordinate of the first point which lies on the line.</param>
        /// <param name="x2">X-coordinate of the second point which lies on the line.</param>
        /// <param name="y2">Y-coordinate of the second point which lies on the line.</param>
        /// <returns>Calculated angle between the line and X axis (in radians).</returns>
        public static double GetLineAngle(double x1, double y1, double x2, double y2)
        {            
            //---------------------
            // Directional coefficient of the line

            double a = (y2 - y1) / (x2 - x1);

            //---------------------
            // Angle between the line and X axis (in radians)

            return Math.Atan(a);
        }

        /// <summary>
        /// Gets the point on the circle.
        /// </summary>
        /// <param name="x">X coordinate of the circle's center.</param>
        /// <param name="y">Y coordinate of the circle's center.</param>
        /// <param name="angle">Central angle of the circle when the radius is parallel to the X-axis.</param>
        /// <param name="radius">Radius of the circle.</param>
        /// <returns>Coordinates of the point on the circle.</returns>
        public static Point GetPointOnCircle(double x, double y, double angle, double radius)
        {
            double newX = x + Math.Cos(angle) * radius;
            double newY = y + Math.Sin(angle) * radius;

            return new Point(newX, newY);
        }

        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        /// <param name="alfa">Angle in degrees.</param>
        /// <returns>Angle in radians.</returns>
        public static double AlfaToRadian(double alfa)
        {
            return (alfa * Math.PI) / 180.0;
        }
    }
}
