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
            this.NodesList = nodes;

            //foreach (Edge edge in edges)
            //    this.CorrectEdgesList.Add( edge.Clone() );
            
            //foreach (Node node in nodes)
            //    this.CorrectNodesList.Add( node.Clone() );
        }

        //----------------------------------

        protected readonly string INSTRUCTIONS_PATH = Path.GetFullPath("../../Resources/Instructions/");
        protected int StepCount { get; set; } = 0;

        protected List<Edge> EdgesList { get; set; }
        protected List<Node> NodesList { get; set; }

        protected List<string> Instructions { get; set; }

        // Lists of edges and nodes not impured by user's actions
        public List<Edge> CorrectEdgesList { get; set; } = new List<Edge>();
        public List<Node> CorrectNodesList { get; set; } = new List<Node>();

        //----------------------------------

        public bool IsFinished { get; set; } = false;

        //----------------------------------

        public virtual bool Step()
        {
            StepCount++;

            return true;
        }

        public void DecrementStep()
        {
            StepCount--;
        }

        public string GetCurrentInstruction()
        {
            if (StepCount < Instructions.Count)
                return Instructions[StepCount];

            return Instructions.Last();
        }

        //----------------------------------

        protected void RefreshCorrectLists()
        {
            this.CorrectEdgesList.Clear();
            this.CorrectNodesList.Clear();

            foreach (Edge edge in this.EdgesList)
                this.CorrectEdgesList.Add(edge.Clone());

            foreach (Node node in this.NodesList)
                this.CorrectNodesList.Add(node.Clone());
        }
    }
}
