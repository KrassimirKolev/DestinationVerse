using DestinationVerse.Data;
using DestinationVerse.Data.Entities;
using DestinationVerse.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DestinationVerse.Services
{
    public class DestinationService : IDestinationService
    {
        private readonly ApplicationDbContext dbContext;

        public DestinationService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // Взимаме само дестинациите, които не са изтрити, заедно с техния Регион (Eager Loading)
        public async Task<IEnumerable<Destination>> GetAllAvailableDestinationsAsync()
        {
            return await dbContext.Destinations
                .Where(d => d.IsDeleted == false)
                .Include(d => d.Region) // Зарежда релацията с регионите за изгледа
                .AsNoTracking() // Оптимизация за бързина при четене
                .ToListAsync();
        }

        // Записваме новата дестинация в базата данни
        public async Task AddDestinationAsync(Destination destination)
        {
            await dbContext.Destinations.AddAsync(destination);
            await dbContext.SaveChangesAsync();
        }
    }
}
