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

        public static List<Node> GetConnectedNodes(List<Node> nodesList, Node node)
        {
            List<Node> result = new List<Node>();

            List<Edge> connectedEdges = node.Edges;

            foreach(Edge edge in connectedEdges)
            {
                int secondNodeID = 0;                                               // ID of second node connected by this edge
                //int secondNodeID = edge.NodesID.Single(a => a != node.ID);                // ID of second node connected by this edge
                //int secondNodeID = edge.NodesID[1];

                // If edge is bidirectional, second node ID can be anywhere.
                // But if it is directed, second node is ALWAYS in the [1]
                if (edge.IsBidirectional)
                    secondNodeID = edge.NodesID.Single(a => a != node.ID);
                else
                    secondNodeID = edge.NodesID[1];

                Node secondNode = nodesList.Single(n => n.ID == secondNodeID);
                result.Add(secondNode);
            }

            return result;
        }
    }
}
