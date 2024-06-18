using LearningPython.DAL.Context;
using LearningPython.DAL.Interfaces;
using LearningPython.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LearningPython.DAL.Repository
{
    public class SectionRepository : ISectionRepository
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SectionRepository(AppDbContext context)
        {
            _context = context;
        }
        public bool Add(Section section)
        {
            _context.Add(section);
            return Save();
        }
        public bool Delete(Section section)
        {
            _context.Remove(section);
            return Save();
        }
        public async Task<Section?> GetByIdAsync(int id)
        {
            return await _context.Sections.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<List<Section>?> GetAllSectionsAsync(int id)
        {
            var lessonSections = _context.Sections.Where(r => r.Lesson.Id == id);
            return lessonSections.ToList();
        }
        public async Task<Section?> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Sections.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
        public bool Update(Section section)
        {
            _context.Update(section);
            return Save();
        }
    }
}
