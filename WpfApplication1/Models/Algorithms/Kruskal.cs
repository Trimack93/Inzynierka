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
    public class Kruskal : AlgorithmBase
    {
        public Kruskal(List<Edge> edges, List<Node> nodes) : base(edges, nodes)
        {
            string filePath = Path.Combine(INSTRUCTIONS_PATH, "Kruskal.txt");

            this.Instructions = FileHelper.GetInstructionsFromFile(filePath);

            RefreshCorrectLists();

            foreach (Node node in this.NodesList)
            {
                NodesForest item = new NodesForest(node);
                nodesForest.Add(item);
            }
        }

        //----------------------------------
        
        private List<NodesForest> nodesForest = new List<NodesForest>();

        //----------------------------------

        public override bool Step()
        {
            base.Step();
            
            // If all nodes belong to the same forest and user didn't changed anything in last sequence, finish algorithm
            int forestID = this.nodesForest[0].ID;
            bool areAllNodesFromOneForest = this.nodesForest.All(f => f.ID == forestID);
            bool userChangedNothing = CompareWithActualEdges(this.CorrectEdgesList);

            if (areAllNodesFromOneForest && userChangedNothing)
            {
                this.IsFinished = true;
                return true;
            }

            List<Edge> algorithmEdgesList = new List<Edge>();                               // List of edges on which algorithm step will be performed
            Edge shortestCorrectEdge = null;

            // Clone correct nodes from beginning of the step into new list
            foreach (Edge edge in this.CorrectEdgesList)
                algorithmEdgesList.Add( edge.Clone() );

            List<Edge> sortedEdgesList = (from edge in algorithmEdgesList
                                          where edge.Color.Color == Colors.Black
                                          orderby edge.GetIntegerValue()
                                          select edge).ToList();

            List<Edge> shortestBlackEdgesList = GetShortestEdges(sortedEdgesList);

            if (shortestBlackEdgesList.Count == 1)
            {
                shortestCorrectEdge = shortestBlackEdgesList[0];
                shortestCorrectEdge.Color = Brushes.LimeGreen;
            }

            else
            {
                List<Edge> longestRealGreenEdgesList = GetLongestEdges(this.EdgesList);
                
                // There should be max one edge which exists in both shortestBlackEdgesList and longestRealGreenEdgesList.
                List<Edge> shortestCorrectEdges = shortestBlackEdgesList
                    .Where(edge => longestRealGreenEdgesList
                                        .Exists(e => e.ID == edge.ID))
                    .ToList();

                if (shortestCorrectEdges.Count != 1)
                    return false;
                
                shortestCorrectEdge = shortestCorrectEdges[0];
                shortestCorrectEdge.Color = Brushes.LimeGreen;
            }

            if (CompareWithActualEdges(algorithmEdgesList) == false)
                return false;

            MergeForests(shortestCorrectEdge.NodesID[0], shortestCorrectEdge.NodesID[1]);

            RefreshCorrectLists();

            return true;
        }

        /// <summary>
        /// Gets list of longest green edges.
        /// </summary>
        /// <param name="edgesList">List of edges.</param>
        /// <returns>List of longest green edges.</returns>
        private List<Edge> GetLongestEdges(List<Edge> edgesList)
        {
            List<int> greenEdgesValues = edgesList
                .Where(e => e.Color == Brushes.LimeGreen)
                .Select(e => e.GetIntegerValue())
                .ToList();

            // If there are no green edges in graph, return empty list
            if (greenEdgesValues.Count == 0)
                return new List<Edge>();

            int maxValue = greenEdgesValues.Max();

            return edgesList
                .Where(e => e.GetIntegerValue() == maxValue)
                .Where(e => e.Color == Brushes.LimeGreen)
                .ToList();
        }

        /// <summary>
        /// Gets list of shortest black edges connecting nodes from different forests.
        /// </summary>
        /// <param name="edgesList">List of edges.</param>
        /// <returns>List of shortest black edges.</returns>
        private List<Edge> GetShortestEdges(List<Edge> edgesList)
        {
            List<Edge> shortestEdges = new List<Edge>();
            bool foundEdges = false;

            for (int i = 0; i < edgesList.Count; i++)
            {
                int minValue = edgesList[i].GetIntegerValue();                        // First edge in sorted list is always the smallest one

                List<Edge> possibleShortestEdges = edgesList                          // If there are more than 1 edge with smallest value
                    .Where(e => e.GetIntegerValue() == minValue)
                    .ToList();

                foreach(Edge edge in possibleShortestEdges)
                {
                    // If nodes this edge is connecting are from different forests
                    if ( CheckForestAffiliation(edge.NodesID[0], edge.NodesID[1]) == false )
                    {
                        shortestEdges.Add(edge);
                        foundEdges = true;
                    }
                }

                if (foundEdges)
                    break;
            }

            return shortestEdges;
        }

        /// <summary>
        /// Checks if provided nodes are members of the same forest.
        /// </summary>
        /// <param name="firstNodeID">First node to compare.</param>
        /// <param name="secondNodeID">Second node to compare.</param>
        /// <returns>True, if nodes are from the same forest, otherwise false.</returns>
        private bool CheckForestAffiliation(int firstNodeID, int secondNodeID)
        {
            int firstNodeForestID = nodesForest
                .Where(f => f.Node.ID == firstNodeID)
                .Select(f => f.ID)
                .Single();

            int secondNodeForestID = nodesForest
                .Where(f => f.Node.ID == secondNodeID)
                .Select(f => f.ID)
                .Single();

            return firstNodeForestID == secondNodeForestID;
        }

        /// <summary>
        /// Merges forests of provided nodes.
        /// </summary>
        /// <param name="firstNodeID">First node ID.</param>
        /// <param name="secondNodeID">Second node ID.</param>
        private void MergeForests(int firstNodeID, int secondNodeID)
        {
            int firstForestID = this.nodesForest
                .Where(f => f.Node.ID == firstNodeID)
                .Select(f => f.ID)
                .Single();

            int secondForestID = this.nodesForest
                .Where(f => f.Node.ID == secondNodeID)
                .Select(f => f.ID)
                .Single();

            List<NodesForest> firstForest = this.nodesForest
                .Where(f => f.ID == firstForestID)
                .ToList();

            List<NodesForest> secondForest = this.nodesForest
                .Where(f => f.ID == secondForestID)
                .ToList();

            // Move elements from first forest to the second
            if (firstForest.Count < secondForest.Count)
            {
                this.nodesForest
                    .Where(f => f.ID == firstForestID)
                    .ToList()
                    .ForEach(f => f.ID = secondForestID);
            }
            // Move elements from second forest to the first
            else
            {
                this.nodesForest
                    .Where(f => f.ID == secondForestID)
                    .ToList()
                    .ForEach(f => f.ID = firstForestID);
            }
        }
        
        /// <summary>
        /// Compares edges in the list against actual edges in the graph.
        /// </summary>
        /// <returns>True, if edges are the same, otherwise false.</returns>
        private bool CompareWithActualEdges(List<Edge> algorithmEdgesList)
        {
            foreach (Edge edge in this.EdgesList)
            {
                Edge correctEdge = algorithmEdgesList.Single(e => e.ID == edge.ID);

                if (edge.Value != correctEdge.Value || edge.Color.Color != correctEdge.Color.Color)
                    return false;
            }

            return true;
        }
    }
}
