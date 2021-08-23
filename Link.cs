using System.Text.Json.Serialization;

namespace ilspy
{
    public class Link
    {
        [JsonPropertyName("source")]
        public int Source { get; private set; }

        [JsonPropertyName("target")]
        public int Target { get; private set; }

        public Link(int source, int target)
        {
            this.Source = source;
            this.Target = target;
        }
    }
}