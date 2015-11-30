﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Common.Models
{

    // Wierzchołek
    public class Node
    {
        public int ID { get; set; }

        public string Name { get; set; }                // a, b, c, d etc.
        public object Value { get; set; }               // 7, 6/9 or abcd
        public Color Color { get; set; }
        
        // Position (probably for deletion)
        //public int X { get; set; }                      // Grid column
        //public int Y { get; set; }                      // Grid row

        public HorizontalAlignment NameHorizontalAlignment { get; set; }
        public VerticalAlignment NameVerticalAlignment { get; set; }

        public List<Edge> Edges { get; set; }
    }
}
