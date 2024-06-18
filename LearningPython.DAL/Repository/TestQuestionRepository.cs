using LearningPython.BLL.Data.Enums;
using LearningPython.DAL.Context;
using LearningPython.DAL.Interfaces;
using LearningPython.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace LearningPython.DAL.Repository
{
    public class TestQuestionRepository : ITestQuestionRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TestQuestionRepository(AppDbContext context)
        {
            _context = context;
        }
        public bool Add(TestQuestion testQuestion)
        {
            _context.Add(testQuestion);
            return Save();
        }
        public bool Delete(TestQuestion testQuestion)
        {
            _context.Remove(testQuestion);
            return Save();
        }
        public async Task<TestQuestion?> GetByIdAsync(int id)
        {
            return await _context.TestQuestions.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<List<TestQuestion>?> GetAllTestQuestionsAsync(int id)
        {
            var testQuestions = _context.TestQuestions.Where(r => r.Lesson.Id == id);
            return testQuestions.ToList();
        }
        public async Task<TestQuestion?> GetByIdAsyncNoTracking(int id)
        {
            return await _context.TestQuestions.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
        public bool Update(TestQuestion testQuestion)
        {
            _context.Update(testQuestion);
            return Save();
        }
    }
}
