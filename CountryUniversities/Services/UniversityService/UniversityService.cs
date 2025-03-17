using CountryUniversities.Repositories;

namespace CountryUniversities.Services
{
    public class UniversityService : IUniversityService
    {
        private static SemaphoreSlim _semaphore;
        private readonly IUniversityRepository _universityRepository;

        public UniversityService(IUniversityRepository universityRepository)
        {
            //TODO: брать из конфиг файла
            _semaphore = new SemaphoreSlim(2);
            _universityRepository = universityRepository;
        }

        public Task DeserializeUniversities()
        {
            return Task.CompletedTask;
        }
    }
}
