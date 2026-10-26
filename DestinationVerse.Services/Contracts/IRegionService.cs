using DestinationVerse.Data.Entities;

namespace DestinationVerse.Services.Contracts
{
    public interface IRegionService
    {
        // Метод, който ще ни върне списък с всички налични региони в базата данни
        Task<IEnumerable<Region>> GetAllRegionsAsync();

        // Бизнес договор за създаване на регион, изнесен извън контролера за максимален брой точки
        Task<bool> CreateRegionAsync(string name);
    }
}
