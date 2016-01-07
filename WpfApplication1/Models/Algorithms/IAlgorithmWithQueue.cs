using Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Models.Algorithms
{
    interface IAlgorithmWithQueue
    {
        ObservableCollection<ComboboxElement> NodesQueue { get; set; }
    }
}
