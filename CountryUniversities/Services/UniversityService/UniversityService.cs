using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;
using CountryUniversities.DataModels;
using CountryUniversities.DataModels.DTOs;
using CountryUniversities.Repositories;

namespace CountryUniversities.Services
{
    public class UniversityService : IUniversityService
    {
        private readonly int _threadsCount;

        private readonly List<string> _countries = new List<string>()
        {
            "Afghanistan", "Russian Federation", "India", "Australia", "China", "Japan", "France", "Germany", "Brazil",
            "Canada"
        };

        private static SemaphoreSlim _semaphore;
        private readonly IUniversityRepository _universityRepository;

        public UniversityService(IUniversityRepository universityRepository, IConfiguration config)
        {
            _threadsCount = config.GetSection("AppSettings:ThreadsCount").Get<int>();
            _semaphore = new SemaphoreSlim(_threadsCount == 0 ? Environment.ProcessorCount : _threadsCount);
            _universityRepository = universityRepository;
        }


        public async Task<List<UniversityDTO>> DeserializeCountryUniversitiesAsync(string jsonContent)
        {
            List<UniversityDTO> universities = JsonSerializer.Deserialize<List<UniversityDTO>>(jsonContent);
            if(universities.Count == 0)
                return new List<UniversityDTO>();
            
            return universities;
        }

        public async Task ExtractDataForCountriesAsync()
        {
            Task[] tasks = new Task[_countries.Count];
            
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    var content = await ExtractDataForCountryAsync(_countries[i]);
                    var universities = await DeserializeCountryUniversitiesAsync(content);
                });
            }
            await Task.WhenAll(tasks);
        }

        public async Task<string> ExtractDataForCountryAsync(string country)
        {
            using (var httpClient = new HttpClient())
            {
                var response =
                    await httpClient.GetAsync(
                        $"http://universities.hipolabs.com/search?country={country.Replace(' ', '+')}");
                response.EnsureSuccessStatusCode();
                
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}