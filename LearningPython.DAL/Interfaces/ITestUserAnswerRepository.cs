using LearningPython.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LearningPython.DAL.Interfaces
{
    public interface ITestUserAnswerRepository
    {
        Task<TestUserAnswer?> GetByIdAsync(int id);
        Task<IEnumerable<TestUserAnswer>> GetByLessonAndUserAsync(int lessonId, string userId);
        Task<IEnumerable<TestUserAnswer>> GetTestUserAnswers(string userId);
        Task AddAsync(TestUserAnswer testUserAnswer);
        Task AddRangeAsync(IEnumerable<TestUserAnswer> testUserAnswers);
        Task SaveChangesAsync();
        bool Delete(TestUserAnswer testUserAnswer);
        bool Save();
    }
}