using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Models;

namespace GraphGenerator.Models
{
    public class CanvasRectangle
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int SideLength { get; set; }

        public bool DoesContainNode { get; set; } = false;
        public Node Node { get; set; }
        
        public CanvasRectangle(int x, int y, int sideLength)
        {
            this.X = x;
            this.Y = y;
            this.SideLength = sideLength;
        }

        // probably to be deleted
        public int GetGridX { get { return X / SideLength; } }
        public int GetGridY { get { return Y / SideLength; } }
    }
}
