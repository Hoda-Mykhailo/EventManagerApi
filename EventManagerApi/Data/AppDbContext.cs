using EventManagerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagerApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Role> Roles => Set<Role>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Унікальні індекси для Username (як було)
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Конфігурація зв'язку User -> Role (many users -> one role)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed базових ролей
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "User" },
                new Role { Id = 2, Name = "Admin" }
            );
        }
    }
}
