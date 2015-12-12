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
                    CanvasRectangles.Add(new CanvasRectangle(i * columnsCount + j, j * this.RectangleSize, i * this.RectangleSize, this.RectangleSize));
                }

            (CanvasRectangles[1] as CanvasRectangle).Node = new Node()
                { ID = 0, Name = "a", Value = 6, NameHorizontalAlignment = HorizontalAlignment.Center, NameVerticalAlignment = VerticalAlignment.Top };

            (CanvasRectangles[69] as CanvasRectangle).Node = new Node()
                { ID = 1, Name = "b", Value = 9, NameHorizontalAlignment = HorizontalAlignment.Right, NameVerticalAlignment = VerticalAlignment.Center };

            (CanvasRectangles[24] as CanvasRectangle).Node = new Node()
                { ID = 2, Name = "c", Value = 15, NameHorizontalAlignment = HorizontalAlignment.Left, NameVerticalAlignment = VerticalAlignment.Center };

            (CanvasRectangles[15] as CanvasRectangle).Node = new Node()
                { ID = 3, Name = "d", Value = 666, NameHorizontalAlignment = HorizontalAlignment.Center, NameVerticalAlignment = VerticalAlignment.Bottom };

            (CanvasRectangles[1] as CanvasRectangle).DoesContainNode = true;
            (CanvasRectangles[69] as CanvasRectangle).DoesContainNode = true;
            (CanvasRectangles[24] as CanvasRectangle).DoesContainNode = true;
            (CanvasRectangles[15] as CanvasRectangle).DoesContainNode = true;

            //--------------------

            Edge randomEdge = new Edge()
            {
                ID = 0, Thickness = 2, Color = Brushes.Green, Value = "15"
            };

            Edge anotherRandomEdge = new Edge()
            {
                ID = 1, Thickness = 2, Color = Brushes.Red, Value = "43"
            };

            CanvasRectangles.Add(new CanvasEdge(150, 100, 60, 150, randomEdge));
            CanvasRectangles.Add(new CanvasEdge(400, 230, 475, 300, anotherRandomEdge));

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
        private ObservableCollection<CanvasControlBase> _CanvasRectangles = new ObservableCollection<CanvasControlBase>();

        private readonly IDialogService dialogService;

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
        public ObservableCollection<CanvasControlBase> CanvasRectangles
        {
            get { return _CanvasRectangles; }
            private set { _CanvasRectangles = value; }
        }

        //----------------------------------

        private void AddNode(int rectangleID)
        {
            CanvasRectangle rectangle = CanvasRectangles[rectangleID] as CanvasRectangle;

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
                        Name = node.Name,
                        Value = node.Value,
                        NameHorizontalAlignment = node.NameHorizontalAlignment,
                        NameVerticalAlignment = node.NameVerticalAlignment
                    };

                    rectangle.DoesContainNode = true;
                }
            }
        }

        private void AddEdge(int ID)
        {
            //TODO
            if ( (CanvasRectangles[ID] as CanvasRectangle).DoesContainNode )
                MessageBox.Show("Wait for it", "Work in progress", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
            if (this.EdgeButtonIsPressed)
                AddEdge(rectangleID);
        }

        //bool CanChangeCompatibility()
        //{
        //    return true;                        // O
        //}
        //bool CanClickAddNode()
        //{
        //    return true;                        // M
        //}
        //bool CanClickAddEdge()
        //{
        //    return true;                        // G
        //}

        bool CanClickRectangle()
        {
            return this.EdgeButtonIsPressed || this.NodeButtonIsPressed;
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
    }
}
