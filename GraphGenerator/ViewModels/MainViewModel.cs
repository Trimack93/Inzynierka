using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using GraphGenerator.Models;
using Common.Models;
using Common.Utilities;
using MvvmDialogs;
using Common.Controls;

namespace GraphGenerator.ViewModels
{
    public class MainViewModel : BaseNotifyPropertyChanged
    {
        [Obsolete("Use only for design mode", true)]
        public MainViewModel()
        {
            Init();
        }

        public MainViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;
            //this.dialogService = new DialogService(null, (s) => { return Type.GetType("GraphGenerator.Views.AddNode"); });

            Init();
        }

        private void Init()
        {
            Colors.Add(Brushes.Green);
            Colors.Add(Brushes.Red);
            Colors.Add(Brushes.Green);
            Colors.Add(Brushes.Green);
            Colors.Add(Brushes.Red);
            Colors.Add(Brushes.Green);
            Colors.Add(Brushes.Red);

            int columnsCount = this.CanvasWidth / this.RectangleSize;
            int rowsCount = this.CanvasHeight / this.RectangleSize;

            for (int i = 0; i < rowsCount; i++)
                for (int j = 0; j < columnsCount; j++)
                {
                    CanvasItems.Add(new CanvasRectangle(i * columnsCount + j, j * this.RectangleSize, i * this.RectangleSize, this.RectangleSize));
                }

            (CanvasItems[1] as CanvasRectangle).Node = new Node()
                { ID = this.GetNewNodeID(), Name = "a", Value = 6, NameHorizontalAlignment = HorizontalAlignment.Center, NameVerticalAlignment = VerticalAlignment.Top };

            (CanvasItems[69] as CanvasRectangle).Node = new Node()
                { ID = this.GetNewNodeID(), Name = "b", Value = 9, NameHorizontalAlignment = HorizontalAlignment.Right, NameVerticalAlignment = VerticalAlignment.Center };

            (CanvasItems[24] as CanvasRectangle).Node = new Node()
                { ID = this.GetNewNodeID(), Name = "c", Value = 15, NameHorizontalAlignment = HorizontalAlignment.Left, NameVerticalAlignment = VerticalAlignment.Center };

            (CanvasItems[15] as CanvasRectangle).Node = new Node()
                { ID = this.GetNewNodeID(), Name = "d", Value = 666, NameHorizontalAlignment = HorizontalAlignment.Center, NameVerticalAlignment = VerticalAlignment.Bottom };

            (CanvasItems[1] as CanvasRectangle).DoesContainNode = true;
            (CanvasItems[69] as CanvasRectangle).DoesContainNode = true;
            (CanvasItems[24] as CanvasRectangle).DoesContainNode = true;
            (CanvasItems[15] as CanvasRectangle).DoesContainNode = true;

            //--------------------

            Edge randomEdge = new Edge()
            {
                ID = this.GetNewEdgeID(), Thickness = 2, Color = Brushes.Green, Value = "15"
            };
            CanvasItems.Add(new CanvasEdge(150, 100, 60, 150, randomEdge));

            Edge anotherRandomEdge = new Edge()
            {
                ID = this.GetNewEdgeID(), Thickness = 2, Color = Brushes.Red, Value = "43"
            };
            CanvasItems.Add(new CanvasEdge(400, 230, 475, 300, anotherRandomEdge));

            //--------------------

            //CanvasRectangles.Add(new EdgeControl()
            //{
            //    X1 = 100, Y1 = 100, X2 = 150, Y2 = 150,
            //    Edge = e
            //});

            //--------------------

            //CanvasRectangles.Add(new EdgeControl()
            //{
            //    X1 = 100, Y1 = 100, X2 = 150, Y2 = 150,
            //    EdgeThickness = 2, EdgeColor = Brushes.Green
            //});

            //CanvasRectangles.OfType<EdgeControl>().ToList()[0].SetValue(7);
        }

        //----------------------------------

        private bool _NodeButtonIsPressed = false;
        private bool _EdgeButtonIsPressed = false;

        private ObservableCollection<SolidColorBrush> _Colors = new ObservableCollection<SolidColorBrush>();
        private ObservableCollection<CanvasControlBase> _CanvasItems = new ObservableCollection<CanvasControlBase>();

        private readonly IDialogService dialogService;

        private bool isAddingNewNode = false;

