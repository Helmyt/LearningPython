using LearningPython.BLL.Data.Enums;
using LearningPython.DAL.Context;
using LearningPython.DAL.Interfaces;
using LearningPython.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LearningPython.DAL.Repository
{
    public class LessonRepository : ILessonRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LessonRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool Add(Lesson lesson)
        {
            _context.Add(lesson);
            return Save();
        }

        public bool Delete(Lesson lesson)
        {
            _context.Remove(lesson);
            return Save();
        }

        public async Task<IEnumerable<Lesson>> GetAll()
        {
            return await _context.Lessons.ToListAsync();
        }

        public async Task<IEnumerable<Lesson>> GetSliceAsync(int offset, int size)
        {
            return await _context.Lessons.Skip(offset).Take(size).ToListAsync();
        }

        public async Task<IEnumerable<Lesson>> GetLessonsByTagAndSliceAsync(LessonTag tag, int offset, int size)
        {
            return await _context.Lessons
                .Where(t => t.LessonTag == tag)
                .Skip(offset)
                .Take(size)
                .ToListAsync();
            //c => c.LessonTag.Any(x => category.Any(y => y == x))
        }

        public async Task<int> GetCountByTagAsync(LessonTag tag)
        {
            return await _context.Lessons.CountAsync(t => t.LessonTag == tag);
        }

        public async Task<Lesson?> GetByIdAsync(int id)
        {
            return await _context.Lessons.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Lesson?> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Lessons.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<IEnumerable<Lesson>> SearchLessonsAsync(string searchTerm, int offset, int size)
        {
            return await _context.Lessons
                                 .Where(l => l.Title.Contains(searchTerm))
                                 .Skip(offset)
                                 .Take(size)
                                 .ToListAsync();
        }

        public async Task<int> GetCountBySearchAsync(string searchTerm)
        {
            return await _context.Lessons.CountAsync(l => l.Title.Contains(searchTerm));
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool Update(Lesson lesson)
        {
            _context.Update(lesson);
            return Save();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Lessons.CountAsync();
        }
    }
}
