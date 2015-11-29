using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphGenerator.Models
{
    public class CanvasRectangle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int SideLength { get; set; }

        public CanvasRectangle(int x, int y, int sideLength)
        {
            this.X = x;
            this.Y = y;
            this.SideLength = sideLength;
        }
    }
}
