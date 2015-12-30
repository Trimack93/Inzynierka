using Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApplication1.Models.Algorithms
{
    public class DFS : AlgorithmBase
    {
        public DFS(List<Edge> edges, List<Node> nodes) : base(edges, nodes)
        {
            string filePath = Path.Combine(INSTRUCTIONS_PATH, "DFS.txt");

            this.Instructions = FileHelper.GetInstructionsFromFile(filePath);

            nodes.Single(n => n.Value.ToString() == "1/").Color = Brushes.Gray;

            RefreshCorrectLists();
        }

        //----------------------------------

        private int _currentInstructionIndex { get; set; }

        //----------------------------------

        public override bool Step()
        {
            base.Step();
            
            List<Node> algorithmNodesList = new List<Node>();                               // List of nodes on which algorithm will be performed

            // Clone correct nodes from beginning of the step into new list
            foreach (Node node in this.CorrectNodesList)
                algorithmNodesList.Add( node.Clone() );



            // TODO: Set actual instruction index
            _currentInstructionIndex++;

            RefreshCorrectLists();

            return true;
        }

        public override string GetCurrentInstruction()
        {
            if (_currentInstructionIndex < Instructions.Count)
                return Instructions[_currentInstructionIndex];
            else
                return Instructions[Instructions.Count - 1];
        }
    }
}
