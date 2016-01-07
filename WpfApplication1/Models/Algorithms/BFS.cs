using Common.Models;
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApplication1.Models.Algorithms
{
    public class BFS : AlgorithmBase, IAlgorithmWithQueue
    {
        public BFS(List<Edge> edges, List<Node> nodes, ObservableCollection<ComboboxElement> queue) : base(edges, nodes)
        {
            string filePath = Path.Combine(INSTRUCTIONS_PATH, "BFS.txt");

            this.Instructions = FileHelper.GetInstructionsFromFile(filePath);
            this.NodesQueue = queue;

            RefreshCorrectLists();
        }

        //----------------------------------

        public ObservableCollection<ComboboxElement> NodesQueue { get; set; }// = new List<ComboboxElement>();

        // Queue not impured by user's actions
        public ObservableCollection<ComboboxElement> CorrectNodesQueue { get; set; } = new ObservableCollection<ComboboxElement>();

        //----------------------------------

        public override bool Step()
        {
            base.Step();

            if (this.StepCount == 1)
            {
                string rootNodeName = CorrectNodesList.Single(n => n.Value.ToString() == "0").Name;         // Name of node which value is 0 (root node)

                // First value in queue must be the root node
                if (NodesQueue.Count == 0 || NodesQueue.Count > 2 || NodesQueue[0].SelectedValue?.Name != rootNodeName)
                    return false;
                
                if ( CompareWithActualNodes(CorrectNodesList) == false )
                    return false;

                Node observableRootNode = NodesList.Single(n => n.Name == rootNodeName);
                observableRootNode.Color = Brushes.Gray;
            }
            else
            {
                List<Node> algorithmNodesList = new List<Node>();                               // List of nodes on which algorithm will be performed
                
                // Clone correct nodes from beginning of the step into new list
                foreach (Node node in this.CorrectNodesList)
                    algorithmNodesList.Add( node.Clone() );

                // Get first node from queue and set its color to black
                Node blackNode = algorithmNodesList.Single(n => n.Name == CorrectNodesQueue[0].SelectedValue.Name);      // Black node of the current step
                blackNode.Color = Brushes.Black;

                // Get black node non-black neighbours
                List<Node> nodeNeighbours = GraphHelper.GetConnectedNodes(algorithmNodesList, blackNode)    // Non-black neighbours of node
                    .Where(n => n.Color != Brushes.Black && n.Color != Brushes.Gray)
                    .ToList();

                // Change their values to (blackNodeValue + 1)
                nodeNeighbours.ForEach(n => n.Value = ( Int32.Parse(blackNode.Value.ToString()) ) + 1);
                
                // Compare correctly processed graph with actual one
                if ( CompareWithActualNodes(algorithmNodesList) == false)
                    return false;

                //--------------------
                // Nodes are vaild, time to check the queue

                List<string> nodeNamesFromCorrectQueue = CorrectNodesQueue
                    .Where(el => el.SelectedValue != null)
                    .Select(el => el.SelectedValue.Name)
                    .ToList();

                List<string> nodeNamesFromQueue = NodesQueue
                    .Where(el => el.SelectedValue != null)
                    .Select(el => el.SelectedValue.Name)
                    .ToList();

                // Compare first values in queue - the beginning should be the same
                for (int i = 0; i < nodeNamesFromCorrectQueue.Count; i++)
                {
                    if ( nodeNamesFromQueue[i] != nodeNamesFromCorrectQueue[i] )
                        return false;
                }

                // The only new elements in the queue should be the black node neighbours
                if (nodeNamesFromQueue.Count != nodeNamesFromCorrectQueue.Count + nodeNeighbours.Count)
                    return false;

                // Compare last values in queue with black node neighbours
                for (int i = nodeNamesFromCorrectQueue.Count; i < nodeNamesFromQueue.Count; i++)
                {
                    if ( nodeNeighbours.Any(n => n.Name == nodeNamesFromQueue[i] ) == false)
                        return false;
                }
                
                // If everything went well, remove first element from queue
                this.NodesQueue.RemoveAt(0);

                // Color all neighbours added to queue in gray
                List<Node> observableNodes = NodesList
                    .Where(n => nodeNeighbours
                            .Any(neighbour => neighbour.ID == n.ID))
                    .ToList();

                observableNodes.ForEach( n => n.Color = Brushes.Gray );

                // If queue is empty at this point, finish algorithm
                if (this.NodesQueue
                        .Where(el => el.SelectedValue != null)
                        .ToList()
                        .Count == 0)
                {
                    this.IsFinished = true;
                }
            }

            RefreshCorrectLists();

            return true;
        }

        //----------------------------------

        protected new void RefreshCorrectLists()
        {
            base.RefreshCorrectLists();

            this.CorrectNodesQueue.Clear();

            foreach (ComboboxElement element in this.NodesQueue)
                this.CorrectNodesQueue.Add( element.Clone()) ;
        }
    }
}
