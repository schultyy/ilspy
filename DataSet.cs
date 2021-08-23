using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ilspy
{
    public class DataSet
    {
        [JsonPropertyName("nodes")]
        public List<Node> Nodes { get; private set; }

        [JsonPropertyName("links")]
        public List<Link> Links { get; private set; }

        public DataSet(List<Node> nodes)
        {
            this.Nodes = new List<Node>();
            this.Links = new List<Link>();

            this.ProcessNodes(nodes);

            foreach (var node in nodes)
            {
                foreach (var nodeChild in node.Children)
                {
                    Links.Add(new Link(node.Id, nodeChild.Id));
                }
            }
        }

        private void ProcessNodes(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                this.Nodes.Add(node);
                this.ProcessNodes(node.Children);
            }
        }
    }
}