using CountryUniversities.DataModels;
using CountryUniversities.DataModels.DTOs;

namespace CountryUniversities.Services
{
    public interface IUniversityService
    {
        Task<List<UniversityDTO>> DeserializeCountryUniversitiesAsync(string jsonContent);
        Task ExtractDataForCountriesAsync();
        Task<string> ExtractDataForCountryAsync(string country);
        Task<List<UniversityDTO>> GetUniversitiesAsync(string? country, string? name);
    }
}
