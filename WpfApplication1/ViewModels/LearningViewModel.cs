using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Utilities;

namespace WpfApplication1.ViewModels
{
    public class LearningViewModel : BaseNotifyPropertyChanged
    {
        public LearningViewModel() : this("[Nazwa algorytmu]") { }

        public LearningViewModel(string algorithmName)
        {
            AlgorithmName = algorithmName;
            //IsStopButtonVisible = true;
        }

        public string AlgorithmName { get; private set; }
        public string Instructions { get; set; } = @"Instrukcja dla aktualnie wykonywanej sekwencji. asdf asdf
                                   Test nowej linii
                                   fdsgsdfg";                                   // move to model of each algorithm

        private bool _isStopButtonVisible { get; set; }
        private bool _isNodeNamesControlVisible { get; set; }

        public bool IsStopButtonVisible
        {
            get { return _isStopButtonVisible; }
            set
            {
                _isStopButtonVisible = value;
                RaisePropertyChanged("IsStopButtonVisible");
                RaisePropertyChanged("IsAdditionalControlVisible");
            }
        }
        public bool IsNodeNamesControlVisible
        {
            get { return _isNodeNamesControlVisible; }
            set
            {
                _isNodeNamesControlVisible = value;
                RaisePropertyChanged("IsNodeNamesControlVisible");
                RaisePropertyChanged("IsAdditionalControlVisible");
            }
        }
        public bool IsAdditionalControlVisible
        {
            get { return IsStopButtonVisible || IsNodeNamesControlVisible; }
        }
    }
}
