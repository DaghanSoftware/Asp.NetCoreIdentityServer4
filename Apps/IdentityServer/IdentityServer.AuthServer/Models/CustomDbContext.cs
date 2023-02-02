using Microsoft.EntityFrameworkCore;

namespace IdentityServer.AuthServer.Models
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options)
        {

        }
        public DbSet<CustomUser> CustomUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomUser>().HasData(
                new CustomUser() { Id = 1, Email = "Elbatra@outlook.com",Password="password",City="İstanbul", UserName="Elbatra" },
                new CustomUser() { Id = 2, Email = "EmoBaskan@outlook.com", Password = "password", City = "Konya", UserName = "EmoBaskan" },
                new CustomUser() { Id = 3, Email = "Avenom@outlook.com", Password = "password", City = "Eskişehir", UserName = "Avenom" }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
