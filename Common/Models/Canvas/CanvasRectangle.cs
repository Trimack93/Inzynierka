using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Models;
using Common.Utilities;

namespace Common.Models.Canvas
{
    public class CanvasRectangle : CanvasControlBase
    {
        private int _RectangleID;
        private int _SideLength;

        private bool _DoesContainNode = false;
        private Node _Node;

        //---------------------------------

        public int RectangleID
        {
            get { return _RectangleID; }
            set
            {
                _RectangleID = value;
                RaisePropertyChanged("RectangleID");
            }
        }
        public int SideLength
        {
            get { return _SideLength; }
            set
            {
                _SideLength = value;
                RaisePropertyChanged("SideLength");
            }
        }

        public bool DoesContainNode
        {
            get { return _DoesContainNode; }
            set
            {
                _DoesContainNode = value;
                RaisePropertyChanged("DoesContainNode");
            }
        }
        public Node Node
        {
            get { return _Node; }
            set
            {
                _Node = value;
                RaisePropertyChanged("Node");
            }
        }

        //---------------------------------

        public CanvasRectangle(int ID, int x, int y, int sideLength) : base(x, y)
        {
            this.RectangleID = ID;
            this.SideLength = sideLength;
        }
    }
}
