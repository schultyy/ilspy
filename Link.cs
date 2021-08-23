using System.Text.Json.Serialization;

namespace ilspy
{
    public class Link
    {
        [JsonPropertyName("source")]
        public string Source { get; private set; }

        [JsonPropertyName("target")]
        public string Target { get; private set; }

        [JsonPropertyName("value")]
        public int Value { get; private set; }

        public Link(string source, string target, int value)
        {
            this.Source = source;
            this.Target = target;
            this.Value = value;
        }
    }
}