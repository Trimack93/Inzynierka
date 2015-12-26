using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Canvas
{
    /// <summary>
    /// Provides basic properties which define object that can be placed inside drawing canvas.
    /// </summary>
    public abstract class CanvasControlBase : BaseNotifyPropertyChanged
    {
        private double _CanvasLeft;
        private double _CanvasTop;

        //---------------------------------
        
        public double CanvasLeft
        {
            get { return _CanvasLeft; }
            set
            {
                _CanvasLeft = value;
                RaisePropertyChanged("CanvasLeft");
            }
        }
        public double CanvasTop
        {
            get { return _CanvasTop; }
            set
            {
                _CanvasTop = value;
                RaisePropertyChanged("CanvasTop");
            }
        }

        //---------------------------------

        protected CanvasControlBase(double x, double y)
        {
            this.CanvasLeft = x;
            this.CanvasTop = y;
        }

        protected CanvasControlBase()
        {
            this.CanvasLeft = 0;
            this.CanvasTop = 0;
        }
    }
}
