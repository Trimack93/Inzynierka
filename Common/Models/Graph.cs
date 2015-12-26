using Common.Models.Canvas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Common.Models
{
    [XmlInclude( typeof(CanvasRectangle) )]
    [XmlInclude( typeof(CanvasEdge) )]
    public class Graph
    {
        /// <summary>
        /// Collection of nodes and edges.
        /// </summary>
        public ObservableCollection<CanvasControlBase> CanvasGraph { get; set; }

        /// <summary>
        /// "True" on specific position means that this algorithm is supported (order as in Graph Compatibility GroupBox).
        /// </summary>
        public bool[] AlgorithmsSupported { get; set; } = new bool[7];
    }
}
