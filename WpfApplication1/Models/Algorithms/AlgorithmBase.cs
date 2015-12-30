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
        }

        //----------------------------------

        protected readonly string INSTRUCTIONS_PATH = Path.GetFullPath("../../Resources/Instructions/");
        protected int StepCount { get; set; } = 0;

        protected List<Edge> EdgesList { get; set; }
        protected List<Node> NodesList { get; set; }
        protected List<string> Instructions { get; set; }

        //----------------------------------

        public bool IsFinished { get; set; } = false;

        // Lists of edges and nodes not impured by user's actions
        public List<Edge> CorrectEdgesList { get; set; } = new List<Edge>();
        public List<Node> CorrectNodesList { get; set; } = new List<Node>();

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

        public virtual string GetCurrentInstruction()
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

        /// <summary>
        /// Compares nodes in the list against actual nodes in the graph.
        /// </summary>
        /// <returns>True, if nodes are the same, otherwise false.</returns>
        protected bool CompareWithActualNodes(List<Node> algorithmNodesList)
        {
            foreach (Node node in this.NodesList)
            {
                Node correctNode = algorithmNodesList.Single(n => n.ID == node.ID);

                if (node.Value.ToString() != correctNode.Value.ToString() || node.Color.Color != correctNode.Color.Color)     // One can't simply compare SolidColorBrush (reference type :<)
                    return false;
            }

            return true;
        }
    }
}
