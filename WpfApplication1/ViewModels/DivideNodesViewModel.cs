using Common.Models;
using Common.Utilities;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApplication1.Models.Algorithms;

namespace WpfApplication1.ViewModels
{
    public class DivideNodesViewModel : BaseNotifyPropertyChanged, IModalDialogViewModel
    {
        public DivideNodesViewModel()
        {
            NodesList.Add( new Node() { Name = "a" } );
            FirstQueue.Add( new ComboboxElement(0) );
            SecondQueue.Add( new ComboboxElement(0) );
        }

        public DivideNodesViewModel(List<Node> nodesList)
        {
            this.NodesList = nodesList;

            FirstQueue.Add( new ComboboxElement(0) );
            SecondQueue.Add( new ComboboxElement(0) );
        }

        //----------------------------------
        
        private bool? dialogResult = false;
        private ObservableCollection<ComboboxElement> _firstQueue = new ObservableCollection<ComboboxElement>();
        private ObservableCollection<ComboboxElement> _secondQueue = new ObservableCollection<ComboboxElement>();

        //----------------------------------

        public bool? DialogResult
        {
            get { return dialogResult; }
            private set
            {
                dialogResult = value;
                RaisePropertyChanged("DialogResult");
            }
        }

        public ObservableCollection<ComboboxElement> FirstQueue
        {
            get { return _firstQueue; }
            set
            {
                _firstQueue = value;
                RaisePropertyChanged("FirstQueue");
            }
        }
        public ObservableCollection<ComboboxElement> SecondQueue
        {
            get { return _secondQueue; }
            set
            {
                _secondQueue = value;
                RaisePropertyChanged("SecondQueue");
            }
        }

        public List<Node> NodesList { get; set; } = new List<Node>();

        //----------------------------------

        void OkClickExecute()
        {
            if ( Bipartition.AreQueuesCorrect(this.FirstQueue, this.SecondQueue, NodesList) )
                this.DialogResult = true;
            else
            {
                MessageBox.Show("Zbiory nie zostały wybrane poprawnie. Spróbuj ponownie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);

                this.FirstQueue.Clear();
                this.SecondQueue.Clear();
                FirstQueue.Add( new ComboboxElement(0) );
                SecondQueue.Add( new ComboboxElement(0) );
            }
        }

        //----------------------------------

        public ICommand OkClick
        {
            get { return new RelayCommand<object>(p => OkClickExecute()); }
        }
    }
}
