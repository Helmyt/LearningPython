using LearningPython.DAL.Context;
using LearningPython.DAL.Interfaces;
using LearningPython.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearningPython.DAL.Repository
{
    public class TestUserAnswerRepository : ITestUserAnswerRepository
    {
        private readonly AppDbContext _context;

        public TestUserAnswerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TestUserAnswer?> GetByIdAsync(int id)
        {
            return await _context.TestUserAnswers
                .Include(t => t.TestQuestion)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TestUserAnswer>> GetByLessonAndUserAsync(int lessonId, string userId)
        {
            return await _context.TestUserAnswers
                .Include(t => t.TestQuestion)
                .Where(t => t.LessonId == lessonId && t.UserId == userId)
                .ToListAsync();
        }
        public async Task<IEnumerable<TestUserAnswer>> GetTestUserAnswers(string userId)
        {
            return await _context.TestUserAnswers
                .Include(t => t.TestQuestion)
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }
        public async Task AddAsync(TestUserAnswer testUserAnswer)
        {
            await _context.TestUserAnswers.AddAsync(testUserAnswer);
        }
        public async Task AddRangeAsync(IEnumerable<TestUserAnswer> testUserAnswers)
        {
            await _context.TestUserAnswers.AddRangeAsync(testUserAnswers);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
        public bool Delete(TestUserAnswer testUserAnswer)
        {
            _context.Remove(testUserAnswer);
            return Save();
        }
    }
}
