using DestinationVerse.Data.Entities;

namespace DestinationVerse.Services.Contracts
{
    public interface IDestinationService
    {
        // 1. Метод, който ще ни върне списък с всички активни (неизтрити) дестинации
        Task<IEnumerable<Destination>> GetAllAvailableDestinationsAsync();

        // 2. Метод за създаване и запис на нова дестинация в базата данни
        Task AddDestinationAsync(Destination destination);
    }
}
