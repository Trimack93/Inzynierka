using Common.Models;
using Common.Models.Canvas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Common.Utilities
{
    /// <summary>
    /// Custom deserializer which repairs problem with SolidColorBrush deserialization.
    /// </summary>
    public class CustomXmlSerializer : XmlSerializer
    {
        public CustomXmlSerializer() : base() { }
        public CustomXmlSerializer(Type type) : base(type) { }

        /// <summary>
        /// Custom deserialize method that restores proper value of SolidColorBrush in deserialized objects.
        /// </summary>
        public new object Deserialize(TextReader reader)
        {
            var result = base.Deserialize(reader);

            if (result is List<Graph>)
            {
                List<Graph> graphsList = result as List<Graph>;

                foreach (Graph graph in graphsList)
                {
                    ObservableCollection<CanvasControlBase> items = graph.CanvasGraph;

                    List<Edge> edgesList = items.GetAllEdges();
                    List<Node> nodesList = items.GetAllNodes();

                    foreach (Edge edge in edgesList)
                    {
                        if (edge.Color.Color != edge.SerializedColor)
                            edge.Color = new SolidColorBrush(edge.SerializedColor);
                    }

                    foreach (Node node in nodesList)
                    {
                        if (node.Color.Color != node.SerializedColor)
                            node.Color = new SolidColorBrush(node.SerializedColor);

                        // Cryptographic algorithm can't resolve Unicode characters, they are encrypted as "?"
                        if (node.Value.ToString() == "?")
                            node.Value = "∞";
                    }
                }
            }

            return result;
        }
    }
}
