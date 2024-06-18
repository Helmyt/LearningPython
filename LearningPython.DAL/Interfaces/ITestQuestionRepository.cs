using LearningPython.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningPython.DAL.Interfaces
{
    public interface ITestQuestionRepository
    {
        bool Add(TestQuestion testQuestion);
        Task<TestQuestion?> GetByIdAsync(int id);
        Task<List<TestQuestion>> GetAllTestQuestionsAsync(int id);
        Task<TestQuestion?> GetByIdAsyncNoTracking(int id);
        bool Update(TestQuestion testQuestion);

        bool Delete(TestQuestion testQuestion);

        bool Save();
    }
}
