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

        // 6. Извличане на всички любими дестинации за конкретен потребител
        Task<IEnumerable<Destination>> GetUserFavoritesAsync(string userId);

        // 7. Добавяне на дестинация в любими (Моят Пътеводител)
        Task AddToFavoritesAsync(string userId, int destinationId);

        // 8. Премахване на дестинация от любими
        Task RemoveFromFavoritesAsync(string userId, int destinationId);


    }
}
