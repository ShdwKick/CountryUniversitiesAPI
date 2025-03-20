using CountryUniversities.Database;
using CountryUniversities.DataModels;
using Microsoft.EntityFrameworkCore;

namespace CountryUniversities.Repositories
{
    public class UniversityRepository : IUniversityRepository
    {
        private readonly IDbContextFactory<DatabaseConnection> _contextFactory;

        public UniversityRepository(IDbContextFactory<DatabaseConnection> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddUniversitiesAsync(List<University> universities)
        {
            await using var context = _contextFactory.CreateDbContext();
            
            // Начинаем транзакцию
            await using var transaction = await context.Database.BeginTransactionAsync();

            // Загружаем существующие сайты в словарь
            var existingWebPages = await context.WebPages
                .ToDictionaryAsync(wp => wp.Url, wp => wp);

            foreach (var university in universities)
            {
                var updatedWebPages = new List<WebPage>();

                foreach (var webPage in university.WebPages)
                {
                    if (existingWebPages.TryGetValue(webPage.Url, out var existingWebPage))
                    {
                        updatedWebPages.Add(existingWebPage);
                    }
                    else
                    {
                        // Добавляем новый сайт в БД и в словарь
                        existingWebPages[webPage.Url] = webPage;
                        updatedWebPages.Add(webPage);
                        await context.WebPages.AddAsync(webPage);
                    }
                }

                university.WebPages = updatedWebPages;
                await context.Universities.AddAsync(university);
            }

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }


        public async Task<List<University>> GetUniversitiesAsync(string? country, string? name)
        {
            await using var context = _contextFactory.CreateDbContext();
            IQueryable<University> query = context.Universities.Include(u => u.WebPages);

            // Фильтрация
            if (!string.IsNullOrEmpty(country))
                query = query.Where(u => u.Country == country);

            if (!string.IsNullOrEmpty(name))
                query = query.Where(u => u.Name.Contains(name));

            return await query.ToListAsync();
        }

        public async Task<bool> DoesCountryAlreadyExist(string country)
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Universities.AnyAsync(u => u.Country == country);
        }

    }
}
