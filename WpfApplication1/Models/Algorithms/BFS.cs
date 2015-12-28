using Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Models.Algorithms
{
    public class BFS : AlgorithmBase
    {
        public BFS(List<Edge> edges, List<Node> nodes) : base(edges, nodes)
        {
            string filePath = Path.Combine(INSTRUCTIONS_PATH, "BFS.txt");
            this.Instructions = FileHelper.GetInstructionsFromFile(filePath, 0);
        }
    }
}
