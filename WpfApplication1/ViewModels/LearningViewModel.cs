using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Utilities;
using System.Collections.ObjectModel;
using Common.Models.Canvas;
using System.Windows;
using Common.Models;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApplication1.Models.Algorithms;
using System.IO;
using Common.Controls;
using MvvmDialogs;
using WpfApplication1.Views;

namespace WpfApplication1.ViewModels
{
    public class LearningViewModel : BaseViewModel
    {
        public LearningViewModel() : base("../../Resources/Graphs/LearningData.huu")
        {
            this.AlgorithmName = "[Nazwa algorytmu]";
            this.AreInstructionsVisible = true;

            //int columnsCount = this.CanvasWidth / this.RectangleSize;
            //int rowsCount = this.CanvasHeight / this.RectangleSize;

            //for (int i = 0; i < rowsCount; i++)
            //    for (int j = 0; j < columnsCount; j++)
            //    {
            //        CanvasItems.Add(new CanvasRectangle(i * columnsCount + j, j * this.RectangleSize, i * this.RectangleSize, this.RectangleSize));
            //    }

            //(CanvasItems[1] as CanvasRectangle).Node = new Node()
            //{ ID = 0, Name = "a", Value = 6, NameHorizontalAlignment = HorizontalAlignment.Center, NameVerticalAlignment = VerticalAlignment.Top };

            //(CanvasItems[69] as CanvasRectangle).Node = new Node()
            //{ ID = 1, Name = "b", Value = 9, NameHorizontalAlignment = HorizontalAlignment.Right, NameVerticalAlignment = VerticalAlignment.Center };

            //(CanvasItems[24] as CanvasRectangle).Node = new Node()
            //{ ID = 2, Name = "c", Value = 15, NameHorizontalAlignment = HorizontalAlignment.Left, NameVerticalAlignment = VerticalAlignment.Center };

            //(CanvasItems[1] as CanvasRectangle).DoesContainNode = true;
            //(CanvasItems[69] as CanvasRectangle).DoesContainNode = true;
            //(CanvasItems[24] as CanvasRectangle).DoesContainNode = true;
        }

