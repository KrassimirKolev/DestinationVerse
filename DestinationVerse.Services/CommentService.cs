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
        public async Task<double> GetAverageRatingAsync(int destinationId)
        {
            var hasComments = await dbContext.Comments.AnyAsync(c => c.DestinationId == destinationId);
            if (!hasComments)
            {
                return 0.0; // Ако няма коментари, рейтингът е чиста нула
            }

            return await dbContext.Comments
                .Where(c => c.DestinationId == destinationId)
                .AverageAsync(c => c.Rating);
        }
    }
}
