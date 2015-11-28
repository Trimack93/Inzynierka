using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Common.Models
{
    public enum Direction
    {
        AB, BA, Bidirectional                                           // Probably will stick with Onedirectional, Bidirectional
    }
    
    // Krawędź
    public class Edge
    {
        public int ID { get; set; }
        public int Value { get; set; }
        public Color Color { get; set; }                                // Maybe also thickness?
        public Direction Direction { get; set; }
        public List<int> NodesID { get; set; } = new List<int>(2);      // Stores the IDs of the nodes it's connecting - to avoid circular dependency
    }
}
