using System.Text.Json.Serialization;

namespace task13;

public class Student
{
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;
    
    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("middle_name")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]    //Игнорирование Null-значений.
    public string? MiddleName {get; set;}

    [JsonPropertyName("birth_date")]
    [JsonConverter(typeof(CustomDateTime))]
    public DateTime BirthDate { get; set; }

    [JsonPropertyName("subjects")]
    public List<Subject> Subjects { get; set; } = new();
}
