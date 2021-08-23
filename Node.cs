using System.Text.Json.Serialization;

namespace ilspy
{
    public class Node
    {
        [JsonPropertyName("id")]
        public string Id { get; private set; }

        [JsonPropertyName("group")]
        public int Group { get; private set; }

        public Node(string id, int group)
        {
            this.Id = id;
            this.Group = group;
        }
    }
}