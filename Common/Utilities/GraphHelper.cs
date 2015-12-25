using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    /// <summary>
    /// Helper for graph-associated operations.
    /// </summary>
    public static class GraphHelper
    {
        /// <summary>
        /// Gets list of edges between two nodes.
        /// </summary>
        /// <param name="node1">First node.</param>
        /// <param name="node2">Second node.</param>
        /// <returns>List of edges between provided nodes.</returns>
        public static List<Edge> GetListOfEdges(Node node1, Node node2)
        {
            return node1.Edges
                .Where( e => e.NodesID.Contains(node2.ID) )
                .ToList();
        }
    }
}
