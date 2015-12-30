using Common.Models;
using Common.Utilities;
using Common.Utilities.Exceptions;
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

            try
            {
                Node highSeqNode = GraphHelper.GetNodeWithHighestStep(algorithmNodesList);
                Node highSeqRealNode = GraphHelper.GetNodeWithHighestStep(this.NodesList);

                if (highSeqNode == null || highSeqRealNode == null)
                    return false;

                string highSeqNodeValue = highSeqNode.Value.ToString();

                // Node with highest step value is in format "x/"
                if (highSeqNodeValue.EndsWith("/"))
                {
                    List<Node> whiteNeighbours = GraphHelper.GetConnectedNodes(algorithmNodesList, highSeqNode)
                                                        .Where(n => n.Color == Brushes.Transparent)
                                                        .ToList();

                    // If node has white neighbours
                    if (whiteNeighbours.Count > 0)
                    {
                        //Node highSeqRealNode = GraphHelper.GetNodeWithHighestStep(this.NodesList);

                        if (whiteNeighbours.Any(n => n.ID == highSeqRealNode.ID) == false)
                            return false;
                        else
                        {
                            Node selectedNeighbour = whiteNeighbours.Single(n => n.ID == highSeqRealNode.ID);
                            GraphHelper.SetNodeVisitedTime(selectedNeighbour, this.StepCount + 1);

                            if (CompareWithActualNodes(algorithmNodesList) == false)
                                return false;
                        }
                    }
                    // If not, its value changes from "x/" to "x/x+1"
                    else
                    {
                        GraphHelper.SetNodeProcessedTime(highSeqNode, this.StepCount + 1);              // Algorithm step count is always 1 number behind the timeflow
                        highSeqNode.Color = Brushes.Black;

                        if (CompareWithActualNodes(algorithmNodesList) == false)
                            return false;
                    }
                }
                // Node with highest step value is in format "x/y"
                else if (highSeqNodeValue.Count(v => v == '/') == 1)
                {
                    Node previousNode = GraphHelper.GetPreviousGrayNodeWithHighestSeq(algorithmNodesList, highSeqNode);

                    if (previousNode != null)
                    {
                        List<Node> whiteNeighbours = GraphHelper.GetConnectedNodes(algorithmNodesList, previousNode)
                                                            .Where(n => n.Color == Brushes.Transparent)
                                                            .ToList();

                        // If previous node has white neighbours, check if user picked one of them
                        if (whiteNeighbours.Count > 0)
                        {
                            //Node highSeqRealNode = GraphHelper.GetNodeWithHighestStep(this.NodesList);

                            if (whiteNeighbours.Any(n => n.ID == highSeqRealNode.ID) == false)
                                return false;
                            else
                            {
                                Node selectedNeighbour = whiteNeighbours.Single(n => n.ID == highSeqRealNode.ID);
                                GraphHelper.SetNodeVisitedTime(selectedNeighbour, this.StepCount + 1);

                                if (CompareWithActualNodes(algorithmNodesList) == false)
                                    return false;
                            }
                        }

                        // If not, previous node is the next processed
                        else
                        {
                            GraphHelper.SetNodeProcessedTime(previousNode, this.StepCount + 1);
                            previousNode.Color = Brushes.Black;

                            if (CompareWithActualNodes(algorithmNodesList) == false)
                                return false;
                        }
                    }

                    // If previous node doesn't exits, it's time to pick random white node from the leftovers
                    else
                    {
                        List<Node> randomWhiteNodes = algorithmNodesList
                                                                .Where(n => n.Color == Brushes.Transparent)
                                                                .ToList();

                        // If there are still free white nodes left
                        if (randomWhiteNodes.Count > 0)
                        {
                            // If node selected by user is none of those
                            if (randomWhiteNodes.Any(n => n.ID == highSeqRealNode.ID) == false)
                                return false;
                            else
                            {
                                Node selectedNeighbour = randomWhiteNodes.Single(n => n.ID == highSeqRealNode.ID);
                                GraphHelper.SetNodeVisitedTime(selectedNeighbour, this.StepCount + 1);

                                if (CompareWithActualNodes(algorithmNodesList) == false)
                                    return false;
                            }
                        }

                        // Shouldn't ever happen. If so, report is as error
                        else
                            return false;
                    }
                }
                else
                    return false;

                //highSeqNode = GraphHelper.GetNodeWithHighestStep(algorithmNodesList);

                if ( highSeqRealNode.Value.ToString().EndsWith("/") )
                    highSeqRealNode.Color = Brushes.Gray;
            }
            catch (MultipleNodesWithHighestStepException)
            {
                return false;
            }

            // TODO: Set actual instruction index
            _currentInstructionIndex++;

            // If all nodes are processed, finish algorithm
            if ( this.NodesList.All( n => n.Color == Brushes.Black) )
                this.IsFinished = true;

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
