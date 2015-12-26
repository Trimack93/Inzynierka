using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using Common.Utilities;
using System.Xml.Serialization;

namespace Common.Models
{
    // Wierzchołek
    public class Node : BaseNotifyPropertyChanged
    {
        public Node()
        {
            Thickness = 1.0;
            Color = Brushes.Transparent;
        }

        //---------------------------------

        private int _ID;

        private string _Name;
        private object _Value;
        private SolidColorBrush _Color;
        private double _Thickness;

        private HorizontalAlignment _NameHorizontalAlignment;
        private VerticalAlignment _NameVerticalAlignment;

        private bool _canChangeHorizontalAlignment = true;

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

                if (Name.Length > 2)
                    CanChangeHorizontalAlignment = false;
                else
                    CanChangeHorizontalAlignment = true;
            }
        }
        public object Value                         // 7, 6/9 or abcd
        {
            get { return _Value; }
            set
            {
                _Value = value;
                RaisePropertyChanged("Value");
            }
        }
        public double Thickness                     // Node circle thickness
        {
            get { return _Thickness; }
            set
            {
                _Thickness = value;
                RaisePropertyChanged("Thickness");
            }
        }
        public Color SerializedColor { get; set; }  // Color which will be serialized into XML file

        [XmlIgnore]
        public SolidColorBrush Color                // Node color (used in some algorithms)
        {
            get { return _Color; }
            set
            {
                _Color = value;
                RaisePropertyChanged("Color");

                SerializedColor = value.Color;
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

        public bool CanChangeHorizontalAlignment
        {
            get { return _canChangeHorizontalAlignment; }
            set
            {
                _canChangeHorizontalAlignment = value;
                RaisePropertyChanged("CanChangeHorizontalAlignment");
            }
        }

        public List<Edge> Edges { get; set; } = new List<Edge>();
    }
}