        //----------------------------------

        // TODO: get properties from View
        public int CanvasWidth { get { return 726; } }
        public int CanvasHeight { get { return 660; } }
        public int RectangleSize { get { return 66; } }

        public bool NodeButtonIsPressed
        {
            get { return _NodeButtonIsPressed; }

            private set
            {
                _NodeButtonIsPressed = value;
                RaisePropertyChanged("NodeButtonIsPressed");
            }
        }
        public bool EdgeButtonIsPressed
        {
            get { return _EdgeButtonIsPressed; }

            private set
            {
                _EdgeButtonIsPressed = value;
                RaisePropertyChanged("EdgeButtonIsPressed");
            }
        }

        public ObservableCollection<SolidColorBrush> Colors
        {
            get { return _Colors; }
            private set { _Colors = value; }
        }
        public ObservableCollection<CanvasControlBase> CanvasItems
        {
            get { return _CanvasItems; }
            private set { _CanvasItems = value; }
        }

        //----------------------------------

        private void AddNode(int rectangleID)
        {
            CanvasRectangle rectangle = CanvasItems[rectangleID] as CanvasRectangle;

            if (rectangle.DoesContainNode == false)
            {
                var dialogViewModel = new AddNodeViewModel();

                bool? success = dialogService.ShowDialog(this, dialogViewModel);

                // If user provided the node parameters successfully
                if (success.HasValue && success.Value)
                {
                    Node node = dialogViewModel.Node;

                    rectangle.Node = new Node()
                    {
                        ID = this.GetNewNodeID(),
                        Name = node.Name,
                        Value = node.Value,
                        NameHorizontalAlignment = node.NameHorizontalAlignment,
                        NameVerticalAlignment = node.NameVerticalAlignment
                    };

                    rectangle.DoesContainNode = true;
                }
            }
        }

        //private void AddEdge(int ID)
        //{
        //    //TODO
        //    if ( (CanvasRectangles[ID] as CanvasRectangle).DoesContainNode )
        //        MessageBox.Show("Wait for it", "Work in progress", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        //}

        private int GetNewNodeID()
        {
            List<Node> nodesList = CanvasItems                 // List of all nodes in a canvas
                .OfType<CanvasRectangle>()
                .Select(rect => rect.Node)
                .Where(n => n != null)
                .ToList();

            if (nodesList.Count == 0)
                return 0;

            int maxID = nodesList
                .Select(n => n.ID)
                .Max();

            return maxID + 1;
        }
        private int GetNewEdgeID()
        {
            List<Edge> edgesList = CanvasItems                 // List of all edges in a canvas
                .OfType<CanvasEdge>()
                .Select(canv => canv.Edge)
                .Where(e => e != null)
                .ToList();

            if (edgesList.Count == 0)
                return 0;

            int maxID = edgesList
                .Select(e => e.ID)
                .Max();

            return maxID + 1;
        }

        private int GetCanvasRectangleID(double tmpX, double tmpY)
        {
            int x = (int) tmpX;
            int y = (int) tmpY;
            int columnsCount = this.CanvasWidth / this.RectangleSize;

            // Anti-idiot protection, when user goes with mouse beyond canvas borders
            if (x < 0)
                x = 0;
            else if (x > CanvasWidth)
                x = CanvasWidth - 1;

            if (y < 0)
                y = 0;
            else if (y > CanvasHeight)
                y = CanvasHeight - 1;

            return columnsCount * (y / this.RectangleSize) + (x / this.RectangleSize);
        }

        private void DeleteEdge(CanvasEdge canvasEdge)
        {
            List<Node> nodesList = CanvasItems                 // List of all nodes in a canvas
                   .OfType<CanvasRectangle>()
                   .Select(rect => rect.Node)
                   .Where(n => n != null)
                   .ToList();

            foreach (Node node in nodesList)
            {
                Edge edge = node.Edges
                    .Where(e => e.ID == canvasEdge.Edge.ID)
                    .SingleOrDefault();

                if (edge != null)
                    node.Edges.Remove(edge);
            }

            CanvasItems.Remove(canvasEdge);
        }

        //----------------------------------

