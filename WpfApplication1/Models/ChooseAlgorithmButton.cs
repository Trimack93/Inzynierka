using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApplication1.Models
{
    public class ChooseAlgorithmButton : BaseNotifyPropertyChanged
    {
        public ChooseAlgorithmButton(string algorithmName)
        {
            this.AlgorithmName = algorithmName;
            this.IsEnabled = true;
        }

        private bool _isEnabled;
        private string _algorithmName;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged("IsEnabled");
            }
        }
        public string AlgorithmName
        {
            get { return _algorithmName; }
            set
            {
                _algorithmName = value;
                RaisePropertyChanged("AlgorithmName");
            }
        }
    }
}
