using Common.Models;
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApplication1.Models.Algorithms
{
    public class Dijkstra : AlgorithmBase
    {
        public Dijkstra(List<Edge> edges, List<Node> nodes) : base(edges, nodes)
        {
            string filePath = Path.Combine(INSTRUCTIONS_PATH, "Dijkstra.txt");

            this.Instructions = FileHelper.GetInstructionsFromFile(filePath);

            RefreshCorrectLists();
        }

        public override bool Step()
        {
            base.Step();

            List<Node> algorithmNodesList = new List<Node>();                               // List of nodes on which algorithm step will be performed

            // Clone correct nodes from beginning of the step into new list
            foreach (Node node in this.CorrectNodesList)
                algorithmNodesList.Add( node.Clone() );

            List<Node> smallestNodesList = GraphHelper.GetSmallestWhiteNodes(algorithmNodesList);

            // User can pick one and only one node
            if (smallestNodesList.Count == 1)
            {
                Node smallestNode = smallestNodesList[0];

                RelaxNeighbours(algorithmNodesList, smallestNode);

                smallestNode.Color = Brushes.Black;

                if ( CompareWithActualNodes(algorithmNodesList) == false )
                    return false;
            }
            
            // User can pick more than one node
            else if (smallestNodesList.Count > 1)
            {
                List<Node> smallestRealNodesList = GraphHelper.GetHighestBlackNodes(this.NodesList);

                // There should be max one node which exists in both smallestNodesList and smallestRealNodesList.
                Node smallestNode = smallestNodesList
                    .SingleOrDefault(node => smallestRealNodesList
                                        .Exists(n => n.ID == node.ID));

                if (smallestNode != null)
                {
                    RelaxNeighbours(algorithmNodesList, smallestNode);

                    smallestNode.Color = Brushes.Black;

                    if (CompareWithActualNodes(algorithmNodesList) == false)
                        return false;
                }
                // User didn't selected the smallest node
                else
                    return false;
            }
            // Should never happen, but if so, report it as error
            else
                return false;
            
            // If all nodes are processed, finish algorithm
            if ( this.NodesList.All(n => n.Color == Brushes.Black) )
                this.IsFinished = true;

            RefreshCorrectLists();

            return true;
        }

        /// <summary>
        /// Relax neighbour edges of provided node.
        /// </summary>
        /// <param name="algorithmNodesList">Full list of nodes.</param>
        /// <param name="smallestNode">Node which neighbour edges will be relaxed.</param>
        private void RelaxNeighbours(List<Node> algorithmNodesList, Node smallestNode)
        {
            List<Node> smallestNodeNeighbours = GraphHelper.GetConnectedNodes(algorithmNodesList, smallestNode);

            foreach (Node neighbour in smallestNodeNeighbours)
            {
                Edge connectingEdge = GraphHelper.GetConnectingEdge(smallestNode, neighbour);

                int edgeValue = Int32.Parse(connectingEdge.Value);
                int smallestNodeValue = smallestNode.GetIntegerValue();
                int neighbourValue = neighbour.GetIntegerValue();

                if (smallestNodeValue + edgeValue < neighbourValue)
                    neighbour.Value = smallestNodeValue + edgeValue;
            }
        }
    }
}
