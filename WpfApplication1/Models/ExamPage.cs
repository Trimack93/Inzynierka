using Common.Models.Canvas;
using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Models
{
    public class ExamPage : BaseNotifyPropertyChanged
    {
        private bool _isPassed = false;

        public ExamPage(ObservableCollection<CanvasControlBase> graph)
        {
            this.Graph = graph;
        }

        public ObservableCollection<CanvasControlBase> Graph { get; set; }
        public bool IsPassed
        {
            get { return _isPassed; }
            set
            {
                _isPassed = value;
                RaisePropertyChanged("IsPassed");
            }
        }
    }
}
