using Common.Utilities;
using Common.ViewModels;
using GraphGenerator.Models;
using GraphGenerator.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace GraphGenerator.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<SolidColorBrush> _Colors = new ObservableCollection<SolidColorBrush>();

        private bool _NodeButtonIsPressed = false;
        private bool _EdgeButtonIsPressed = false;

        public ObservableCollection<SolidColorBrush> Colors
        {
            get { return _Colors; }
            private set { _Colors = value; }
        }

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

        public MainViewModel()
        {
            Colors.Add( Brushes.Green );
            Colors.Add( Brushes.Red );
            Colors.Add( Brushes.Green );
            Colors.Add( Brushes.Green );
            Colors.Add( Brushes.Red);
            Colors.Add( Brushes.Green );
            Colors.Add( Brushes.Red);   
        }

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

        bool CanAddNode()
        {
            return true;                        // O
        }

        bool CanAddEdge()
        {
            return true;                        // M
        }

        bool CanChangeCompatibility()
        {
            return true;                        // G
        }

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
