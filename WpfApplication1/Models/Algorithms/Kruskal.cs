using Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Models.Algorithms
{
    public class Kruskal : AlgorithmBase
    {
        public Kruskal(List<Edge> edges, List<Node> nodes) : base(edges, nodes)
        {
            string filePath = Path.Combine(INSTRUCTIONS_PATH, "Kruskal.txt");

            this.Instructions = FileHelper.GetInstructionsFromFile(filePath);

            RefreshCorrectLists();
        }

        public override bool Step()
        {
            return base.Step();
        }
    }
}
