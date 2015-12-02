using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Models;
using Common.Utilities;

namespace GraphGenerator.Models
{
    public class CanvasRectangle : BaseNotifyPropertyChanged
    {
        private int _ID;

        private int _X;
        private int _Y;
        private int _SideLength;

        private bool _DoesContainNode = false;
        private Node _Node;

        //---------------------------------

        public int ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                RaisePropertyChanged("ID");
            }
        }
        public int X
        {
            get { return _X; }
            set
            {
                _X = value;
                RaisePropertyChanged("X");
            }
        }
        public int Y
        {
            get { return _Y; }
            set
            {
                _Y = value;
                RaisePropertyChanged("Y");
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

        public CanvasRectangle(int x, int y, int sideLength, int ID)
        {
            this.X = x;
            this.Y = y;
            this.SideLength = sideLength;
            this.ID = ID;
        }

        // probably to be deleted
        //public int GetGridX { get { return X / SideLength; } }
        //public int GetGridY { get { return Y / SideLength; } }
    }
}
