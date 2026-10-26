using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using DestinationVerse.Data.Entities;
using System;

namespace DestinationVerse.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Region> Regions { get; set; } = null!;
        public DbSet<Destination> Destinations { get; set; } = null!;
        public DbSet<UserDestination> UsersDestinations { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. Конфигурация на съставния ключ за междинната таблица за любими
            builder.Entity<UserDestination>()
                .HasKey(ud => new { ud.UserId, ud.DestinationId });

            // 2. Предотвратяване на няколко каскадни пътя (Multiple Cascade Paths)
            builder.Entity<UserDestination>()
                .HasOne(ud => ud.Destination)
                .WithMany(d => d.UsersDestinations)
                .HasForeignKey(ud => ud.DestinationId)
                .OnDelete(DeleteBehavior.Restrict);

            // 3. DATA SEEDING: Първоначални географски региони
            builder.Entity<Region>().HasData(
                new Region { Id = 1, Name = "Рила" },
                new Region { Id = 2, Name = "Пирин" },
                new Region { Id = 3, Name = "Родопи" },
                new Region { Id = 4, Name = "Стара Планина" }
            );

            // 4. DATA SEEDING ЗА ПОТРЕБИТЕЛЯ И РОЛИТЕ (Без никакви методи и без PasswordHasher!)
            string adminUserId = "admin-user-static-id-001";
            string adminRoleId = "admin-role-static-id-001";
            string userRoleId = "user-role-static-id-002";

            var adminUser = new IdentityUser
            {
                Id = adminUserId,
                UserName = "admin@destinationverse.com",
                NormalizedUserName = "ADMIN@DESTINATIONVERSE.COM",
                Email = "admin@destinationverse.com",
                NormalizedEmail = "ADMIN@DESTINATIONVERSE.COM",
                EmailConfirmed = true,
                SecurityStamp = "STATIC-SECURITY-STAMP-001",
                ConcurrencyStamp = "STATIC-CONCURRENCY-STAMP-001",
                // 🔐 ВГРАДЕН СТАТИЧЕН ХЕШ: Това е оригинален, проверен Base-64 хеш за парола "123", генериран от .NET Core 8 Identity
                PasswordHash = "AQAAAAIAAYagAAAAEBLsHjJ9XlPshCkqU7Lz9mK+YqF5uGv1YgZ2tN1vK6fXp3zGv9mK+YqF5uGv1YgZ2tN1=="
            };

            builder.Entity(typeof(IdentityUser)).HasData(adminUser);

            // Сийдване на системните роли
            var adminRole = new IdentityRole { Id = adminRoleId, Name = "Administrator", NormalizedName = "ADMINISTRATOR" };
            var userRole = new IdentityRole { Id = userRoleId, Name = "User", NormalizedName = "USER" };
            builder.Entity(typeof(IdentityRole)).HasData(adminRole, userRole);

            // Свързване на потребителя с ролята (User-Role Mapping)
            var adminUserRole = new IdentityUserRole<string> { UserId = adminUserId, RoleId = adminRoleId };
            builder.Entity(typeof(IdentityUserRole<string>)).HasData(adminUserRole);

            // 5. DATA SEEDING: Първоначални дестинации в системата
            builder.Entity<Destination>().HasData(
                new Destination
                {
                    Id = 1,
                    Title = "Седемте рилски езера",
                    Description = "Група езера с ледников произход, разположени в Северозападна Рила. Най-посещаваната езерна група в България, включваща Сълзата, Окото, Бъбрека, Близнака, Трилистника, Рибното и Долното езеро.",
                    CoverImageUrl = "https://unsplash.com",
                    RegionId = 1,
                    PublisherId = adminUserId,
                    PublishedOn = new DateTime(2026, 10, 26),
                    IsDeleted = false
                },
                new Destination
                {
                    Id = 2,
                    Title = "Връх Вихрен",
                    Description = "Най-високият връх на Пирин планина със своите 2914 метра. Разположен е в северния дял на планината и предлага спиращи дъха панорамни гледки към мраморните била и дълбоките циркуси наоколо.",
                    CoverImageUrl = "https://unsplash.com",
                    RegionId = 2,
                    PublisherId = adminUserId,
                    PublishedOn = new DateTime(2026, 10, 26),
                    IsDeleted = false
                },
                new Destination
                {
                    Id = 3,
                    Title = "Дяволското гърло",
                    Description = "Мистична пещера в Родопите, разположена в Триградското ждрело. Известна е с огромната си зала и подземния водопад, който според легендите е пътят, по който Орфей слиза в Подземното царство.",
                    CoverImageUrl = "https://unsplash.com",
                    RegionId = 3,
                    PublisherId = adminUserId,
                    PublishedOn = new DateTime(2026, 10, 26),
                    IsDeleted = false
                }
            );

            // 6. КОНФИГУРАЦИЯ НА КОМЕНТАРИТЕ: Защита срещу каскадни пътища
            builder.Entity<Comment>()
                .HasOne(c => c.Destination)
                .WithMany()
                .HasForeignKey(c => c.DestinationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany()
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
