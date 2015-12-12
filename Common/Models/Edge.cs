﻿using Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Common.Models
{
    //public enum Direction
    //{
    //    Onedirectional, Bidirectional                                           // Probably won't be used at all
    //}
    
    // Krawędź
    public class Edge : BaseNotifyPropertyChanged
    {
        private int _ID;

        private string _Value;
        private SolidColorBrush _Color;
        private double _Thickness;

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
        public SolidColorBrush Color
        {
            get { return _Color; }
            set
            {
                _Color = value;
                RaisePropertyChanged("Color");
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
        //public Direction Direction { get; set; }

        //---------------------------------

        public List<int> NodesID { get; set; } = new List<int>(2);      // Stores the IDs of the nodes it's connecting - to avoid circular dependency
    }
}
