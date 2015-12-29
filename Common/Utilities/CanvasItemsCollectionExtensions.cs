using Common.Models;
using Common.Models.Canvas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public static class CanvasItemsCollectionExtensions
    {
        /// <summary>
        /// Gets all nodes from collection.
        /// </summary>
        /// <param name="canvasItems">Collection with canvas items.</param>
        /// <returns>List of all nodes in collection.</returns>
        public static List<Node> GetAllNodes(this ObservableCollection<CanvasControlBase> canvasItems)
        {
            return canvasItems                 // List of all nodes in a canvas
                .OfType<CanvasRectangle>()
                .Select(rect => rect.Node)
                .Where(n => n != null)
                .OrderBy(k => k.Value.ToString())
                .ToList();
        }

        /// <summary>
        /// Gets all edges from collection.
        /// </summary>
        /// <param name="canvasItems">Collection with canvas items.</param>
        /// <returns>List of all edges in collection.</returns>
        public static List<Edge> GetAllEdges(this ObservableCollection<CanvasControlBase> canvasItems)
        {
            return canvasItems                 // List of all nodes in a canvas
                .OfType<CanvasEdge>()
                .Select(canv => canv.Edge)
                .Where(e => e != null)
                .ToList();
        }

        /// <summary>
        /// Gets nodes connected by provided edge.
        /// </summary>
        /// <param name="canvasItems">Collection with canvas items.</param>
        /// <param name="edge">Edge connecting two nodes.</param>
        /// <returns>List of nodes connected by provided edge.</returns>
        public static List<Node> GetConnectedNodes(this ObservableCollection<CanvasControlBase> canvasItems, Edge edge)
        {
            return canvasItems
                .OfType<CanvasRectangle>()
                .Where( r => r.Node != null )
                .Select( r => r.Node )
                .Where( n => edge.NodesID.Contains(n.ID) )
                .ToList();
        }

        /// <summary>
        /// Gets rectangles containing nodes connected by provided edge.
        /// </summary>
        /// <param name="canvasItems">Collection with canvas items.</param>
        /// <param name="edge">Edge connecting two nodes.</param>
        /// <returns>List of rectangles containing nodes connected by provided edge.</returns>
        public static List<CanvasRectangle> GetConnectedRectangles(this ObservableCollection<CanvasControlBase> canvasItems, Edge edge)
        {
            return canvasItems
                .OfType<CanvasRectangle>()
                .Where( r => r.Node != null )
                .Where( r => edge.NodesID.Contains(r.Node.ID) )
                .ToList();
        }

        /// <summary>
        /// Gets all edges connected to provided node.
        /// </summary>
        /// <param name="canvasItems">Collection with canvas items.</param>
        /// <param name="node">Node whose edges will be searched for.</param>
        /// <returns>List of canvas edges connected to provided node.</returns>
        public static List<CanvasEdge> GetConnectedEdges(this ObservableCollection<CanvasControlBase> canvasItems, Node node)
        {
            return canvasItems
                .OfType<CanvasEdge>()
                .Where( e => node.Edges.Contains(e.Edge) )
                .ToList();
        }

        /// <summary>
        /// Gets the node at the beginning of edge.
        /// </summary>
        /// <param name="canvasItems">Collection with canvas items.</param>
        /// <param name="edge">Edge whose beginning will be searched for.</param>
        /// <returns>Canvas rectangle containing node at the beginning of provided edge.</returns>
        public static CanvasRectangle GetRectangleAtBeginning(this ObservableCollection<CanvasControlBase> canvasItems, Edge edge)
        {
            return canvasItems
                .OfType<CanvasRectangle>()
                .Where(r => r.Node?.ID == edge.NodesID[0])
                .Single();
        }

        /// <summary>
        /// Gets rectangle from canvas by it's node ID.
        /// </summary>
        /// <param name="canvasItems">Collection with canvas items.</param>
        /// <param name="nodeID">ID of node.</param>
        /// <returns>Canvas rectangle containing node with provided ID.</returns>
        public static CanvasRectangle GetRectByNodeId(this ObservableCollection<CanvasControlBase> canvasItems, int nodeID)
        {
            return canvasItems
                .OfType<CanvasRectangle>()
                .Single(r => r.Node?.ID == nodeID);
        }

        /// <summary>
        /// Gets edge from canvas by it's edge ID.
        /// </summary>
        /// <param name="canvasItems">Collection with canvas items.</param>
        /// <param name="edgeID">ID of edge.</param>
        /// <returns>CanvasEdge containing edge with provided ID.</returns>
        public static CanvasEdge GetEdgeById(this ObservableCollection<CanvasControlBase> canvasItems, int edgeID)
        {
            return canvasItems
                .OfType<CanvasEdge>()
                .Single(e => e.Edge.ID == edgeID);
        }

        /// <summary>
        /// Gets node from canvas by it's node ID.
        /// </summary>
        /// <param name="canvasItems">Collection with canvas items.</param>
        /// <param name="nodeID">ID of node.</param>
        /// <returns>Node with provided ID.</returns>
        public static Node GetNodeById(this ObservableCollection<CanvasControlBase> canvasItems, int nodeID)
        {
            return canvasItems
                .OfType<CanvasRectangle>()
                .Single(n => n.Node?.ID == nodeID)
                .Node;
        }

        /// <summary>
        /// Retrieves CanvasEdge object from collection.
        /// </summary>
        /// <param name="canvasItems">Collection with canvas items.</param>
        /// <param name="edge">Edge whose CanvasEdge will be searched for.</param>
        /// <returns>CanvasEdge of provided Edge.</returns>
        public static CanvasEdge GetCanvasEdge(this ObservableCollection<CanvasControlBase> canvasItems, Edge edge)
        {
            return canvasItems
                .OfType<CanvasEdge>()
                .Single(ce => ce.Edge == edge);
        }
    }
}
