using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DestinationVerse.Data;
using DestinationVerse.Data.Entities;
using DestinationVerse.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DestinationVerse.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext dbContext;

        public CommentService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByDestinationIdAsync(int destinationId)
        {
            return await dbContext.Comments
                .Where(c => c.DestinationId == destinationId)
                .Include(c => c.Author) // Зареждаме автора, за да изпишем името му в коментара
                .OrderByDescending(c => c.CreatedOn) // Най-новите коментари излизат най-отгоре
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddCommentAsync(Comment comment)
        {
            comment.CreatedOn = DateTime.Now;
            await dbContext.Comments.AddAsync(comment);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Comment?> GetCommentByIdAsync(int commentId)
        {
            return await dbContext.Comments.FindAsync(commentId);
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = await dbContext.Comments.FindAsync(commentId);
            if (comment != null)
            {
                dbContext.Comments.Remove(comment);
                await dbContext.SaveChangesAsync();
            }
        }

        // Изчисляване на среден рейтинг: Агрегираща LINQ функция
        // 📐 Рефакториран среден рейтинг: Взима само последната оценка на всеки уникален потребител
        public async Task<double> GetAverageRatingAsync(int destinationId)
        {
            // 1. Проверяваме дали изобщо има коментари за тази дестинация
            var hasComments = await dbContext.Comments.AnyAsync(c => c.DestinationId == destinationId);
            if (!hasComments)
            {
                return 0.0;
            }

            // 2. Издърпваме коментарите за дестинацията в паметта, за да извършим напредналото групиране
            var destinationComments = await dbContext.Comments
                .Where(c => c.DestinationId == destinationId)
                .AsNoTracking()
                .ToListAsync();

            // 3. Сложен LINQ филтър:
            // - Групираме по AuthorId (всеки потребител става отделна група)
            // - От всяка група избираме най-новия запис (OrderByDescending по CreatedOn)
            // - Извличаме само оценката (Rating) на този най-нов запис
            var latestUserRatings = destinationComments
                .GroupBy(c => c.AuthorId)
                .Select(g => g.OrderByDescending(c => c.CreatedOn).First().Rating)
                .ToList();

            if (!latestUserRatings.Any())
            {
                return 0.0;
            }

            // 4. Връщаме средното аритметично от филтрираните уникални гласове
            return latestUserRatings.Average();
        }

    }
}
