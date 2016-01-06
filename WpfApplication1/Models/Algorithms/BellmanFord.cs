using Common.Models;
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Models.Algorithms
{
    public class BellmanFord : AlgorithmBase
    {
        public BellmanFord(List<Edge> edges, List<Node> nodes, ObservableCollection<ComboboxElement> queue) : base(edges, nodes)
        {
            string filePath = Path.Combine(INSTRUCTIONS_PATH, "BellmanFord.txt");

            this.Instructions = FileHelper.GetInstructionsFromFile(filePath);
            this.NodesQueue = queue;
            this.Instructions[1] += "Aktualny krok algorytmu: 1";
            this.Instructions[2] += "\n\n\n\nAktualny krok algorytmu: 1";

            RefreshCorrectLists();
        }

        //----------------------------------

        public ObservableCollection<ComboboxElement> NodesQueue { get; set; }
        public bool IsLastStep { get; set; }

        //----------------------------------

        public override bool Step()
        {
            base.Step();

            AddStepNumberToInstruction(1);
            AddStepNumberToInstruction(2);

            if (this.StepCount == 1)
                return ValidateQueue();

            List<Node> algorithmNodesList = new List<Node>();                               // List of nodes on which algorithm step will be performed

            // Clone correct nodes from beginning of the step into new list
            foreach (Node node in this.CorrectNodesList)
                algorithmNodesList.Add(node.Clone());

            List<Node> sortedNodes = new List<Node>(algorithmNodesList.Count);

            for (int i = 0; i < this.NodesQueue.Count; i++)
            {
                Node newNode = algorithmNodesList.Single(n => n.ID == this.NodesQueue[i].SelectedValue.ID);
                sortedNodes.Add(newNode);
            }

            foreach (Node node in sortedNodes)
            {
                bool result = RelaxNeighbours(algorithmNodesList, node);

                // If this is the last step of algorithm and still something got relaxed, it means that there is negative cycle in graph
                if (this.IsLastStep && result == false)
                    return false;
            }

            if ( CompareWithActualNodes(algorithmNodesList) == false )
                return false;

            if (this.StepCount == this.NodesList.Count)
                this.IsLastStep = true;

            if (this.StepCount > this.NodesList.Count)
                this.IsFinished = true;

            RefreshCorrectLists();

            return true;
        }

        public bool IsGraphWithoutNegativeCycle()
        {
            List<Node> algorithmNodesList = new List<Node>();                               // List of nodes on which algorithm step will be performed

            // Clone correct nodes from beginning of the step into new list
            foreach (Node node in this.CorrectNodesList)
                algorithmNodesList.Add(node.Clone());

            // No need to sort anything at this point
            foreach (Node node in algorithmNodesList)
            {
                bool result = RelaxNeighbours(algorithmNodesList, node);

                // If something got relaxed, it means that there is negative cycle in graph
                if (result == false)
                    return false;
            }

            return true;
        }

        public override string GetCurrentInstruction()
        {
            if (StepCount < Instructions.Count - 1)
                return Instructions[StepCount];

            return Instructions[1];
        }

        public string GetLastInstruction()
        {
            return Instructions.Last();
        }

        //----------------------------------

        private bool ValidateQueue()
        {
            var queueDistinct = this.NodesQueue
                .Where(el => el.SelectedValue != null)
                .GroupBy(el => el.SelectedValue.Name)
                .Select(group => group.First())
                .ToList();

            // If user didn't selected all nodes, clear the queue
            if ( queueDistinct.Count != this.CorrectNodesList.Count )
            {
                this.NodesQueue.Clear();
                this.NodesQueue.Add( new ComboboxElement(0) );

                return false;
            }

            return true;
        }

        /// <summary>
        /// Relax neighbour edges of provided node. Method gets into consideration element positions in queue.
        /// </summary>
        /// <param name="algorithmNodesList">Full list of nodes.</param>
        /// <param name="node">Node which neighbour edges will be relaxed.</param>
        /// <returns> True, if  </returns>
        private bool RelaxNeighbours(List<Node> algorithmNodesList, Node node)
        {
            // If node value is infinity, relaxation doesn't have any sense
            if (node.Value.ToString() == "∞")
                return true;

            List<Node> nodeNeighbours = GraphHelper.GetConnectedNodes(algorithmNodesList, node);
            List<Node> nodeNeighboursSorted = new List<Node>(nodeNeighbours.Count);
            
            // Iterate through queue. If current node is one of neighbours, add him into sorted queue
            // This way neighbours will be sorted in order which reflects the order from queue
            foreach ( Node nodeInQueue in this.NodesQueue.Select(el => el.SelectedValue) )
            {
                Node newNode = nodeNeighbours.SingleOrDefault(n => n.ID == nodeInQueue.ID);

                if (newNode != null)
                    nodeNeighboursSorted.Add(newNode);
            }

            // Relax neighbours in sorted order
            foreach (Node neighbour in nodeNeighboursSorted)
            {
                Edge connectingEdge = GraphHelper.GetConnectingEdge(node, neighbour);

                int edgeValue = connectingEdge.GetIntegerValue();
                int nodeValue = node.GetIntegerValue();
                int neighbourValue = neighbour.GetIntegerValue();

                if (nodeValue + edgeValue < neighbourValue)
                {
                    neighbour.Value = nodeValue + edgeValue;

                    // if this is the last step of algorithm, method should return false when relaxation occured
                    if (this.IsLastStep)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Adds step number to instruction.
        /// </summary>
        /// <param name="i">Index of instruction.</param>
        private void AddStepNumberToInstruction(int i)
        {
            int instructionLength = this.Instructions[i].Length;
            int index = this.Instructions[i].LastIndexOf(": ");

            this.Instructions[i] = this.Instructions[i].Remove(index, instructionLength - index);
            this.Instructions[i] += ": " + this.StepCount.ToString();
        }
    }
}
