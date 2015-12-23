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

        public EditEdgeViewModel(string edgeValueToEdit) : base(true)
        {
            this.Edge.Value = edgeValueToEdit;
        }
    }
}