        public void AddEdge(double x, double y)
        {
            if (this.EdgeButtonIsPressed)
            {
                int rectID = GetCanvasRectangleID(x, y);
                CanvasRectangle rect = CanvasItems[rectID] as CanvasRectangle;

                // Add edge only if rectangle contains a node
                if (rect.DoesContainNode)
                {
                    int edgeID = GetNewEdgeID();
                    Edge edge = new Edge(edgeID);
                    
                    // Add edge to canvas
                    CanvasItems.Add( new CanvasEdge(x, y, x, y, edge) );

                    // Add references between edge and node
                    rect.Node.Edges.Add(edge);
                    edge.NodesID.Add(rect.Node.ID);

                    this.isAddingNewNode = true;
                }
            }
        }
        public void UpdateEdgePosition(double x2, double y2)
        {
            if (this.isAddingNewNode)
            {
                CanvasEdge edge = CanvasItems.LastOrDefault(l => l is CanvasEdge) as CanvasEdge;
                
                if (edge != null)
                {
                    if (x2 > 0 && x2 < CanvasWidth)
                        edge.X2 = x2;

                    if (y2 > 0 && y2 < CanvasHeight)
                        edge.Y2 = y2;
                }
            }
        }
        public void AddEdgeFinish(double x, double y)
        {
            if (this.isAddingNewNode)
            {
                CanvasEdge canvasEdge = CanvasItems.LastOrDefault(l => l is CanvasEdge) as CanvasEdge;
                
                int rectID = GetCanvasRectangleID(x, y);
                CanvasRectangle rect = CanvasItems[rectID] as CanvasRectangle;

                // Add edge only if rectangle contains node and the edge doesn't try to connect node with itself
                if ( rect.DoesContainNode && rect.Node.Edges.Contains(canvasEdge.Edge) == false )
                {
                    canvasEdge.Edge.Value = "69";         // TODO: Call modal window and get node's value
                        
                    // Add references between edge and node
                    rect.Node.Edges.Add(canvasEdge.Edge);
                    canvasEdge.Edge.NodesID.Add(rect.Node.ID);
                }
                else
                    this.DeleteEdge(canvasEdge);

                this.isAddingNewNode = false;
            }
        }

        //----------------------------------

        // GUI update test
        void ChangeCompatibilityExecute()
        {
            if (Colors[2] == Brushes.Green)
                Colors[2] = Brushes.Red;
            else
                Colors[2] = Brushes.Green;
        }

        void AddNodeClickExecute()
        {
            EdgeButtonIsPressed = false;
            NodeButtonIsPressed = !NodeButtonIsPressed;
        }
        void AddEdgeClickExecute()
        {
            NodeButtonIsPressed = false;
            EdgeButtonIsPressed = !EdgeButtonIsPressed;
        }

        void ClickRectangleExecute(int rectangleID)
        {
            if (this.NodeButtonIsPressed)
                AddNode(rectangleID);
            //if (this.EdgeButtonIsPressed)
            //    AddEdge(rectangleID);
        }

        //void CanvasMouseDownExecute()
        //{
        //    ;
        //}
        //void CanvasMouseMoveExecute()
        //{
        //    ;
        //}
        //void CanvasMouseUpExecute()
        //{
        //    ;
        //}

        //----------------------------------

        bool CanClickRectangle()
        {
            return this.EdgeButtonIsPressed || this.NodeButtonIsPressed;
            //return this.NodeButtonIsPressed;
        }

        //----------------------------------

        public ICommand AddNodeClick
        {
            get { return new RelayCommand<object>( p => AddNodeClickExecute() ); }
        }
        public ICommand AddEdgeClick
        {
            get { return new RelayCommand<object>( p => AddEdgeClickExecute() ); }
        }
        public ICommand ChangeCompatibility
        {
            get { return new RelayCommand<object>( p => ChangeCompatibilityExecute() ); }
        }

        //public ICommand CanvasMouseDown
        //{
        //    get { return new RelayCommand<object>( p => CanvasMouseDownExecute() ); }
        //}
        //public ICommand CanvasMouseMove
        //{
        //    get { return new RelayCommand<object>(p => CanvasMouseMoveExecute()); }
        //}
        //public ICommand CanvasMouseUp
        //{
        //    get { return new RelayCommand<object>(p => CanvasMouseUpExecute()); }
        //}

        public ICommand ClickRectangle
        {
            get { return new RelayCommand<int>( param => ClickRectangleExecute(param), CanClickRectangle ); }
        }
    }
}
