using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.ViewModels
{
    public class LearningViewModel : INotifyPropertyChanged
    {
        public string AlgorithmName { get; private set; } = "[Nazwa algorytmu]";
        public string Instructions { get; set; } = @"Instrukcja dla aktualnie wykonywanej sekwencji. asdf asdf
                                   Test nowej linii
                                   fdsgsdfg";

        public LearningViewModel()
        {
        }
        public LearningViewModel(string algorithmName)
        {
            this.AlgorithmName = algorithmName;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;              // Take a copy to prevent thread issues

            if (handler != null)
                handler( this, new PropertyChangedEventArgs(propertyName) );
        }
    }
}
