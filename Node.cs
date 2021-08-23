using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ilspy
{
    public class Node
    {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public String Name { get; set; }

        [JsonIgnore]
        public List<Node> Children { get; set; }

        public Node(int id, String name)
        {
            this.Id = id;
            this.Name = name;
            this.Children = new List<Node>();
        }
    }
}