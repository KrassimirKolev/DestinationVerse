using DestinationVerse.Data;
using DestinationVerse.Data.Entities;
using DestinationVerse.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DestinationVerse.Services
{
    public class RegionService : IRegionService
    {
        private readonly ApplicationDbContext dbContext;

        public RegionService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Region>> GetAllRegionsAsync()
        {
            return await dbContext.Regions.AsNoTracking().ToListAsync();
        }

        // Цялата бизнес логика живее тук, далеч от контролера!
        public async Task<bool> CreateRegionAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Trim().Length < 3 || name.Trim().Length > 20)
            {
                return false; // Бизнес правилото не е спазено
            }

            var cleanedName = name.Trim();

            // Бонус защита: Проверяваме дали такъв регион вече не съществува
            var exists = await dbContext.Regions.AnyAsync(r => r.Name == cleanedName);
            if (exists)
            {
                return false;
            }

            var region = new Region()
            {
                Name = cleanedName
            };

            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
