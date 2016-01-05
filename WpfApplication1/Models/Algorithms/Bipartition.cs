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
            if ( AreNeighboursWithDifferentParity() == false )
                return false;

            return base.Step();
        }

        public bool AreNeighboursWithDifferentParity()
        {
            if (this.CorrectNodesQueue.Count == 0)
                return false;

            Node firstNode = this.CorrectNodesQueue[0].SelectedValue;
            int firstNodeModuloResult = firstNode.GetIntegerValue() % 2;

            List<int> neighboursValuesList = GraphHelper.GetConnectedNodes(this.CorrectNodesList, firstNode)
                                                .Where(n => n.Value.ToString() != "∞")
                                                .Select(n => n.GetIntegerValue())
                                                .ToList();

            return neighboursValuesList
                .All( value => value % 2 != firstNodeModuloResult );
        }
    }
}
