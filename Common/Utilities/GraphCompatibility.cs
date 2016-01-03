using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Models.Canvas;
using System.Collections.ObjectModel;
using Common.Models;

// TODO: sprawdzanie acykliczności grafu (do sortowania topologicznego)
namespace Common.Utilities
{
    /// <summary>
    /// Class for checking graph compatibility with various algorithms.
    /// </summary>
    public class GraphCompatibility
    {
        private ObservableCollection<CanvasControlBase> _canvasItems;
        private List<Edge> edgesList;
        private List<Node> nodesList;

        public GraphCompatibility(ObservableCollection<CanvasControlBase> canvasItems)
        {
            _canvasItems = canvasItems;
        }

        // Call before every use of class
        public void Init()
        {
            edgesList = _canvasItems.GetAllEdges();
            nodesList = _canvasItems.GetAllNodes();
        }

        //-------------------------
        // Requirements common for every algorithm

        public bool AreBasicRequirementsMet()
        {
            return IsAtLeastOneEdgeTwoNodes() && AreAllNodesConnected() && AreEdgesOfOneType();
        }

        private bool IsAtLeastOneEdgeTwoNodes()
        {
            return edgesList.Count >= 1 && nodesList.Count >= 2;
        }

        private bool AreAllNodesConnected()
        {
            return nodesList.All(n => n.Edges.Count > 0);
        }

        private bool AreEdgesOfOneType()
        {
            if (edgesList.Count > 0)
            {
                bool pattern = edgesList[0].IsBidirectional;

                return edgesList.All(e => e.IsBidirectional == pattern);
            }

            return false;
        }

        //-------------------------
        
        // Kruskal
        public bool AreNodesWithCharValuesOnly()
        {
            return nodesList.All(n => n.Name == "" &&
                n.Value.ToString().All(Char.IsLetter) );
        }

        // Przeszukiwanie wszerz
        public bool AreNodesWithNames()
        {
            return nodesList.All(n => n.Name.Length > 0);
        }

        // Sortowanie topologiczne, Dijkstra, Bellman-Ford = true, reszta false
        public bool IsGraphDirected()
        {
            if (edgesList.Count > 0)
                return edgesList.All(e => e.IsBidirectional == false);

            return false;
        }

        // Przeszukiwanie wszerz / w głąb, sortowanie topologiczne, wykrywanie dwudzielności
        public bool AreEdgesEmpty()
        {
            return edgesList.All(e => e.Value == "");
        }

        // Bellman-Ford
        public bool AreEdgesWithIntValues()
        {
            try
            {
                int dummy;

                if (edgesList.Count > 0)
                    return edgesList.All(e => Int32.TryParse(e.Value, out dummy));
            }
            catch (Exception) { }

            return false;
        }

        // Kruskal, Dijkstra
        public bool AreEdgesNonNegative()
        {
            try
            {
                if (edgesList.Count > 0)
                    return edgesList.All(e => Int32.Parse(e.Value) >= 0);
            }
            catch (Exception) { }

            return false;
        }

        // przeszukiwanie wszerz, Dijkstra, Bellman-Ford
        public bool AreNodesZeroAndInfinites()
        {
            try
            {
                if (nodesList.Count > 0)
                {
                    Node zeroNode = nodesList.SingleOrDefault(n => n.Value.ToString() == "0");      // If there exists no zero nodes, zeroNode will be null.
                                                                                                    // If more than one zero node exists, method will throw an exception
                    // Therefore, this condition will be true if and only if there is ONE zero node in the list
                    if (zeroNode != null)
                        return nodesList.All(n => n.Value.ToString() == "∞" || n.Value.ToString() == "0");
                }
            }
            catch (Exception) { }

            return false;
        }

        // Przeszukiwanie w głąb, sortowanie topologiczne
        public bool AreNodesOneAndEmpties()
        {
            try
            {
                if (nodesList.Count > 0)
                {
                    Node oneNode = nodesList.SingleOrDefault(n => n.Value.ToString() == "1/");      // If there exists no "1/" nodes, oneNode will be null.
                                                                                                    // If more than one "1/" node exists, method will throw an exception
                                                                                                    // Therefore, this condition will be true if and only if there is ONE "1/" node in the list
                    if (oneNode != null)
                        return nodesList.All(n => n.Value.ToString() == "" || n.Value.ToString() == "1/");
                }
            }
            catch (Exception) { }

            return false;
        }
    }
}
