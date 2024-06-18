using LearningPython.BLL.Data.Enums;
using LearningPython.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPython.Web.ViewModels.TestQuestionVM
{
    public class CreateViewModel
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public IFormFile? Image { get; set; }
        public string? URL { get; set; }
        public TestCategory Category { get; set; } = TestCategory.Quiz;
        public string? CorrectAnswer { get; set; }
        public List<string>? Options { get; set; }
        public int LessonId { get; set; }
    }
}
