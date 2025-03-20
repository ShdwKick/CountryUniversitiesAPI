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
    
    // Конвертация в сущность EF
    public University ToEntity()
    {
        return new University
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Country = Country,
            WebPages = WebPages.Select(url => new WebPage
            {
                Id = Guid.NewGuid(),
                Url = url
            }).ToList()
        };
    }

    // Конвертация из сущности EF
    public static UniversityDTO FromEntity(University university)
    {
        return new UniversityDTO
        {
            Name = university.Name,
            Country = university.Country,
            WebPages = university.WebPages.Select(wp => wp.Url).ToList()
        };
    }
}