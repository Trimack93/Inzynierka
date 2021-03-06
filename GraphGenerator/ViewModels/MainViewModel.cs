﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using Common.Models;
using Common.Models.Canvas;
using Common.Utilities;
using MvvmDialogs;
using GraphGenerator.Utilities;
using System.Windows.Controls;
using Microsoft.Win32;
using System.IO;
using System.Xml.Serialization;

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

            _graphCompatibility = new GraphCompatibility(CanvasItems);

            int columnsCount = this.CanvasWidth / this.RectangleSize;
            int rowsCount = this.CanvasHeight / this.RectangleSize;

            for (int i = 0; i < rowsCount; i++)
                for (int j = 0; j < columnsCount; j++)
                {
                    CanvasItems.Add(new CanvasRectangle(i * columnsCount + j, j * this.RectangleSize, i * this.RectangleSize, this.RectangleSize));
                }

            //(CanvasItems[1] as CanvasRectangle).Node = new Node()
            //    { ID = this.GetNewNodeID(), Name = "a", Value = 6, NameHorizontalAlignment = HorizontalAlignment.Center, NameVerticalAlignment = VerticalAlignment.Top };

            //(CanvasItems[69] as CanvasRectangle).Node = new Node()
            //    { ID = this.GetNewNodeID(), Name = "b", Value = 9, NameHorizontalAlignment = HorizontalAlignment.Right, NameVerticalAlignment = VerticalAlignment.Center };

            //(CanvasItems[24] as CanvasRectangle).Node = new Node()
            //    { ID = this.GetNewNodeID(), Name = "c", Value = 15, NameHorizontalAlignment = HorizontalAlignment.Left, NameVerticalAlignment = VerticalAlignment.Center };

            //(CanvasItems[15] as CanvasRectangle).Node = new Node()
            //{ ID = this.GetNewNodeID(), Name = "d", Value = "0", NameHorizontalAlignment = HorizontalAlignment.Center, NameVerticalAlignment = VerticalAlignment.Top };

            //(CanvasItems[59] as CanvasRectangle).Node = new Node()
            //{ ID = this.GetNewNodeID(), Name = "e", Value = "", NameHorizontalAlignment = HorizontalAlignment.Center, NameVerticalAlignment = VerticalAlignment.Bottom };

            //(CanvasItems[1] as CanvasRectangle).DoesContainNode = true;
            //(CanvasItems[69] as CanvasRectangle).DoesContainNode = true;
            //(CanvasItems[24] as CanvasRectangle).DoesContainNode = true;
            //(CanvasItems[15] as CanvasRectangle).DoesContainNode = true;
            //(CanvasItems[59] as CanvasRectangle).DoesContainNode = true;

            CheckCompatibility();

            //--------------------

            //Edge randomEdge = new Edge()
            //{
            //    ID = this.GetNewEdgeID(), Thickness = 2, Color = Brushes.Green, Value = "15"
            //};
            //CanvasItems.Add(new CanvasEdge(150, 100, 60, 150, randomEdge));

            //Edge anotherRandomEdge = new Edge()
            //{
            //    ID = this.GetNewEdgeID(), Thickness = 2, Color = Brushes.Red, Value = "43"
            //};
            //CanvasItems.Add(new CanvasEdge(400, 230, 475, 300, anotherRandomEdge));
        }

        //----------------------------------
        #region Compatibility
            
        private void CheckCompatibility()
        {
            _graphCompatibility.Init();

            if (_graphCompatibility.AreBasicRequirementsMet() == false)
            {
                for (int i = 0; i < Colors.Count; i++)
                    Colors[i] = Brushes.Red;

                RaisePropertyChanged("CanSaveGraph");
                return;
            }

            bool areNodesWithDifferentCharNames = _graphCompatibility.AreNodesWithDifferentCharNames();
            bool areNodesWithCharValuesOnly = _graphCompatibility.AreNodesWithCharValuesOnly();
            bool areNodesWithNames = _graphCompatibility.AreNodesWithNames();
            bool isGraphDirected = _graphCompatibility.IsGraphDirected();
            bool areEdgesEmpty = _graphCompatibility.AreEdgesEmpty();
            bool areEdgesWithIntValues = _graphCompatibility.AreEdgesWithIntValues();
            bool areEdgesNonNegative = _graphCompatibility.AreEdgesNonNegative();
            bool areNodesZeroAndInfinites = _graphCompatibility.AreNodesZeroAndInfinites();
            bool areNodesOneAndEmpties = _graphCompatibility.AreNodesOneAndEmpties();

            // Breadth-first search
            Colors[0] = areNodesWithDifferentCharNames && areEdgesEmpty && areNodesZeroAndInfinites && areNodesWithNames ?
                Brushes.Green : Brushes.Red;

            // Depth-first search
            Colors[1] = areNodesWithDifferentCharNames && areEdgesEmpty && areNodesOneAndEmpties ?
                Brushes.Green : Brushes.Red;

            // Topological sorting
            Colors[2] = areNodesWithDifferentCharNames && areEdgesEmpty && areNodesOneAndEmpties && isGraphDirected ?
                Brushes.Green : Brushes.Red;

            // Kruskal's algorithm
            Colors[3] = areEdgesNonNegative && areNodesWithCharValuesOnly && !isGraphDirected ?
                Brushes.Green : Brushes.Red;

            // Bipartite graph detecting
            Colors[4] = areNodesWithDifferentCharNames && areEdgesEmpty && areNodesZeroAndInfinites && !isGraphDirected ?
                Brushes.Green : Brushes.Red;

            // Dijkstra's algorithm
            Colors[5] = areNodesWithDifferentCharNames && areEdgesNonNegative && areNodesZeroAndInfinites && isGraphDirected ?
                Brushes.Green : Brushes.Red;

            // Bellman–Ford algorithm
            Colors[6] = areNodesWithDifferentCharNames && areEdgesWithIntValues && areNodesZeroAndInfinites && isGraphDirected ?
                Brushes.Green : Brushes.Red;

            RaisePropertyChanged("CanSaveGraph");
        }

        #endregion
        //----------------------------------

        private bool _NodeButtonIsPressed = false;
        private bool _EdgeButtonIsPressed = false;
        private bool isAddingNewNode = false;

        private ObservableCollection<SolidColorBrush> _Colors = new ObservableCollection<SolidColorBrush>();
        private ObservableCollection<CanvasControlBase> _CanvasItems = new ObservableCollection<CanvasControlBase>();

        private readonly IDialogService dialogService;

        private GraphCompatibility _graphCompatibility;

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
        public bool CanSaveGraph
        {
            get { return Colors.Any(c => c == Brushes.Green); }
        }

        public ObservableCollection<SolidColorBrush> Colors
        {
            get { return _Colors; }
            private set
            {
                _Colors = value;
                RaisePropertyChanged("Colors");
            }
        }
        public ObservableCollection<CanvasControlBase> CanvasItems
        {
            get { return _CanvasItems; }
            private set
            {
                _CanvasItems = value;
                _graphCompatibility = new GraphCompatibility(CanvasItems);              // Refresh collection within _graphCompatibility object

                RaisePropertyChanged("CanvasItems");

            }
        }

        //----------------------------------
        
        private int GetNewNodeID()
        {
            List<Node> nodesList = CanvasItems.GetAllNodes();

            if (nodesList.Count == 0)
                return 0;

            int maxID = nodesList
                .Select(n => n.ID)
                .Max();

            return maxID + 1;
        }
        private int GetNewEdgeID()
        {
            List<Edge> edgesList = CanvasItems.GetAllEdges();

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
            List<Node> nodesList = CanvasItems.GetAllNodes();

            foreach (Node node in nodesList)
            {
                node.Edges.Remove(canvasEdge.Edge);
            }

            CanvasItems.Remove(canvasEdge);
        }

        /// <summary>
        /// Checks if there already exists connection from first node to the second.
        /// </summary>
        /// <param name="node1">First node (beginning of the edge).</param>
        /// <param name="node2">Second node (end of the edge).</param>
        /// <returns>True, if edge already exists. Otherwise false.</returns>
        private bool NodesAreAlreadyConnected(Node node1, Node node2)
        {
            List<Edge> connectingEdges = GraphHelper.GetListOfEdges(node1, node2);
            
            // If there already exist edges in both directions
            if (connectingEdges.Count > 1)
                return true;

            // Check if there is edge that goes from another direction or bidirectional edge
            if (connectingEdges.Count == 1)
            {
                if (connectingEdges[0].IsBidirectional)
                    return true;

                return connectingEdges[0].NodesID[0] != node2.ID;                   // First node in list is where the edge begins
            }

            // If there are no edges between them yet
            return false;
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
                    CheckCompatibility();
                }
            }
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
                    CanvasRectangle rectStart = CanvasItems.GetRectangleAtBeginning(canvasEdge.Edge);

                    if ( NodesAreAlreadyConnected(rectStart.Node, rectEnd.Node) )
                    {
                        DeleteEdge(canvasEdge);

                        // TODO: Change to custom information modal dialog
                        MessageBox.Show("Krawędź łącząca podane wierzchołki w tym kierunku już istnieje.\nRozważ edycję wartości istniejącej krawędzi.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        // Check if edge can be bidirectional
                        //--------------------------------------

                        List<Edge> connectingEdges = GraphHelper.GetListOfEdges(rectStart.Node, rectEnd.Node);
                        bool canEdgeBeBidirectional = true;
                        
                        if (connectingEdges.Count == 1 && connectingEdges[0].IsBidirectional == false)
                            canEdgeBeBidirectional = false;

                        // Open dialog for adding new edge
                        //--------------------------------------

                        var dialogViewModel = new AddEdgeViewModel(canEdgeBeBidirectional);

                        bool? success = dialogService.ShowDialog(this, dialogViewModel);
                        
                        if (success.HasValue && success.Value)                                  // If user provided the node parameters successfully
                        {
                            // Correct edge(s) position
                            //--------------------------------------
                            
                            if (connectingEdges.Count == 1)                                     // If there already exist edge connecting both nodes (it's not bidirectional for sure, because we checked it before
                            {
                                CanvasEdge secondCanvasEdge = CanvasItems.GetCanvasEdge( connectingEdges[0] );          // Edge with opposed direction

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

                            canvasEdge.Edge.IsBidirectional = dialogViewModel.Edge.IsBidirectional;
                            canvasEdge.Edge.Value = dialogViewModel.Edge.Value;

                            // Add references between edge and node
                            //--------------------------------------

                            rectEnd.Node.Edges.Add(canvasEdge.Edge);
                            canvasEdge.Edge.NodesID.Add(rectEnd.Node.ID);

                            CheckCompatibility();
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
        }

        void NodeMenuItemEditExecute(int nodeID)
        {
            CanvasRectangle canvasRectangle = CanvasItems.GetRectByNodeId(nodeID);

            var dialogViewModel = new EditNodeViewModel(canvasRectangle.Node);

            bool? success = dialogService.ShowDialog(this, dialogViewModel);

            // If user provided the node parameters successfully
            if (success.HasValue && success.Value)
            {
                Node newNode = dialogViewModel.Node;
                
                canvasRectangle.Node.Name = newNode.Name;
                canvasRectangle.Node.Value = newNode.Value;
                canvasRectangle.Node.NameHorizontalAlignment = newNode.NameHorizontalAlignment;
                canvasRectangle.Node.NameVerticalAlignment = newNode.NameVerticalAlignment;

                CheckCompatibility();
            }
        }
        void EdgeMenuItemEditExecute(int edgeID)
        {
            bool canEdgeBeBidirectional = true;
            
            // Edge to edit
            CanvasEdge canvasEdge = CanvasItems.GetEdgeById(edgeID);

            // Nodes which this edge is connecting
            List<Node> connectedNodes = CanvasItems.GetConnectedNodes(canvasEdge.Edge);
            
            // List of all edges between those two nodes
            List<Edge> connectingEdges = GraphHelper.GetListOfEdges(connectedNodes[0], connectedNodes[1]);

            if (connectingEdges.Count == 2)
                canEdgeBeBidirectional = false;
            
            var dialogViewModel = new EditEdgeViewModel(canvasEdge.Edge, canEdgeBeBidirectional);

            bool? success = dialogService.ShowDialog(this, dialogViewModel);

            // If user provided the node parameters successfully
            if (success.HasValue && success.Value)
            {
                Edge edge = dialogViewModel.Edge;
                
                canvasEdge.Edge.Value = edge.Value;
                canvasEdge.Edge.IsBidirectional = edge.IsBidirectional;

                CheckCompatibility();
            }
        }

        void NodeMenuItemDeleteExecute(int nodeID)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć podany wierzchołek?", "Potwierdź działanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {                
                // Rectangle containing node to delete
                CanvasRectangle canvasRectangle = CanvasItems.GetRectByNodeId(nodeID);
                
                // All edges connected to removable node
                List<CanvasEdge> canvasEdgesList = CanvasItems.GetConnectedEdges(canvasRectangle.Node);

                foreach (CanvasEdge canvasEdge in canvasEdgesList)
                    DeleteEdge(canvasEdge);

                canvasRectangle.Node = null;
                canvasRectangle.DoesContainNode = false;

                CheckCompatibility();
            }
        }
        void EdgeMenuItemDeleteExecute(int edgeID)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz usunąć podaną krawędź?", "Potwierdź działanie", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Edge to be deleted
                CanvasEdge canvasEdge = CanvasItems.GetEdgeById(edgeID);
                
                // List of nodes which are connected by this edge
                List<CanvasRectangle> rectanglesList = CanvasItems.GetConnectedRectangles(canvasEdge.Edge);

                DeleteEdge(canvasEdge);

                Edge connectingEdge = GraphHelper.GetListOfEdges(rectanglesList[0].Node, rectanglesList[1].Node)
                    .SingleOrDefault();

                // If the nodes were connected both-sides by two directional edges
                if (connectingEdge != null)
                {
                    CanvasEdge connectingCanvasEdge = CanvasItems.GetCanvasEdge(connectingEdge);

                    // NodesID[0] = beginning of the edge. CorrectEdgePosition requires valid order of nodes, and because of that
                    // if the starting node isn't at the beginning, we need to reverse the order in rectanglesList.
                    if (rectanglesList[0].Node.ID != connectingEdge.NodesID[0])
                        rectanglesList.Reverse();

                    CorrectEdgePosition(rectanglesList[0], rectanglesList[1], connectingCanvasEdge);
                }

                CheckCompatibility();
            }
        }

        void ContextMenuOpenedExecute(RoutedEventArgs e)
        {
            ContextMenu contextMenu = e.Source as ContextMenu;                      // change to custom control in future - temporary workaround

            contextMenu.IsOpen = this.IsEditState;
        }

        void SaveGraphExecute()
        {
            string path = Path.GetFullPath("../../CustomGraphs/");                  // Can bug in release, test it

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.InitialDirectory = path;
            saveDialog.Filter = "Custom graph file (*.huu)|*.huu";

            if (saveDialog.ShowDialog() == true)
            {
                List<Graph> graphsList = new List<Graph>();                         // Graphs will be initially stored as list - algorithm views will read random graph from it,
                                                                                    // and user graphs will always be the first and only element in the list
                Graph graph = new Graph();
                graph.CanvasGraph = CanvasItems;

                for (int i = 0; i < Colors.Count; i++)
                {
                    if (Colors[i] == Brushes.Green)
                        graph.AlgorithmsSupported[i] = true;
                }

                graphsList.Add(graph);
                CustomXmlSerializer xmlSerializer = new CustomXmlSerializer( typeof(List<Graph>) );

                using ( StringWriter writer = new StringWriter() )
                {
                    xmlSerializer.Serialize(writer, graphsList);

                    string plainXml = writer.GetStringBuilder().ToString();
                    string encryptedXml = StringEncryption.Encrypt(plainXml, "Yaranaika?");
                    //string encryptedXml = plainXml;

                    //----------------

                    //string dataPath = Path.GetFullPath("../../CustomGraphs/Prawdziwe algorytmy - learning/Data.huu");
                    //string encryptedDataPath = Path.GetFullPath("../../CustomGraphs/Prawdziwe algorytmy - learning/EncryptedData.huu");

                    //string dataPath = Path.GetFullPath("../../CustomGraphs/Prawdziwe algorytmy - egzamin/Data.huu");
                    //string encryptedDataPath = Path.GetFullPath("../../CustomGraphs/Prawdziwe algorytmy - egzamin/EncryptedData.huu");

                    //plainXml = File.ReadAllText(dataPath);
                    //encryptedXml = StringEncryption.Encrypt(plainXml, "Yaranaika?");

                    //File.WriteAllText(encryptedDataPath, encryptedXml);

                    //----------------

                    File.WriteAllText(saveDialog.FileName, encryptedXml);
                }
            }
        }

        void LoadGraphExecute()
        {
            string path = Path.GetFullPath("../../CustomGraphs/");                  // Can bug in release, test it

            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.InitialDirectory = path;
            openDialog.Filter = "Custom graph file (*.huu)|*.huu";

            if (openDialog.ShowDialog() == true)
            {
                CustomXmlSerializer xmlSerializer = new CustomXmlSerializer( typeof(List<Graph>) );

                try
                {
                    string encryptedXml = File.ReadAllText(openDialog.FileName);
                    string plainXml = StringEncryption.Decrypt(encryptedXml, "Yaranaika?");
                    //string plainXml = encryptedXml;

                    //-------------------

                    //string dataPath = Path.GetFullPath("../../CustomGraphs/FullData.huu");
                    //plainXml = File.ReadAllText(dataPath);

                    //encryptedXml = StringEncryption.Encrypt(plainXml, "Yaranaika?");

                    //-------------------

                    using (StringReader reader = new StringReader(plainXml))
                    {
                        List<Graph> graphsList = ( (List<Graph>) xmlSerializer.Deserialize(reader) );

                        if (graphsList.Count != 1)
                            throw new Exception("W pliku powinien być dokładnie jeden graf.");

                        //Graph graph = ((List<Graph>)xmlSerializer.Deserialize(reader))[0];
                        Graph graph = graphsList[0];

                        //this.CanvasItems.Clear();
                        this.CanvasItems = graph.CanvasGraph;

                        for (int i = 0; i < Colors.Count; i++)
                        {
                            Colors[i] = graph.AlgorithmsSupported[i] ?
                                Brushes.Green : Brushes.Red;
                        }

                        RaisePropertyChanged("CanSaveGraph");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Wystąpił błąd podczas wczytywania pliku. Upewnij się, że nie jest on uszkodzony.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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
        public ICommand SaveGraph
        {
            get { return new RelayCommand<object>( p => SaveGraphExecute() ); }
        }
        public ICommand LoadGraph
        {
            get { return new RelayCommand<object>( p => LoadGraphExecute() ); }
        }

        public ICommand ClickRectangle
        {
            get { return new RelayCommand<int>(param => ClickRectangleExecute(param), CanClickRectangle); }
        }

        public ICommand ContextMenuOpened
        {
            get { return new RelayCommand<RoutedEventArgs>( p => ContextMenuOpenedExecute(p) ); }
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
