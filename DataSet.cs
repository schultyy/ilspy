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

        public DataSet(List<DependencyGraph> dependencyGraphs)
        {
            this.Nodes = new List<Node>();
            this.Links = new List<Link>();

            for (int i = 0; i < dependencyGraphs.Count; i++)
            {
                var currentGraph = dependencyGraphs[i];
                Nodes.Add(new Node(currentGraph.RootNode, i));
                foreach (var dependency in currentGraph.Dependencies)
                {
                    Nodes.Add(new Node(dependency, i));
                    Links.Add(new Link(dependency, currentGraph.RootNode, i));
                }
            }
        }
    }
}