using Common.Controls;
using Common.Models;
using Common.Models.Canvas;
using Common.Utilities;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfApplication1.Models.Algorithms;

namespace WpfApplication1.ViewModels
{
    /// <summary>
    /// Base view model for every application mode.
    /// </summary>
    public abstract class BaseViewModel : BaseNotifyPropertyChanged, IModalDialogViewModel
    {
        protected BaseViewModel(string graphPath)
        {
            GRAPH_PATH = Path.GetFullPath(graphPath);
        }

        //----------------------------------

        protected bool _canEdgesAnimate { get; set; } = false;
        protected bool _canMarkNodesBlack { get; set; } = false;
        protected bool _canClickNodes { get; set; } = false;
        protected bool _isStopButtonVisible { get; set; } = false;
        protected bool _isNodeNamesControlVisible { get; set; } = false;
        protected bool _isNodeNamesControlEnabled { get; set; } = false;

        protected readonly string GRAPH_PATH;
        protected IDialogService _dialogService;

        protected AlgorithmBase _algorithm { get; set; }

        protected ObservableCollection<CanvasControlBase> _canvasItems = new ObservableCollection<CanvasControlBase>();
        protected ObservableCollection<ComboboxElement> _comboBoxItems;
        protected List<int> oldGrayNodes = new List<int>();                                 // Contains ID of nodes which were gray before changing their color to black

