using Microsoft.EntityFrameworkCore;
using MyMusicServicesApi.Models;

namespace MyMusicServicesApi.Data
{
    public class AppDbContext : DbContext
    {
        // Construtor que recebe as opções configuradas no Program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet para sua entidade User
        public DbSet<User> Users { get; set; }
    }
}
