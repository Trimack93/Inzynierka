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

            CanvasRectangles[12].DoesContainNode = true;
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
