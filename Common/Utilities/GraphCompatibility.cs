using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Models.Canvas;
using System.Collections.ObjectModel;

namespace Common.Utilities
{
    /// <summary>
    /// Class for checking graph compatibility with various algorithms.
    /// </summary>
    public class GraphCompatibility
    {
        private ObservableCollection<CanvasControlBase> canvasRectangles;

        public GraphCompatibility(ObservableCollection<CanvasControlBase> canvasRectangles)
        {
            this.canvasRectangles = canvasRectangles;
        }

        private bool IsGraphDirected()
        {
            return true;
        }
    }
}
