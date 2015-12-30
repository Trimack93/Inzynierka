using Common.Models;
using Common.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

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

        /// <summary>
        /// Gets nodes to which this node is directed to.
        /// </summary>
        /// <param name="nodesList">List of nodes.</param>
        /// <param name="node">Node which neighbours are being searched.</param>
        /// <returns>List of connected nodes.</returns>
        public static List<Node> GetConnectedNodes(List<Node> nodesList, Node node)
        {
            List<Node> result = new List<Node>();
            List<Edge> connectedEdges = node.Edges;

            foreach(Edge edge in connectedEdges)
            {
                int secondNodeID = 0;                                               // ID of second node connected by this edge

                // If edge is bidirectional, second node ID can be anywhere.
                // But if it is directed, second node is ALWAYS in the [1]
                if (edge.IsBidirectional)
                    secondNodeID = edge.NodesID.Single(a => a != node.ID);
                else
                {
                    // If edge is pointing at this node, skip it
                    if (edge.NodesID[1] == node.ID)
                        continue;

                    secondNodeID = edge.NodesID[1];
                }

                Node secondNode = nodesList.Single(n => n.ID == secondNodeID);
                result.Add(secondNode);
            }

            return result;
        }

        /// <summary>
        /// Gets nodes that are directed into provided node.
        /// </summary>
        /// <param name="nodesList">List of nodes.</param>
        /// <param name="currentNode">Node which precedessors are being searched for.</param>
        /// <returns>List of nodes directed into provided node.</returns>
        public static List<Node> GetPreviousNodes(List<Node> nodesList, Node currentNode)
        {
            List<Node> result = new List<Node>();
            List<Edge> connectedEdges = currentNode.Edges;

            foreach (Edge edge in connectedEdges)
            {
                int secondNodeID = 0;                                               // ID of second node connected by this edge

                // If edge is bidirectional, second node ID can be anywhere.
                // But if it is directed, beginning of the edge is ALWAYS in the [0]
                if (edge.IsBidirectional)
                    secondNodeID = edge.NodesID.Single(a => a != currentNode.ID);
                else
                {
                    if (edge.NodesID[0] == currentNode.ID)
                        continue;

                    secondNodeID = edge.NodesID[0];
                }

                Node secondNode = nodesList.Single(n => n.ID == secondNodeID);
                result.Add(secondNode);
            }

            return result;
        }

        /// <summary>
        /// Gets gray node with highest step ("x/" or "x/y") directed into provided node.
        /// </summary>
        /// <param name="nodesList">List of nodes</param>
        /// <param name="currentNode">Node which precedessor is being searched for.</param>
        /// <returns>Gray node with highest step directed into provided node.</returns>
        public static Node GetPreviousGrayNodeWithHighestSeq(List<Node> nodesList, Node currentNode)
        {
            List<Node> previousGrayNodes = GraphHelper.GetPreviousNodes(nodesList, currentNode)
                                                    .Where(n => n.Color == Brushes.Gray)
                                                    .ToList();

            return GraphHelper.GetNodeWithHighestStep(previousGrayNodes);
        }

        /// <summary>
        /// Gets node with highest step ("x/" or "x/y") from list.
        /// </summary>
        /// <param name="nodesList">List of nodes.</param>
        /// <returns>Node with highest step.</returns>
        public static Node GetNodeWithHighestStep(List<Node> nodesList)
        {
            List<string> nodeValues = nodesList
                .Where(n => n.Value.ToString() != "")
                .Select(n => n.Value.ToString())
                .ToList();

            try
            {                
                List<int> leftValues = new List<int>(nodeValues.Count);
                List<int> rightValues = new List<int>(nodeValues.Count);

                for (int i = 0; i < nodeValues.Count; i++)
                {
                    int slashIndex = nodeValues[i].IndexOf('/');
                    int count = nodeValues[i].Length - slashIndex;                      // Number of characters to delete

                    string leftValue = nodeValues[i].Remove(slashIndex, count);
                    string rightValue = nodeValues[i].Remove(0, slashIndex + 1);

                    leftValues.Add(leftValue != "" ?
                        Int32.Parse(leftValue) : 0);

                    rightValues.Add(rightValue != "" ?
                        Int32.Parse(rightValue) : 0);
                }

                int maxValueLeft = leftValues.Max();
                int maxValueRight = rightValues.Max();

                string maxValue = maxValueLeft > maxValueRight ?
                    maxValueLeft.ToString() : maxValueRight.ToString();

                if (maxValueLeft > maxValueRight)
                {
                    return nodesList
                        .Single( n => n.Value.ToString().StartsWith(maxValue) );
                }
                else
                {
                    return nodesList
                        .Single( n => n.Value.ToString().EndsWith(maxValue) );
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Sequence contains more than one matching element")
                    throw new MultipleNodesWithHighestStepException();
            }

            return null;
        }

        /// <summary>
        /// Gets list of nodes' processed time values.
        /// </summary>
        /// <param name="nodesList">List of nodes.</param>
        /// <returns>List of nodes' processed time values.</returns>
        public static List<int> GetListOfNodesProcessedTimes(List<Node> nodesList)
        {
            List<string> nodeValues = nodesList
                .Where( n => n != null )
                .Where( n => n.Value.ToString() != "" )
                .Select( n => n.Value.ToString() )
                .ToList();

            List<int> rightValues = new List<int>(nodeValues.Count);

            for (int i = 0; i < nodeValues.Count; i++)
            {
                int slashIndex = nodeValues[i].IndexOf('/');
                int count = nodeValues[i].Length - slashIndex;                      // Number of characters to delete
                
                string rightValue = nodeValues[i].Remove(0, slashIndex + 1);

                rightValues.Add(rightValue != "" ?
                    Int32.Parse(rightValue) : 0);
            }

            return rightValues;
        }

        /// <summary>
        /// Increases node's visit time ("x/" -> "x+1/").
        /// </summary>
        /// <param name="node">Node to change.</param>
        public static void IncreaseNodeVisitTime(Node node)
        {
            string nodeValue = node.Value.ToString();

            nodeValue = nodeValue.Replace("/", "");

            int value = Int32.Parse(nodeValue);
            value++;

            node.Value = value.ToString();
        }

        /// <summary>
        /// Sets node visited time ("" -> "stepCount/").
        /// </summary>
        /// <param name="node">Node to change.</param>
        /// <param name="stepCount">Visited time value.</param>
        public static void SetNodeVisitedTime(Node node, int stepCount)
        {
            node.Value = stepCount.ToString() + "/";
        }

        /// <summary>
        /// Sets node processed time ("x/" -> "x/stepCount").
        /// </summary>
        /// <param name="node">Node to change.</param>
        /// <param name="stepCount">Processed time value.</param>
        public static void SetNodeProcessedTime(Node node, int stepCount)
        {
            string newNodeValue = node.Value.ToString();
            newNodeValue += stepCount.ToString();

            node.Value = newNodeValue;
        }
    }
}
