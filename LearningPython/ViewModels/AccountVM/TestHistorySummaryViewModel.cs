using LearningPython.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace LearningPython.Web.ViewModels.AccountVM
{
    public class TestHistorySummaryViewModel
    {
        public int LessonId { get; set; }
        public string LessonTitle { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public double Percentage { get; set; }
    }
}
