using LearningPython.DAL.Models;
using System.Collections.Generic;

namespace LearningPython.Web.ViewModels.TestQuestionVM
{
    public class SubmitAnswerVM
    {
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public double SuccessPercentage { get; set; }
        public List<AnswerResultVM> Answers { get; set; }
    }

    public class AnswerResultVM
    {
        public string QuestionText { get; set; }
        public string UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
    }
}