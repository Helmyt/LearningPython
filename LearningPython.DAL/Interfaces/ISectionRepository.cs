using LearningPython.DAL.Models;

namespace LearningPython.DAL.Interfaces
{
    public interface ISectionRepository
    {
        bool Add(Section section);
        Task<Section?> GetByIdAsync(int id);
        Task<List<Section>> GetAllSectionsAsync(int id);
        Task<Section?> GetByIdAsyncNoTracking(int id);
        bool Update(Section section);

        bool Delete(Section section);

        bool Save();
    }
}
