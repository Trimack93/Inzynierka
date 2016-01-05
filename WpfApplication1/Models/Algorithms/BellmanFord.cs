using Common.Models;
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

            RefreshCorrectLists();
        }

        //----------------------------------

        public ObservableCollection<ComboboxElement> NodesQueue { get; set; }

        //----------------------------------

        public override bool Step()
        {
            base.Step();

            if (this.StepCount == 1)
                return ValidateQueue();

            List<Node> algorithmNodesList = new List<Node>();                               // List of nodes on which algorithm step will be performed

            // Clone correct nodes from beginning of the step into new list
            foreach (Node node in this.CorrectNodesList)
                algorithmNodesList.Add(node.Clone());

            RefreshCorrectLists();

            return true;
        }

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
    }
}
