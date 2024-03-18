using JWTAuthenticationWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthenticationWebAPI.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<LoginModel>? LoginModels { get; set; }

        public DbSet<LoginHistory>? LoginHistory { get; set; }

        public DbSet<LogEntry>? LogEntry { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginModel>().HasData(new LoginModel
            {
                Id = 1,
                UserName = "admin",
                Password = "admin@123"
            });
        }
    }
}
