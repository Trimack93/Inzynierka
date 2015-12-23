using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphGenerator.ViewModels
{
    public class EditEdgeViewModel : AddEdgeViewModel
    {
        public EditEdgeViewModel() : base(true) { }

        public EditEdgeViewModel(Edge edgeToEdit, bool canEdgeBeBidirectional) : base(canEdgeBeBidirectional)
        {
            this.Edge.Value = edgeToEdit.Value;
            this.Edge.IsBidirectional = edgeToEdit.IsBidirectional;

            this.IsBidirectional = Edge.IsBidirectional;
            this.IsOneDirectional = !Edge.IsBidirectional;
        }

        //----------------------------------

        private bool _isOneDirectional;
        private bool _isBidirectional;

        //----------------------------------

        public bool IsOneDirectional
        {
            get { return _isOneDirectional; }
            set
            {
                _isOneDirectional = value;
                RaisePropertyChanged("IsOneDirectional");
            }
        }
        public bool IsBidirectional
        {
            get { return _isBidirectional; }
            set
            {
                _isBidirectional = value;
                RaisePropertyChanged("IsBidirectional");
            }
        }
    }
}
