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

using Common.Utilities;
using Common.ViewModels;
using GraphGenerator.Models;
using Common.Models;

namespace GraphGenerator.ViewModels
{
    public class MainViewModel : BaseViewModel
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
                    CanvasRectangles.Add(new CanvasRectangle(j * this.RectangleSize, i * this.RectangleSize, this.RectangleSize));
                }
            
            CanvasRectangles[0].Node = new Node();
            CanvasRectangles[69].Node = new Node();
            CanvasRectangles[24].Node = new Node();
            CanvasRectangles[15].Node = new Node();

            CanvasRectangles[0].DoesContainNode = true;
            CanvasRectangles[69].DoesContainNode = true;
            CanvasRectangles[24].DoesContainNode = true;
            CanvasRectangles[15].DoesContainNode = true;

            CanvasRectangles[0].Node.Name = "a";
            CanvasRectangles[69].Node.Name = "b";
            CanvasRectangles[24].Node.Name = "c";
            CanvasRectangles[15].Node.Name = "d";

            CanvasRectangles[0].Node.Value = 6;
            CanvasRectangles[69].Node.Value = 9;
            CanvasRectangles[24].Node.Value = 15;
            CanvasRectangles[15].Node.Value = 666;

            CanvasRectangles[0].Node.NameHorizontalAlignment = HorizontalAlignment.Center;
            CanvasRectangles[69].Node.NameHorizontalAlignment = HorizontalAlignment.Right;
            CanvasRectangles[24].Node.NameHorizontalAlignment = HorizontalAlignment.Left;
            CanvasRectangles[15].Node.NameHorizontalAlignment = HorizontalAlignment.Center;

            CanvasRectangles[0].Node.NameVerticalAlignment = VerticalAlignment.Top;
            CanvasRectangles[69].Node.NameVerticalAlignment = VerticalAlignment.Center;
            CanvasRectangles[24].Node.NameVerticalAlignment = VerticalAlignment.Center;
            CanvasRectangles[15].Node.NameVerticalAlignment = VerticalAlignment.Bottom;
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

        // GUI update test
        void ChangeCompatibilityExecute()
        {
            if (Colors[2] == Brushes.Green)
                Colors[2] = Brushes.Red;
            else
                Colors[2] = Brushes.Green;
        }
        void AddNodeExecute()
        {
            EdgeButtonIsPressed = false;
            NodeButtonIsPressed = !NodeButtonIsPressed;
        }
        void AddEdgeExecute()
        {
            NodeButtonIsPressed = false;
            EdgeButtonIsPressed = !EdgeButtonIsPressed;
        }

        bool CanChangeCompatibility()
        {
            return true;                        // O
        }
        bool CanAddNode()
        {
            return true;                        // M
        }
        bool CanAddEdge()
        {
            return true;                        // G
        }

        //----------------------------------

        public ICommand AddNode
        {
            get { return new RelayCommand(AddNodeExecute, CanAddNode); }
        }
        public ICommand AddEdge
        {
            get { return new RelayCommand(AddEdgeExecute, CanAddEdge); }
        }
        public ICommand ChangeCompatibility
        {
            get { return new RelayCommand(ChangeCompatibilityExecute, CanChangeCompatibility); }
        }
    }
}
