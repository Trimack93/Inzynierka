using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Models
{
    public class NodesForest
    {
        public NodesForest(Node node)
        {
            this.ID = _instanceCounter++;
            this.Node = node;
        }
        
        public int ID { get; set; }
        public Node Node { get; set; }                          // TODO: change to list of nodes

        private static int _instanceCounter = 0;
    }
}
