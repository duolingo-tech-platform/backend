using Microsoft.EntityFrameworkCore;
using DuolingoTechPlatform.Models;

namespace DuolingoTechPlatform.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseOption> ExerciseOptions { get; set; }
        public DbSet<UserProgress> UserProgress { get; set; }
        public DbSet<UserAnswer> UserAnswers { get; set; }
    }
}