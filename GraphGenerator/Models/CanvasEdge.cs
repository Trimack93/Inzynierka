using Common.Models;
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphGenerator.Models
{
    public class CanvasEdge : CanvasControlBase
    {
        private double _X1;
        private double _Y1;
        private double _X2;
        private double _Y2;

        private Edge _Edge = new Edge();

        //---------------------------------

        public double X1
        {
            get { return _X1; }
            set
            {
                _X1 = value;
                RaisePropertyChanged("X1");
            }
        }
        public double Y1
        {
            get { return _Y1; }
            set
            {
                _Y1 = value;
                RaisePropertyChanged("Y1");
            }
        }
        public double X2
        {
            get { return _X2; }
            set
            {
                _X2 = value;
                RaisePropertyChanged("X2");
            }
        }
        public double Y2
        {
            get { return _Y2; }
            set
            {
                _Y2 = value;
                RaisePropertyChanged("Y2");
            }
        }

        public Edge Edge
        {
            get { return _Edge; }
            set
            {
                _Edge = value;
                RaisePropertyChanged("Edge");
            }
        }

        //---------------------------------
        
        public CanvasEdge(int x1, int y1, int x2, int y2, Edge ec) : base(0, 0)         // Arrows' coordinates must be relative to S=(0,0) point on canvas
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.X2 = x2;
            this.Y2 = y2;

            this.Edge = ec;
        }

        // probably to be deleted
        //public int GetGridX { get { return X / SideLength; } }
        //public int GetGridY { get { return Y / SideLength; } }
    }
}