        public LearningViewModel(string algorithmName) : base("../../Resources/Graphs/LearningData.huu")
        {
            this.AreInstructionsVisible = true;

            try
            {
                Func<INotifyPropertyChanged, Type> typeLocator = (t) => App.GetViewClassTypeLocalizer(t);
                this._dialogService = new DialogService(null, typeLocator);

                this.AlgorithmName = algorithmName;
                this.LoadGraphs = GetRandomGraphFromList;

                List<Graph> graphsList;

                CustomXmlSerializer xmlSerializer = new CustomXmlSerializer( typeof(List<Graph>) );

                string encryptedXml = File.ReadAllText(GRAPH_PATH);
                string plainXml = StringEncryption.Decrypt(encryptedXml, "Yaranaika?");

                using (StringReader reader = new StringReader(plainXml))
                {
                    graphsList = (List<Graph>)xmlSerializer.Deserialize(reader);
                }
                
                InitializeAlgorithmFromGraph(graphsList);

                this.Instruction = _algorithm?.GetCurrentInstruction();
                                
                RaisePropertyChanged("CanvasNodes");
            }
            catch (Exception)
            {
                MessageBox.Show("Wystąpil nieoczekiwany błąd. Upewnij się, że pliki danych nie są uszkodzone.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);

                this.DialogResult = false;
            }
        }

        //----------------------------------
        
        private string _instruction { get; set; }

        //----------------------------------
        
        public string Instruction
        {
            get { return _instruction; }
            set
            {
                _instruction = value;
                RaisePropertyChanged("Instruction");
            }
        }

        //----------------------------------

        /// <summary>
        /// Gets random graph from list.
        /// </summary>
        /// <param name="graphsList">List of graphs.</param>
        /// <param name="algorithmID">ID of algorithm for which graph is valid.</param>
        /// <returns>Collection of nodes and canvas rectangles.</returns>
        protected ObservableCollection<CanvasControlBase> GetRandomGraphFromList(List<Graph> graphsList, byte algorithmID)
        {
            List<Graph> compatibileGraphsList = graphsList
                .Where(g => g.AlgorithmsSupported[algorithmID] == true)
                .ToList();

            int randomIndex = new Random().Next(compatibileGraphsList.Count);

            return compatibileGraphsList[randomIndex].CanvasGraph;
        }

        private void ReturnGraphToValidState()
        {
            List<Node> nodesList = CanvasItems.GetAllNodes();
            List<Edge> edgesList = CanvasItems.GetAllEdges();

            foreach (Node node in nodesList)
            {
                Node correctNode = _algorithm.CorrectNodesList.Single(n => n.ID == node.ID);

                node.Value = correctNode.Value;
                node.Color = correctNode.Color;
            }

            foreach (Edge edge in edgesList)
            {
                Edge correctEdge = _algorithm.CorrectEdgesList.Single(e => e.ID == edge.ID);

                edge.Value = correctEdge.Value;
                edge.Color = correctEdge.Color;
                edge.Thickness = correctEdge.Thickness;
            }

            // If algorithm is BFS, the queue of nodes also needs to be refreshed (user could mess up there a lot)
            if (_algorithm is BFS)
            {
                this.ComboBoxItems.Clear();
                int i = 0;

                // ComboBoxItems needs to have reference to real node from Canvas, not a filthy clone
                foreach (ComboboxElement element in (_algorithm as BFS).CorrectNodesQueue)
                {
                    Node observableNode = null;

                    if (element.SelectedValue != null)
                        observableNode = CanvasItems.GetNodeById(element.SelectedValue.ID);

                    ComboboxElement newElement = new ComboboxElement(i);
                    newElement.SelectedValue = observableNode;

                    this.ComboBoxItems.Add(newElement);
                }
            }
        }

        private void FinishAlgorithm(Window window)
        {
            MessageBox.Show("Gratulacje, algorytm został zakończony!\nZa chwilę nastąpi przejście do głównego menu...", "Informacja",
                MessageBoxButton.OK, MessageBoxImage.Information);

            if (window != null)
                window.Close();
        }

        //----------------------------------

        protected override void EndSequenceClickExecute(Window learningWindow)
        {
            bool isSequenceGood = _algorithm.Step();

            if (isSequenceGood)
            {
                if (_algorithm is TopologicalSort && (_algorithm as TopologicalSort).isSortingNodes)
                {
                    this.IsNodeNamesControlEnabled = true;
                    this.IsNodeNamesControlVisible = true;

                    (_algorithm as TopologicalSort).SetLastInstruction();
                }

                this.Instruction = _algorithm.GetCurrentInstruction();

                if (_algorithm is BellmanFord)
                {
                    this.IsNodeNamesControlEnabled = false;

                    if ((_algorithm as BellmanFord).IsLastStep)
                    {
                        this.IsStopButtonVisible = true;
                        this.Instruction = (_algorithm as BellmanFord).GetLastInstruction();
                    }
                }
                
                if (_algorithm.IsFinished)
                {
                    if (_algorithm is Bipartition)
                    {
                        this.Instruction = (_algorithm as Bipartition).GetLastInstruction();

                        DivideNodesViewModel viewModel = new DivideNodesViewModel(_algorithm.CorrectNodesList);
                        _dialogService.ShowDialog(this, viewModel);
                    }

                    FinishAlgorithm(learningWindow);
                }
            }
            else
            {
                MessageBox.Show("Wykonano błędny krok. Stan grafu został cofnięty na początek sekwencji", "Co za niedopatrzenie", MessageBoxButton.OK, MessageBoxImage.Information);

                ReturnGraphToValidState();
                _algorithm.DecrementStep();
            }
        }

        protected override void StopButtonClickExecute(Window learningWindow)
        {
            if (_algorithm is Bipartition)
            {
                if ( (_algorithm as Bipartition).AreNeighboursWithDifferentParity() == false )
                {
                    FinishAlgorithm(learningWindow);
                }
                else
                {
                    MessageBox.Show("Brak podstaw do stwierdzenia braku dwudzielności. Przeanalizuj dokładnie wartości sąsiadów aktualnego wierzchołka.", "Błąd",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            else if (_algorithm is BellmanFord)
            {
                if ( (_algorithm as BellmanFord).IsGraphWithoutNegativeCycle() == false )
                {
                    FinishAlgorithm(learningWindow);
                }
                else
                {
                    MessageBox.Show("Jesteś pewny, że w grafie istnieje ujemny cykl?", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
