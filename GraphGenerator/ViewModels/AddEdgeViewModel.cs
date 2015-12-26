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
        public AddEdgeViewModel() : this(true) { }

        public AddEdgeViewModel(bool canEdgeBeBidirectional)
        {
            this.Edge.Value = "";
            this.CanAcceptBidirectionalEdge = canEdgeBeBidirectional;
        }

        //----------------------------------

        private Edge _Edge = new Edge();
        private bool? dialogResult = false;
        private bool _canAcceptInput = true;
        private bool _canAcceptBidirectionalEdge = true;

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
        public bool CanAcceptBidirectionalEdge
        {
            get { return _canAcceptBidirectionalEdge; }
            set
            {
                _canAcceptBidirectionalEdge = value;
                RaisePropertyChanged("CanAcceptBidirectionalEdge");
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
