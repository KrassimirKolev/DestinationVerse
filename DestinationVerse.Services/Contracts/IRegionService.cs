using DestinationVerse.Data.Entities;

namespace DestinationVerse.Services.Contracts
{
    public interface IRegionService
    {
        // Метод, който ще ни върне списък с всички налични региони в базата данни
        Task<IEnumerable<Region>> GetAllRegionsAsync();

        // Бизнес договор за създаване на регион, изнесен извън контролера за максимален брой точки
        Task<bool> CreateRegionAsync(string name);

        Task<Data.Entities.Region?> GetRegionByIdAsync(int id);
        Task<bool> UpdateRegionAsync(int id, string name);
        Task DeleteRegionAsync(int id);

    }
}
