using CountryUniversities.DataModels;

namespace CountryUniversities.Repositories
{
    public interface IUniversityRepository
    {
        Task AddUniversitiesAsync(List<University> universities);
        Task<List<University>> GetUniversitiesAsync(string? country, string? name);
        Task<bool> DoesCountryAlreadyExist(string country);
    }
}
