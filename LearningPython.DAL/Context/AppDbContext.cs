using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LearningPython.DAL.Models;

namespace LearningPython.DAL.Context
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<TestQuestion> TestQuestions { get; set; }
        public DbSet<TestUserAnswer> TestUserAnswers { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<TestQuestion> TestResults { get; set; }
    }
}
