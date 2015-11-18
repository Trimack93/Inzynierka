using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.ViewModels
{
    public class LearningViewModel : BaseViewModel
    {
        public string AlgorithmName { get; private set; } = "[Nazwa algorytmu]";
        public string Instructions { get; set; } = @"Instrukcja dla aktualnie wykonywanej sekwencji. asdf asdf
                                   Test nowej linii
                                   fdsgsdfg";                                   // move to model of each algorithm

        public LearningViewModel()
        {
        }
        public LearningViewModel(string algorithmName)
        {
            this.AlgorithmName = algorithmName;
        }
    }
}
