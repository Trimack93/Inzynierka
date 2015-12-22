﻿using System;
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
using GraphGenerator.Utilities;
using System.Windows.Controls;

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

            (CanvasItems[59] as CanvasRectangle).Node = new Node()
            { ID = this.GetNewNodeID(), Name = "e", Value = 92, NameHorizontalAlignment = HorizontalAlignment.Center, NameVerticalAlignment = VerticalAlignment.Top };

            (CanvasItems[1] as CanvasRectangle).DoesContainNode = true;
            (CanvasItems[69] as CanvasRectangle).DoesContainNode = true;
            (CanvasItems[24] as CanvasRectangle).DoesContainNode = true;
            (CanvasItems[15] as CanvasRectangle).DoesContainNode = true;
            (CanvasItems[59] as CanvasRectangle).DoesContainNode = true;

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
                RaisePropertyChanged("IsEditState");
            }
        }
        public bool EdgeButtonIsPressed
        {
            get { return _EdgeButtonIsPressed; }

            private set
            {
                _EdgeButtonIsPressed = value;
                RaisePropertyChanged("EdgeButtonIsPressed");
                RaisePropertyChanged("IsEditState");
            }
        }
        public bool IsEditState
        {
            get { return !(NodeButtonIsPressed || EdgeButtonIsPressed); }
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

        private void CorrectEdgePosition(CanvasRectangle rectStart, CanvasRectangle rectEnd, CanvasEdge canvasEdge, bool isTwoSide = false)
        {
            double x1 = rectStart.CanvasLeft + RectangleSize / 2;
            double y1 = rectStart.CanvasTop + RectangleSize / 2;
            double x2 = rectEnd.CanvasLeft + RectangleSize / 2;
            double y2 = rectEnd.CanvasTop + RectangleSize / 2;

            double angleStart;
            double angleEnd;
            double twoSideAdjustment = 0;                           // When there are edges in both directions, their position on the circle must be additionally adjusted...

            if (isTwoSide)
                twoSideAdjustment = 25;                             // ... by a value of 25/50 degrees (depends on edge direction)

            if (x1 <= x2)
            {
                angleStart = MathHelper.GetLineAngle(x1, y1, x2, y2) - MathHelper.AlfaToRadian(twoSideAdjustment);
                angleEnd = angleStart + MathHelper.AlfaToRadian(180) + MathHelper.AlfaToRadian(twoSideAdjustment * 2);
            }
            else
            {
                angleEnd = MathHelper.GetLineAngle(x1, y1, x2, y2) + MathHelper.AlfaToRadian(twoSideAdjustment);
                angleStart = angleEnd + MathHelper.AlfaToRadian(180) - MathHelper.AlfaToRadian(twoSideAdjustment * 2);
            }

            Point newStartPoint = MathHelper.GetPointOnCircle(x1, y1, angleStart, 15);                   // can't be helped ;(
            Point newEndPoint = MathHelper.GetPointOnCircle(x2, y2, angleEnd, 15);

            canvasEdge.X1 = newStartPoint.X;
            canvasEdge.Y1 = newStartPoint.Y;
            canvasEdge.X2 = newEndPoint.X;
            canvasEdge.Y2 = newEndPoint.Y;
        }

        /// <summary>
        /// Gets list of edges between two nodes.
        /// </summary>
        private List<Edge> GetListOfEdges(Node node1, Node node2)
        {
            return node1.Edges
                .Where(e => e.NodesID.Contains(node2.ID))
                .ToList();
        }

        /// <summary>
        /// Checks if there already exists connection from first node to the second.
        /// </summary>
        /// <param name="node1">First node (beginning of the edge).</param>
        /// <param name="node2">Second node (end of the edge).</param>
        /// <returns>True, if edge already exists. Otherwise false.</returns>
        private bool NodesAreAlreadyConnected(Node node1, Node node2)
        {
            List<Edge> connectingEdges = GetListOfEdges(node1, node2);
            
            // If there already exist nodes in both directions
            if (connectingEdges.Count > 1)
                return true;

            // Check if there is edge that goes from another direction
            if (connectingEdges.Count == 1)
                return connectingEdges[0].NodesID[0] != node2.ID;                   // First node in list is where the edge begins

            // If there are no edges between them yet
            return false;
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
                    // If crafty user tries to draw edge outside the canvas boundary
                    if (x2 > 0 && x2 < CanvasWidth)
                        edge.X2 = x2;

                    if (y2 > 0 && y2 < CanvasHeight)
                        edge.Y2 = y2;
                }
            }
        }
        // <gunwokod>
        public void AddEdgeFinish(double x, double y)
        {
            if (this.isAddingNewNode)
            {
                CanvasEdge canvasEdge = CanvasItems.LastOrDefault(l => l is CanvasEdge) as CanvasEdge;
                
                int rectID = GetCanvasRectangleID(x, y);
                CanvasRectangle rectEnd = CanvasItems[rectID] as CanvasRectangle;                   // rectangle containing node at the end of edge (arrow-side)

                // Add edge only if rectangle contains node and the edge doesn't try to connect node with itself
                if ( rectEnd.DoesContainNode && rectEnd.Node.Edges.Contains(canvasEdge.Edge) == false )
                {                    
                    CanvasRectangle rectStart = CanvasItems
                        .OfType<CanvasRectangle>()
                        .Where( r => r.Node?.ID == canvasEdge.Edge.NodesID[0] )
                        .Single();

                    if ( NodesAreAlreadyConnected(rectStart.Node, rectEnd.Node) )
                    {
                        DeleteEdge(canvasEdge);

                        // TODO: Change to custom information modal dialog
                        MessageBox.Show("Krawędź łącząca podane wierzchołki w tym kierunku już istnieje.\nRozważ edycję wartości istniejącej krawędzi.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        // Open dialog for adding new edge
                        //--------------------------------------

                        var dialogViewModel = new AddEdgeViewModel();

                        bool? success = dialogService.ShowDialog(this, dialogViewModel);
                        
                        if (success.HasValue && success.Value)                                      // If user provided the node parameters successfully
                        {
                            // Correct edge(s) position
                            //--------------------------------------

                            List<Edge> connectingEdges = GetListOfEdges(rectStart.Node, rectEnd.Node);
                            
                            if (connectingEdges.Count == 1)                                         // If there already exist edge connecting both nodes
                            {
                                CanvasEdge secondCanvasEdge = CanvasItems                           // Edge with opposed direction
                                    .OfType<CanvasEdge>()
                                    .Single(ce => ce.Edge == connectingEdges[0]);

                                CorrectEdgePosition(rectStart, rectEnd, canvasEdge, true);
                                CorrectEdgePosition(rectEnd, rectStart, secondCanvasEdge, true);

                                // Correct label position - call PropertyChanged on Value property (taki cwany workaround)
                                string tmp = connectingEdges[0].Value;
                                connectingEdges[0].Value = "dummy";
                                connectingEdges[0].Value = tmp;
                            }
                            else
                                CorrectEdgePosition(rectStart, rectEnd, canvasEdge);

                            // Correct label position
                            //--------------------------------------

                            canvasEdge.Edge.Value = dialogViewModel.Edge.Value;

                            // Add references between edge and node
                            //--------------------------------------

                            rectEnd.Node.Edges.Add(canvasEdge.Edge);
                            canvasEdge.Edge.NodesID.Add(rectEnd.Node.ID);
                        }
                        else
                            DeleteEdge(canvasEdge);
                    }
                }
                else
                    DeleteEdge(canvasEdge);

                this.isAddingNewNode = false;
            }
        }
        // </gunwokod>

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

        void NodeMenuItemEditExecute(int nodeID)
        {
            MessageBox.Show("Edytowanie wierzchołka o ID: " + nodeID.ToString());
        }
        void NodeMenuItemDeleteExecute(int nodeID)
        {
            MessageBox.Show("Usuwanie wierzchołka o ID: " + nodeID.ToString());
        }
        void EdgeMenuItemEditExecute(int edgeID)
        {
            MessageBox.Show("Edytowanie krawędzi o ID: " + edgeID.ToString());
        }
        void EdgeMenuItemDeleteExecute(int edgeID)
        {
            MessageBox.Show("Usuwanie krawędzi o ID: " + edgeID.ToString());
        }

        void ContextMenuOpenedExecute(RoutedEventArgs e)
        {
            ContextMenu contextMenu = e.Source as ContextMenu;                      // change to custom control in future - temporary workaround

            contextMenu.IsOpen = this.IsEditState;
        }

        //----------------------------------

        bool CanClickRectangle()
        {
            //return this.EdgeButtonIsPressed || this.NodeButtonIsPressed;
            return true;                                                            // Otherwise triggers in CanvasButton don't work
        }

        bool CanModifyCanvasElements()
        {
            return this.IsEditState;
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

        public ICommand ClickRectangle
        {
            get { return new RelayCommand<int>( param => ClickRectangleExecute(param), CanClickRectangle ); }
        }

        public ICommand ContextMenuOpened
        {
            get { return new RelayCommand<RoutedEventArgs>(p => ContextMenuOpenedExecute(p)); }
        }

        public ICommand NodeMenuItemEdit
        {
            get { return new RelayCommand<int>(param => NodeMenuItemEditExecute(param), CanModifyCanvasElements); }
        }
        public ICommand NodeMenuItemDelete
        {
            get { return new RelayCommand<int>(param => NodeMenuItemDeleteExecute(param), CanModifyCanvasElements); }
        }
        public ICommand EdgeMenuItemEdit
        {
            get { return new RelayCommand<int>(param => EdgeMenuItemEditExecute(param), CanModifyCanvasElements); }
        }
        public ICommand EdgeMenuItemDelete
        {
            get { return new RelayCommand<int>(param => EdgeMenuItemDeleteExecute(param), CanModifyCanvasElements); }
        }
    }
}
