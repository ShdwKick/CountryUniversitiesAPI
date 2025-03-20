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
            _semaphore = new SemaphoreSlim(_threadsCount <= 0 ? Environment.ProcessorCount : _threadsCount);
            _universityRepository = universityRepository;
        }


        public async Task<List<UniversityDTO>> DeserializeCountryUniversitiesAsync(string jsonContent)
        {
            try
            {
                return JsonSerializer.Deserialize<List<UniversityDTO>>(jsonContent) ?? new List<UniversityDTO>();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Ошибка десериализации: {ex.Message}");
                return new List<UniversityDTO>();
            }
        }

        public async Task ExtractDataForCountriesAsync()
        {
            using var semaphore = new SemaphoreSlim(_threadsCount);
    
            await Parallel.ForEachAsync(_countries, async (country, _) =>
            {
                if (await _universityRepository.DoesCountryAlreadyExist(country))
                    return;
                        
                await semaphore.WaitAsync();
                try
                {
                    var content = await ExtractDataForCountryAsync(country);
                    var universities = await DeserializeCountryUniversitiesAsync(content);
                    await SaveUniversitiesToDbAsync(universities);
                }
                finally
                {
                    semaphore.Release();
                }
            });
        }


        public async Task<string> ExtractDataForCountryAsync(string country)
        {
            using var httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10)
            };

            var url = $"http://universities.hipolabs.com/search?country={Uri.EscapeDataString(country)}";

            try
            {
                var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();
        
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Ошибка при запросе {url}: {ex.Message}");
                return string.Empty;
            }
        }
        
        private async Task SaveUniversitiesToDbAsync(List<UniversityDTO> universityDTOs)
        {
            var universities = universityDTOs.Select(dto => new University
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Country = dto.Country,
                WebPages = dto.WebPages.Select(url => new WebPage
                {
                    Id = Guid.NewGuid(),
                    Url = url,
                }).ToList()
            }).ToList();
            
            await _universityRepository.AddUniversitiesAsync(universities);
        }
        
        public async Task<List<UniversityDTO>> GetUniversitiesAsync(string? country, string? name)
        {
            var universities = await _universityRepository.GetUniversitiesAsync(country, name);
    
            return universities.Select(u => new UniversityDTO
            {
                Name = u.Name,
                Country = u.Country,
                WebPages = u.WebPages.Select(wp => wp.Url).ToList()
            }).ToList();
        }
    }
}