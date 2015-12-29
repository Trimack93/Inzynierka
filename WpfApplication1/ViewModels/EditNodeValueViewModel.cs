using Common.Models;
using Common.Utilities;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApplication1.ViewModels
{
    public class EditNodeValueViewModel : BaseNotifyPropertyChanged, IModalDialogViewModel
    {
        public EditNodeValueViewModel()
        {
            this.Node.Value = 0;
        }
        public EditNodeValueViewModel(object currentValue)
        {
            this.Node.Value = currentValue;
        }

        //----------------------------------

        private Node _Node = new Node();
        private bool? dialogResult = false;

        //----------------------------------

        public Node Node
        {
            get { return _Node; }
            set
            {
                _Node = value;
                RaisePropertyChanged("Node");
            }
        }
        public bool? DialogResult
        {
            get { return dialogResult; }
            private set
            {
                dialogResult = value;
                RaisePropertyChanged("DialogResult");
            }
        }

        //----------------------------------

        void OkClickExecute()
        {
            this.DialogResult = true;
        }

        //----------------------------------

        public ICommand OkClick
        {
            get { return new RelayCommand<object>( p => OkClickExecute() ); }
        }
    }
}
