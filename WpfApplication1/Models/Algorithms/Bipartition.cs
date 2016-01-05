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
    public class Bipartition : BFS
    {
        public Bipartition(List<Edge> edges, List<Node> nodes, ObservableCollection<ComboboxElement> queue) : base(edges, nodes, queue)
        {
            string filePath = Path.Combine(INSTRUCTIONS_PATH, "Bipartition.txt");

            this.Instructions = FileHelper.GetInstructionsFromFile(filePath);
            this.NodesQueue = queue;

            RefreshCorrectLists();
        }

        //----------------------------------

        public override bool Step()
        {
            if ( this.StepCount > 0 && AreNeighboursWithDifferentParity() == false )
                return false;

            return base.Step();
        }

        /// <summary>
        /// Checks if all the neighbours of current node have different parity than it.
        /// </summary>
        /// <returns>True, if all neighbours have different parity, otherwise false.</returns>
        public bool AreNeighboursWithDifferentParity()
        {
            Node firstNode = this.CorrectNodesQueue[0].SelectedValue;

            // If first node even from the correct queue is null, that means user tried to stop algorithm at the very beginning. So let's fail him
            if (firstNode == null)
                return true;

            int firstNodeModuloResult = firstNode.GetIntegerValue() % 2;

            List<int> neighboursValuesList = GraphHelper.GetConnectedNodes(this.CorrectNodesList, firstNode)
                                                .Where(n => n.Value.ToString() != "∞")
                                                .Select(n => n.GetIntegerValue())
                                                .ToList();

            return neighboursValuesList
                .All( value => value % 2 != firstNodeModuloResult );        // returns true even if sequence is empty
        }

        public override string GetCurrentInstruction()
        {
            if (StepCount < Instructions.Count - 1)
                return Instructions[StepCount];

            return Instructions[2];
        }

        public string GetLastInstruction()
        {
            return Instructions.Last();
        }

        public static bool AreQueuesCorrect(ObservableCollection<ComboboxElement> queue1, ObservableCollection<ComboboxElement> queue2, List<Node> nodesList)
        {
            var firstQueueDistinct = queue1
                .Where(el => el.SelectedValue != null)
                .GroupBy(el => el.SelectedValue.Name)
                .Select(group => group.First())
                .ToList();

            var secondQueueDistinct = queue2
                .Where(el => el.SelectedValue != null)
                .GroupBy(el => el.SelectedValue.Name)
                .Select(group => group.First())
                .ToList();

            if (firstQueueDistinct.Count + secondQueueDistinct.Count != nodesList.Count)
                return false;

            bool firstQueueIsValid = firstQueueDistinct
                .All(el => el.SelectedValue.GetIntegerValue() % 2 == 0);

            bool secondQueueIsValid = secondQueueDistinct
                .All(el => el.SelectedValue.GetIntegerValue() % 2 == 1);

            return firstQueueIsValid && secondQueueIsValid;
        }
    }
}
