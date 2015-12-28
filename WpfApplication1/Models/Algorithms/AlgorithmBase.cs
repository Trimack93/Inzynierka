using Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Models.Algorithms
{
    public abstract class AlgorithmBase
    {
        protected AlgorithmBase(List<Edge> edges, List<Node> nodes)
        {
            this.EdgesList = edges;
            this.nodesList = nodes;
        }
        //----------------------------------

        protected readonly string INSTRUCTIONS_PATH = Path.GetFullPath("../../Resources/Instructions/");
        protected List<Edge> EdgesList { get; set; }
        protected List<Node> nodesList { get; set; }

        //----------------------------------

        public int StepCount { get; set; } = 0;

        public List<string> Instructions { get; set; }
    }
}
