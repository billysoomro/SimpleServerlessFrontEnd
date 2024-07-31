using System.Text.Json.Serialization;

namespace SimpleServerlessFrontEnd.Models
{
    public class Guitar
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("make")]
        public string Make { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("shape")]
        public string Shape { get; set; }

        [JsonPropertyName("strings")]
        public int Strings { get; set; }
    }
}
