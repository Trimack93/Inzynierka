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
using GraphGenerator.Views;                 // MVVM rape xD

namespace GraphGenerator.ViewModels
{
    public class MainViewModel : BaseNotifyPropertyChanged
    {
        public MainViewModel()
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
                    CanvasRectangles.Add(new CanvasRectangle(j * this.RectangleSize, i * this.RectangleSize, this.RectangleSize, i*11 + j));
                }

            CanvasRectangles[0].Node = new Node()
                { Name = "a", Value = 6, NameHorizontalAlignment = HorizontalAlignment.Center, NameVerticalAlignment = VerticalAlignment.Top };

            CanvasRectangles[69].Node = new Node()
                { Name = "b", Value = 9, NameHorizontalAlignment = HorizontalAlignment.Right, NameVerticalAlignment = VerticalAlignment.Center };

            CanvasRectangles[24].Node = new Node()
                { Name = "c", Value = 15, NameHorizontalAlignment = HorizontalAlignment.Left, NameVerticalAlignment = VerticalAlignment.Center };

            CanvasRectangles[15].Node = new Node()
                { Name = "d", Value = 666, NameHorizontalAlignment = HorizontalAlignment.Center, NameVerticalAlignment = VerticalAlignment.Bottom };

            CanvasRectangles[0].DoesContainNode = true;
            CanvasRectangles[69].DoesContainNode = true;
            CanvasRectangles[24].DoesContainNode = true;
            CanvasRectangles[15].DoesContainNode = true;
        }

        //----------------------------------

        private bool _NodeButtonIsPressed = false;
        private bool _EdgeButtonIsPressed = false;

        private ObservableCollection<SolidColorBrush> _Colors = new ObservableCollection<SolidColorBrush>();
        private ObservableCollection<CanvasRectangle> _CanvasRectangles = new ObservableCollection<CanvasRectangle>();

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
        public ObservableCollection<CanvasRectangle> CanvasRectangles
        {
            get { return _CanvasRectangles; }
            private set { _CanvasRectangles = value; }
        }

        //----------------------------------

        private void AddNode(int rectangleID)
        {
            if (CanvasRectangles[rectangleID].DoesContainNode == false)
            {
                // temporary call
                AddNodeView nodeView = new AddNodeView();
                nodeView.ShowDialog();

                CanvasRectangles[rectangleID].Node = new Node()
                {
                    Name = "test",
                    Value = 12,
                    NameHorizontalAlignment = HorizontalAlignment.Center,
                    NameVerticalAlignment = VerticalAlignment.Top
                };

                CanvasRectangles[rectangleID].DoesContainNode = true;
            }
        }

        private void AddEdge(int ID)
        {
            //TODO
            if (CanvasRectangles[ID].DoesContainNode)
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
