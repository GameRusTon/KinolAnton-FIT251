using System.Text.Json.Serialization;

namespace task13;

public class Subject
{
    [JsonPropertyName("name")]
    public string Name {get; set; } = string.Empty;

    [JsonPropertyName("grade")]
    public int Grade {get; set; }
}
