using System.Text.Json.Serialization;

namespace CountryUniversities.DataModels.DTOs;

public class UniversityDTO
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("web_pages")]
    public ICollection<string> WebPages { get; set; }
}