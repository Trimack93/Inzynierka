﻿using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Common.Models
{
    // Krawędź
    public class Edge : BaseNotifyPropertyChanged
    {
        public Edge()
        {
            Color = Brushes.Black;
            Thickness = 2.0;
        }
        public Edge(int ID) : this()
        {
            this.ID = ID;
        }

        //---------------------------------

        private int _ID;

        private string _Value;
        private SolidColorBrush _Color;
        private double _Thickness;
        private bool _IsBidirectional = false;

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
        public string Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                RaisePropertyChanged("Value");
            }
        }
        public double Thickness
        {
            get { return _Thickness; }
            set
            {
                _Thickness = value;
                RaisePropertyChanged("Thickness");
            }
        }
        public bool IsBidirectional
        {
            get { return _IsBidirectional; }
            set
            {
                _IsBidirectional = value;
                RaisePropertyChanged("IsBidirectional");
            }
        }
        public Color SerializedColor { get; set; }                      // Color which will be serialized into XML file

        [XmlIgnore]
        public SolidColorBrush Color
        {
            get { return _Color; }
            set
            {
                _Color = value;
                RaisePropertyChanged("Color");

                SerializedColor = value.Color;
            }
        }

        //---------------------------------

        public List<int> NodesID { get; set; } = new List<int>(2);      // Stores the IDs of the nodes it's connecting - to avoid circular dependency

        //---------------------------------
        
        /// <summary>
        /// Creates deep copy of current object.
        /// </summary>
        public Edge Clone()
        {
            Edge newEdge = new Edge();

            newEdge.ID = this.ID;
            newEdge.Value = this.Value;
            newEdge.Thickness = this.Thickness;
            newEdge.IsBidirectional = this.IsBidirectional;
            newEdge.Color = new SolidColorBrush(this.Color.Color);
            newEdge.SerializedColor = newEdge.Color.Color;

            foreach (int ID in NodesID)
                newEdge.NodesID.Add(ID);

            return newEdge;
        }

        //---------------------------------
        // While serializing object into XML, it's hashcode is also preserved.
        // After reboot of the application, object hashcodes change and default Equals method fails.

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Edge e = obj as Edge;

            if (e == null)
                return false;

            bool result = this.ID == e.ID &&
                this.Value == e.Value &&
                this.Thickness == e.Thickness &&
                this.IsBidirectional == e.IsBidirectional &&
                this.SerializedColor == e.SerializedColor &&
                this.Color == e.Color &&
                this.NodesID.SequenceEqual(e.NodesID);

            return result;
        }

        public static bool operator==(Edge a, Edge b)
        {
            if ( Object.ReferenceEquals(a, null) )
                return Object.ReferenceEquals(b, null);

            return a.Equals(b);
        }

        public static bool operator !=(Edge a, Edge b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return this.ID;
        }
    }
}
