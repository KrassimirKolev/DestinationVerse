using DestinationVerse.Data;
using DestinationVerse.Data.Entities;
using DestinationVerse.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DestinationVerse.Services
{
    public class RegionService : IRegionService
    {
        private readonly ApplicationDbContext dbContext;

        // Вграденият IoC контейнер автоматично инжектира контекста на базата данни тук
        public RegionService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Region>> GetAllRegionsAsync()
        {
            return await dbContext.Regions
                .AsNoTracking() // Оптимизация за памет (Read-only заявка)
                .ToListAsync();
        }
    }
}
