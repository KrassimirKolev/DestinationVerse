using DestinationVerse.Data.Entities;

namespace DestinationVerse.Services.Contracts
{
    public interface IRegionService
    {
        // Метод, който ще ни върне списък с всички налични региони в базата данни
        Task<IEnumerable<Region>> GetAllRegionsAsync();
    }

    namespace DestinationVerse.Services.Contracts
    {
        public interface IRegionService
        {
            Task<System.Collections.Generic.IEnumerable<Data.Entities.Region>> GetAllRegionsAsync();

            // Бизнес договор за създаване на регион
            Task<bool> CreateRegionAsync(string name);
        }
    }

}
