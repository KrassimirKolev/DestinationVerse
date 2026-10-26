using System.Collections.Generic;
using System.Threading.Tasks;
using DestinationVerse.Data.Entities;

namespace DestinationVerse.Services.Contracts
{
    public interface ICommentService
    {
        // 1. Извличане на всички одобрени коментари за конкретна дестинация
        Task<IEnumerable<Comment>> GetCommentsByDestinationIdAsync(int destinationId);

        // 2. Добавяне на нов коментар с оценка
        Task AddCommentAsync(Comment comment);

        // 3. Изтриване на коментар (достъпно за автора или за Администратор)
        Task DeleteCommentAsync(int commentId);

        // 4. Намиране на коментар по ID за проверка на собственост
        Task<Comment?> GetCommentByIdAsync(int commentId);

        // 5. Изчисляване на среден рейтинг (звезди) за дадена дестинация
        Task<double> GetAverageRatingAsync(int destinationId);
    }
}
