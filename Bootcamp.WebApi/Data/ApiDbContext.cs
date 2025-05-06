// // Created On: 2025.05.06
// // Create by: althunibat

using Bootcamp.WebApi.Config;
using Bootcamp.WebApi.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Bootcamp.WebApi.Data;

public class ApiDbContext(DbContextOptions<ApiDbContext> options, IConfiguration configuration) : DbContext(options) {
    protected override void OnModelCreating(ModelBuilder builder) {
        builder.HasDefaultSchema(configuration.GetDbSchema());
        builder.ApplyConfiguration(new UserEntityConfig());
        builder.Entity<User>().HasData(
            new User {
                Id = 1, FirstName = "Rami", LastName = "Yousef", Email = "rami.yousef@example.com", RowVersion = 1
            },
            new User {
                Id = 2, FirstName = "Lina", LastName = "Hassan", Email = "lina.hassan@example.com", RowVersion = 1
            },
            new User { Id = 3, FirstName = "Sami", LastName = "Odeh", Email = "sami.odeh@example.com", RowVersion = 1 },
            new User {
                Id = 4, FirstName = "Nour", LastName = "Khalil", Email = "nour.khalil@example.com", RowVersion = 1
            },
            new User {
                Id = 5, FirstName = "Alaa", LastName = "Ahmad", Email = "alaa.ahmad@example.com", RowVersion = 1
            },
            new User {
                Id = 6, FirstName = "Faris", LastName = "Naser", Email = "faris.naser@example.com", RowVersion = 1
            },
            new User {
                Id = 7, FirstName = "Dana", LastName = "Salem", Email = "dana.salem@example.com", RowVersion = 1
            },
            new User {
                Id = 8, FirstName = "Khaled", LastName = "Mansour", Email = "khaled.mansour@example.com", RowVersion = 1
            },
            new User {
                Id = 9, FirstName = "Tariq", LastName = "Zayed", Email = "tariq.zayed@example.com", RowVersion = 1
            },
            new User {
                Id = 10, FirstName = "Reem", LastName = "Haddad", Email = "reem.haddad@example.com", RowVersion = 1
            }
        );
    }

    public DbSet<User> Users { get; set; }
}