        /// <summary>
        /// Method for loading graph into canvas and optionally initializing list of graphs (exam mode).
        /// </summary>
        protected Func< List<Graph>, byte, ObservableCollection<CanvasControlBase> > LoadGraphs;

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
        public bool CanClickNodes
        {
            get { return _canClickNodes; }
            set
            {
                _canClickNodes = value;
                RaisePropertyChanged("CanClickNodes");
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
            }
        }
        public bool IsNodeNamesControlVisible
        {
            get { return _isNodeNamesControlVisible; }
            set
            {
                _isNodeNamesControlVisible = value;
                RaisePropertyChanged("IsNodeNamesControlVisible");
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

        public ObservableCollection<CanvasControlBase> CanvasItems
        {
            get { return _canvasItems; }
            protected set
            {
                _canvasItems = value;

                RaisePropertyChanged("CanvasItems");
                RaisePropertyChanged("CanvasNodes");
            }
        }
        public ObservableCollection<ComboboxElement> ComboBoxItems
        {
            get { return _comboBoxItems; }
            protected set
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
        public bool? DialogResult { get; set; } = true;

        public string AlgorithmName { get; protected set; }

        //----------------------------------

        protected abstract void EndSequenceClickExecute(Window learningWindow);
        protected abstract void StopButtonClickExecute(Window learningWindow);

        //----------------------------------

        protected void InitializeAlgorithmFromGraph(List<Graph> graphsList)
        {
            switch (this.AlgorithmName)
            {
                case "Przeszukiwanie wszerz":
                    this.CanvasItems = LoadGraphs(graphsList, 0);
                    this.IsNodeNamesControlVisible = true;
                    this.IsNodeNamesControlEnabled = true;
                    this.CanMarkNodesBlack = true;
                    this.CanClickNodes = true;

                    this.ComboBoxItems = new ObservableCollection<ComboboxElement>();
                    this.ComboBoxItems.Add(new ComboboxElement(0));

                    _algorithm = new BFS(CanvasItems.GetAllEdges(), CanvasItems.GetAllNodes(), this.ComboBoxItems);
                    break;

                case "Przeszukiwanie w głąb":
                    this.CanvasItems = LoadGraphs(graphsList, 1);
                    this.CanMarkNodesBlack = true;
                    this.CanClickNodes = true;

                    _algorithm = new DFS(CanvasItems.GetAllEdges(), CanvasItems.GetAllNodes());
                    break;

                case "Sortowanie topologiczne":
                    this.CanvasItems = LoadGraphs(graphsList, 2);
                    this.CanMarkNodesBlack = true;
                    this.CanClickNodes = true;

                    this.ComboBoxItems = new ObservableCollection<ComboboxElement>();

                    _algorithm = new TopologicalSort(CanvasItems.GetAllEdges(), CanvasItems.GetAllNodes(), this.ComboBoxItems);
                    break;

                case "Algorytm Kruskala":
                    this.CanvasItems = LoadGraphs(graphsList, 3);
                    this.CanEdgesAnimate = true;

                    _algorithm = new Kruskal(CanvasItems.GetAllEdges(), CanvasItems.GetAllNodes());
                    break;

                case "Wykrywanie dwudzielności":
                    this.CanvasItems = LoadGraphs(graphsList, 4);
                    this.IsStopButtonVisible = true;
                    this.IsNodeNamesControlVisible = true;
                    this.IsNodeNamesControlEnabled = true;
                    this.CanMarkNodesBlack = true;
                    this.CanClickNodes = true;

                    this.ComboBoxItems = new ObservableCollection<ComboboxElement>();
                    this.ComboBoxItems.Add( new ComboboxElement(0) );

                    _algorithm = new Bipartition(CanvasItems.GetAllEdges(), CanvasItems.GetAllNodes(), this.ComboBoxItems);
                    break;

                case "Algorytm Dijkstry":
                    this.CanvasItems = LoadGraphs(graphsList, 5);
                    this.CanMarkNodesBlack = true;
                    this.CanClickNodes = true;

                    _algorithm = new Dijkstra(CanvasItems.GetAllEdges(), CanvasItems.GetAllNodes());
                    break;

                case "Algorytm Bellmana-Forda":
                    this.CanvasItems = LoadGraphs(graphsList, 6);
                    this.IsNodeNamesControlVisible = true;
                    this.IsNodeNamesControlEnabled = true;
                    this.CanClickNodes = true;

                    this.ComboBoxItems = new ObservableCollection<ComboboxElement>();
                    this.ComboBoxItems.Add(new ComboboxElement(0));

                    _algorithm = new BellmanFord(CanvasItems.GetAllEdges(), CanvasItems.GetAllNodes(), this.ComboBoxItems);

                    break;
            }
        }

        //----------------------------------

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
                this.oldGrayNodes.Add(nodeID);

            if (nodeToMark.Color != Brushes.Black)
                nodeToMark.Color = Brushes.Black;
            else
            {
                if (this.oldGrayNodes.Contains(nodeID))
                {
                    nodeToMark.Color = Brushes.Gray;

                    this.oldGrayNodes.Remove(nodeID);
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

        void EdgeClickedExecute(RoutedEventArgs p)
        {
            Edge edge = (p.Source as EdgeControl).Edge;

            if (edge.Color.Color == Colors.Black)
            {
                edge.Color = Brushes.LimeGreen;
                edge.Thickness = 3;
            }
            else
            {
                edge.Color = Brushes.Black;
                edge.Thickness = 2;
            }
        }

        //----------------------------------

        bool CanClickNode() { return this.CanClickNodes; }
        bool CanClickEdge() { return this.CanEdgesAnimate; }

        //----------------------------------
        
        public ICommand ContextMenuOpened
        {
            get { return new RelayCommand<RoutedEventArgs>(p => ContextMenuOpenedExecute(p)); }
        }
        public ICommand NodeClicked
        {
            get { return new RelayCommand<RoutedEventArgs>(p => NodeClickedExecute(p), CanClickNode); }
        }
        public ICommand EdgeClicked
        {
            get { return new RelayCommand<RoutedEventArgs>(p => EdgeClickedExecute(p), CanClickEdge); }
        }

        public ICommand MarkNodeBlack
        {
            get { return new RelayCommand<int>(param => MarkNodeBlackExecute(param)); }
        }

        public ICommand EndSequenceClick
        {
            get { return new RelayCommand<Window>(param => EndSequenceClickExecute(param)); }
        }
        public ICommand StopButtonClick
        {
            get { return new RelayCommand<Window>(param => StopButtonClickExecute(param)); }
        }
    }
}
