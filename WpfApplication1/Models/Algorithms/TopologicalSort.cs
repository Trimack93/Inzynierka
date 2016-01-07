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
    public class TopologicalSort : DFS, IAlgorithmWithQueue
    {
        public TopologicalSort(List<Edge> edges, List<Node> nodes, ObservableCollection<ComboboxElement> queue) : base(edges, nodes)
        {
            string filePath = Path.Combine(INSTRUCTIONS_PATH, "TopologicalSort.txt");

            this.Instructions = FileHelper.GetInstructionsFromFile(filePath);
            this.NodesQueue = queue;
        }

        //----------------------------------

        public bool isSortingNodes { get; set; } = false;

        public ObservableCollection<ComboboxElement> NodesQueue { get; set; }

        //----------------------------------

        public override bool Step()
        {
            if (this.NodesQueue.Count == 0)
            {
                bool result = base.Step();

                if (this.IsFinished)
                {
                    this.IsFinished = false;
                    this.isSortingNodes = true;

                    this.NodesQueue.Add( new ComboboxElement(0) );
                }

                return result;
            }

            // If stupid user changed something in graph at this point
            if (CompareWithActualNodes(this.CorrectNodesList) == false)
                return false;

            List<int> sortedNodeValues = GraphHelper.GetListOfNodesProcessedTimes(this.CorrectNodesList)
                                                                        .OrderByDescending(v => v)
                                                                        .ToList();

            List<Node> nodesFromQueue = this.NodesQueue.Select(n => n.SelectedValue).ToList();

            List<int> nodesFromQueueValues = GraphHelper.GetListOfNodesProcessedTimes(nodesFromQueue)
                                                                        .ToList();

            if (sortedNodeValues.SequenceEqual(nodesFromQueueValues) == false)
            {
                this.NodesQueue.Clear();
                this.NodesQueue.Add( new ComboboxElement(0) );

                return false;
            }

            this.IsFinished = true;
            return true;
        }

        public void SetLastInstruction()
        {
            _currentInstructionIndex = 3;
        }
    }
}
