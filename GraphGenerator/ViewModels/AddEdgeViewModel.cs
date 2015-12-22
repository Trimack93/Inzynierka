using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Common.Utilities;
using Common.Models;
using System.Windows.Media;
using System.Windows.Input;

namespace GraphGenerator.ViewModels
{
    public class AddEdgeViewModel : BaseNotifyPropertyChanged, IModalDialogViewModel
    {
        public AddEdgeViewModel() : this("0") { }

        public AddEdgeViewModel(string edgeValue)
        {
            Edge.Value = edgeValue;
        }

        //----------------------------------

        private Edge _Edge = new Edge();
        private bool? dialogResult = false;
        private bool _canAcceptInput = true;

        //----------------------------------

        public Edge Edge
        {
            get { return _Edge; }
            set
            {
                _Edge = value;
                RaisePropertyChanged("Edge");
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
        public bool CanAcceptInput
        {
            get { return _canAcceptInput; }
            set
            {
                _canAcceptInput = value;
                RaisePropertyChanged("CanAcceptInput");
            }
        }

        //----------------------------------

        void OkClickExecute()
        {
            this.DialogResult = true;
        }

        void SelectDirectedEdgeTypeExecute()
        {
            this.Edge.IsBidirectional = false;
        }
        void SelectNonDirectedEdgeTypeExecute()
        {
            this.Edge.IsBidirectional = true;
        }

        //----------------------------------

        public ICommand OkClick
        {
            get { return new RelayCommand<object>(p => OkClickExecute()); }
        }
        public ICommand SelectDirectedEdgeType
        {
            get { return new RelayCommand<object>(p => SelectDirectedEdgeTypeExecute()); }
        }
        public ICommand SelectNonDirectedEdgeType
        {
            get { return new RelayCommand<object>(p => SelectNonDirectedEdgeTypeExecute()); }
        }
    }
}
