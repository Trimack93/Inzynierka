using Common.Utilities;
using Common.ViewModels;
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
        public ObservableCollection<SolidColorBrush> Colors
        {
            get { return _Colors; }
        }

        public MainViewModel()
        {
            _Colors.Add( Brushes.Green );
            _Colors.Add( Brushes.Red );
            _Colors.Add( Brushes.Green );
            _Colors.Add( Brushes.Green );
            _Colors.Add( Brushes.Red);
            _Colors.Add( Brushes.Green );
            _Colors.Add( Brushes.Red);
        }

        // GUI update test
        void ChangeCompatibilityExecute()
        {
            if (_Colors[2] == Brushes.Green)
                _Colors[2] = Brushes.Red;
            else
                _Colors[2] = Brushes.Green;
        }

        bool CanChangeCompatibility()
        {
            return true;
        }

        public ICommand ChangeCompatibility
        {
            get { return new RelayCommand(ChangeCompatibilityExecute, CanChangeCompatibility); }
        }
    }
}
