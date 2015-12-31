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
    public class LearningViewModel : BaseNotifyPropertyChanged
    {
        public LearningViewModel()
        {
            this.AlgorithmName = "[Nazwa algorytmu]";

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

        public LearningViewModel(string algorithmName)
        {
            try
            {
                Func<INotifyPropertyChanged, Type> typeLocator = (t) => App.GetViewClassTypeLocalizer(t);
                this._dialogService = new DialogService(null, typeLocator);

                this.AlgorithmName = algorithmName;

                List<Graph> graphsList;

                CustomXmlSerializer xmlSerializer = new CustomXmlSerializer( typeof(List<Graph>) );

                string encryptedXml = File.ReadAllText(GRAPH_PATH);
                string plainXml = StringEncryption.Decrypt(encryptedXml, "Yaranaika?");

                using (StringReader reader = new StringReader(plainXml))
                {
                    graphsList = (List<Graph>)xmlSerializer.Deserialize(reader);
                }

                switch (this.AlgorithmName)
                {
                    case "Przeszukiwanie wszerz":
                        this.CanvasItems = GetRandomGraphFromList(graphsList, 0);
                        this.IsNodeNamesControlVisible = true;
                        this.IsNodeNamesControlEnabled = true;
                        this.CanMarkNodesBlack = true;

                        this.ComboBoxItems = new ObservableCollection<ComboboxElement>();
                        this.ComboBoxItems.Add( new ComboboxElement(0) );

                        _algorithm = new BFS( CanvasItems.GetAllEdges(), CanvasItems.GetAllNodes(), this.ComboBoxItems );
                        break;

                    case "Przeszukiwanie w głąb":
                        this.CanvasItems = GetRandomGraphFromList(graphsList, 1);
                        this.CanMarkNodesBlack = true;

                        _algorithm = new DFS( CanvasItems.GetAllEdges(), CanvasItems.GetAllNodes() );
                        break;

                    case "Sortowanie topologiczne":
                        this.CanvasItems = GetRandomGraphFromList(graphsList, 2);
                        this.CanMarkNodesBlack = true;

                        this.ComboBoxItems = new ObservableCollection<ComboboxElement>();

                        _algorithm = new TopologicalSort( CanvasItems.GetAllEdges(), CanvasItems.GetAllNodes(), this.ComboBoxItems );
                        break;

                    case "Algorytm Dijkstry":
                        this.CanvasItems = GetRandomGraphFromList(graphsList, 5);
                        this.CanMarkNodesBlack = true;

                        _algorithm = new Dijkstra( CanvasItems.GetAllEdges(), CanvasItems.GetAllNodes() );
                        break;
                }

                this.Instruction = _algorithm?.GetCurrentInstruction();

                RaisePropertyChanged("CanvasNodes");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wystąpil błąd: " + ex.Message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);

                // Get the learning view window from static property and close it
                var openedWindows = App.Current.Windows;
                Window currentWindow = null;

                for (int i = 0; i < openedWindows.Count; i++)
                {
                    if (openedWindows[i].DataContext?.GetType().Name == this.GetType().Name)
                    {
                        currentWindow = openedWindows[i];
                        break;
                    }
                }
                                                                                                                        // Can't close a window which practically wasn't opened yet. 
                currentWindow.Loaded += new RoutedEventHandler((sender, e) => { (sender as Window).Close(); });         // So let's add closing handler to its Loaded event. (ProjektBD experience~ )
            }
        }

        //----------------------------------

        private bool _canEdgesAnimate { get; set; } = false;
        private bool _canMarkNodesBlack { get; set; } = false;
        private bool _isStopButtonVisible { get; set; } = false;
        private bool _isNodeNamesControlVisible { get; set; } = false;
        private bool _isNodeNamesControlEnabled { get; set; } = false;

        private string _instruction;

        private readonly string GRAPH_PATH = Path.GetFullPath("../../Resources/Graphs/LearningData.huu");
        private readonly IDialogService _dialogService;
        
        private ObservableCollection<CanvasControlBase> _canvasItems = new ObservableCollection<CanvasControlBase>();
        private ObservableCollection<ComboboxElement> _comboBoxItems;
        private List<int> oldGrayNodes = new List<int>();                                   // Contains ID of nodes which were gray before changing their color to black

        private AlgorithmBase _algorithm { get; set; }

        //----------------------------------

        public bool CanMarkNodesBlack
        {
            get { return _canMarkNodesBlack; }
            set
            {
                _canMarkNodesBlack = value;
                RaisePropertyChanged("CanMarkNodesBlack");
            }
        }
        public bool CanEdgesAnimate
        {
            get { return _canEdgesAnimate; }
            set
            {
                _canEdgesAnimate = value;
                RaisePropertyChanged("CanEdgesAnimate");
            }
        }
        public bool IsStopButtonVisible
        {
            get { return _isStopButtonVisible; }
            set
            {
                _isStopButtonVisible = value;
                RaisePropertyChanged("IsStopButtonVisible");
                RaisePropertyChanged("IsAdditionalControlVisible");
            }
        }
        public bool IsNodeNamesControlVisible
        {
            get { return _isNodeNamesControlVisible; }
            set
            {
                _isNodeNamesControlVisible = value;
                RaisePropertyChanged("IsNodeNamesControlVisible");
                RaisePropertyChanged("IsAdditionalControlVisible");
            }
        }
        public bool IsNodeNamesControlEnabled
        {
            get { return _isNodeNamesControlEnabled; }
            set
            {
                _isNodeNamesControlEnabled = value;
                RaisePropertyChanged("IsNodeNamesControlEnabled");
            }
        }
        public bool IsAdditionalControlVisible
        {
            get { return IsStopButtonVisible || IsNodeNamesControlVisible; }
        }

        public ObservableCollection<CanvasControlBase> CanvasItems
        {
            get { return _canvasItems; }
            private set
            {
                _canvasItems = value;

                RaisePropertyChanged("CanvasItems");
                RaisePropertyChanged("CanvasNodes");
            }
        }
        public ObservableCollection<ComboboxElement> ComboBoxItems
        {
            get { return _comboBoxItems; }
            private set
            {
                _comboBoxItems = value;

                RaisePropertyChanged("ComboBoxItems");
            }
        }
        public List<Node> CanvasNodes
        {
            get { return CanvasItems.GetAllNodes(); }
        }

        public int CanvasWidth { get { return 726; } }
        public int CanvasHeight { get { return 660; } }
        public int RectangleSize { get { return 66; } }

        public string AlgorithmName { get; private set; }
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
        private ObservableCollection<CanvasControlBase> GetRandomGraphFromList(List<Graph> graphsList, byte algorithmID)
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

        //----------------------------------

        void EndSequenceClickExecute(Window learningWindow)
        {
            bool isSequenceGood = _algorithm.Step();

            if (isSequenceGood)
            {
                if (_algorithm is TopologicalSort && (_algorithm as TopologicalSort).isSortingNodes)
                {
                    this.IsNodeNamesControlEnabled = true;
                    this.IsNodeNamesControlVisible = true;

                    //this.ComboBoxItems.Add(new ComboboxElement(0));

                    (_algorithm as TopologicalSort).SetLastInstruction();

                    //return;
                }

                this.Instruction = _algorithm.GetCurrentInstruction();
                
                if (_algorithm.IsFinished)
                {
                    MessageBox.Show("Gratulacje, algorytm został zakończony!\nZa chwilę nastąpi przejście do głównego menu...", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);

                    if (learningWindow != null)
                        learningWindow.Close();
                }
            }
            else
            {
                MessageBox.Show("Wykonano błędny krok. Stan grafu został cofnięty na początek sekwencji", "Co za niedopatrzenie", MessageBoxButton.OK, MessageBoxImage.Information);

                ReturnGraphToValidState();
                _algorithm.DecrementStep();
            }
        }

        void ContextMenuOpenedExecute(RoutedEventArgs e)
        {
            ContextMenu contextMenu = e.Source as ContextMenu;                      // change to custom control in future - temporary workaround

            contextMenu.IsOpen = true;
        }
        void MarkNodeBlackExecute(int nodeID)
        {
            List<Node> nodesList = CanvasItems.GetAllNodes();

            Node nodeToMark = nodesList.Single(n => n.ID == nodeID);

            if (nodeToMark.Color == Brushes.Gray)
                oldGrayNodes.Add(nodeID);

            if (nodeToMark.Color != Brushes.Black)
                nodeToMark.Color = Brushes.Black;
            else
            {
                if (oldGrayNodes.Contains(nodeID))
                {
                    nodeToMark.Color = Brushes.Gray;
                    oldGrayNodes.Remove(nodeID);
                }
                else
                    nodeToMark.Color = Brushes.Transparent;
            }
        }

        void NodeClickedExecute(RoutedEventArgs e)
        {
            NodeControl nodeControl = e.Source as NodeControl;
            string nodeName = nodeControl.NodeName;

            Node clickedNode = CanvasNodes.Single(n => n.Name == nodeName);

            if (clickedNode.Color != Brushes.Black)
            {
                EditNodeValueViewModel dialogViewModel = new EditNodeValueViewModel(clickedNode.Value);

                bool? success = _dialogService.ShowDialog(this, dialogViewModel);

                if (success.HasValue && success.Value)
                {
                    clickedNode.Value = dialogViewModel.Node.Value;
                }
            }
            else
                MessageBox.Show("Wierzchołek jest już przetworzony, po co zmieniać jego wartość?", "Co czynisz, studencie", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        //----------------------------------

        public ICommand EndSequenceClick
        {
            get { return new RelayCommand<Window>(param => EndSequenceClickExecute(param)); }
        }

        public ICommand ContextMenuOpened
        {
            get { return new RelayCommand<RoutedEventArgs>( p => ContextMenuOpenedExecute(p) ); }
        }
        public ICommand NodeClicked
        {
            get { return new RelayCommand<RoutedEventArgs>(p => NodeClickedExecute(p)); }
        }

        public ICommand MarkNodeBlack
        {
            get { return new RelayCommand<int>( param => MarkNodeBlackExecute(param) ); }
        }
    }
}
