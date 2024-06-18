using LearningPython.BLL.Data.Enums;
using LearningPython.DAL.Models;

namespace LearningPython.DAL.Interfaces
{
    public interface ILessonRepository
    {
        Task<IEnumerable<Lesson>> GetAll();

        Task<IEnumerable<Lesson>> GetSliceAsync(int offset, int size);

        Task<IEnumerable<Lesson>> GetLessonsByTagAndSliceAsync(LessonTag tag, int offset, int size);

        Task<Lesson?> GetByIdAsync(int id);

        Task<Lesson?> GetByIdAsyncNoTracking(int id);

        Task<int> GetCountAsync();

        Task<int> GetCountByTagAsync(LessonTag tag);
        Task<IEnumerable<Lesson>> SearchLessonsAsync(string searchTerm, int offset, int size);
        Task<int> GetCountBySearchAsync(string searchTerm);

        bool Add(Lesson lesson);

        bool Update(Lesson lesson);

        bool Delete(Lesson lesson);

        bool Save();
    }
}
