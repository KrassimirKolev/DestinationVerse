using DestinationVerse.Data.Entities;

namespace DestinationVerse.Services.Contracts
{
    public interface IDestinationService
    {
        // 1. Метод, който ще ни върне списък с всички активни (неизтрити) дестинации
        Task<IEnumerable<Destination>> GetAllAvailableDestinationsAsync();

        // 2. Метод за създаване и запис на нова дестинация в базата данни
        Task AddDestinationAsync(Destination destination);

        // 3. Извличане на дестинация по нейния уникален идентификатор за Details и Edit
        Task<Destination?> GetDestinationByIdAsync(int id);

        // 4. Обновяване на променените данни на съществуваща дестинация
        Task UpdateDestinationAsync(Destination destination);

        // 5. Сигурно изтриване (Soft Delete) чрез промяна на флага IsDeleted
        Task DeleteDestinationAsync(int id);

    }
}
