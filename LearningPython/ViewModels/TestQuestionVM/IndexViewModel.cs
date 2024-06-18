using LearningPython.DAL.Models;

namespace LearningPython.Web.ViewModels.TestQuestionVM
{
    public class IndexViewModel
    {
        public string Title { get; set; }
        public int LessonId { get; set; }
        public List<TestQuestion> TestQuestions { get; set; }
        public List<TestUserAnswer> Answers { get; set; } = new List<TestUserAnswer>();
    }
    public class UserAnswerVM
    {
        public int TestQuestionId { get; set; }
        public string UserAnswer { get; set; }
    }
}
