using Microsoft.EntityFrameworkCore;
using DuolingoTechPlatform.Models;

namespace DuolingoTechPlatform.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        // Adicione outros DbSets conforme necessário
    }
}