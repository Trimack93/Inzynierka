using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using Common.Utilities;

namespace Common.Models
{
    // Wierzchołek
    public class Node : BaseNotifyPropertyChanged
    {
        private int _ID;

        private string _Name;
        private object _Value;
        private Color _Color;

        private HorizontalAlignment _NameHorizontalAlignment;
        private VerticalAlignment _NameVerticalAlignment;

        //---------------------------------

        public int ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                RaisePropertyChanged("ID");
            }
        }

        public string Name                          // a, b, c, d etc.
        {
            get { return _Name; }
            set
            {
                _Name = value;
                RaisePropertyChanged("Name");
            }
        }
        public object Value                         // 7, 6/9 or abcd
        {
            get { return _Value; }
            set
            {
                if (value.ToString().Length <= 3)               // Value must contain max 3 chars
                    _Value = value;
                else
                {
                    string val = value.ToString();

                    _Value = val.Remove(val.Length - 1, 1);     // Remove last character
                }

                RaisePropertyChanged("Value");
            }
        }
        public Color Color                          // Node color (used in some algorithms)
        {
            get { return _Color; }
            set
            {
                _Color = value;
                RaisePropertyChanged("Color");
            }
        }
        
        public HorizontalAlignment NameHorizontalAlignment
        {
            get { return _NameHorizontalAlignment; }
            set
            {
                _NameHorizontalAlignment = value;
                RaisePropertyChanged("NameHorizontalAlignment");
            }
        }
        public VerticalAlignment NameVerticalAlignment
        {
            get { return _NameVerticalAlignment; }
            set
            {
                _NameVerticalAlignment = value;
                RaisePropertyChanged("NameVerticalAlignment");
            }
        }

        public List<Edge> Edges { get; set; }           // Make it ObservableCollection?
    }
}
