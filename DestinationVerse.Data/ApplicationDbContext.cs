using DestinationVerse.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DestinationVerse.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Регистрация на трите нови таблици в базата данни
        public DbSet<Region> Regions { get; set; } = null!;
        public DbSet<Destination> Destinations { get; set; } = null!;
        public DbSet<UserDestination> UsersDestinations { get; set; } = null!;

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    // Задължително извикваме базовия метод за Identity сигурността
        //    base.OnModelCreating(builder);

        //    // КОНФИГУРАЦИЯ НА СЪСТАВНИЯ КЛЮЧ ЗА СИГУРНОСТ (Composite Primary Key)
        //    builder.Entity<UserDestination>()
        //        .HasKey(ud => new { ud.UserId, ud.DestinationId });
        //}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. Конфигурация на съставния ключ
            builder.Entity<UserDestination>()
                .HasKey(ud => new { ud.UserId, ud.DestinationId });

            // 2. РЕШЕНИЕ НА ГРЕШКАТА: Предотвратяване на няколко каскадни пътя (Multiple Cascade Paths)
            builder.Entity<UserDestination>()
                .HasOne(ud => ud.Destination)
                .WithMany(d => d.UsersDestinations)
                .HasForeignKey(ud => ud.DestinationId)
                .OnDelete(DeleteBehavior.Restrict); // Изключва каскадното изтриване по този път
        }

    }
}
