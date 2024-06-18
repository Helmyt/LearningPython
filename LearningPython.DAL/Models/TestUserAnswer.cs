using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPython.DAL.Models
{
    public class TestUserAnswer
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User? User { get; set; }
        [ForeignKey("Lesson")]
        public int LessonId { get; set; }
        public Lesson? Lesson { get; set; }
        [ForeignKey("TestQuestion")]
        public int TestQuestionId { get; set; }
        public TestQuestion TestQuestion { get; set; }
        public bool? IfAnswerWasRight { get; set; }
        public string UserAnswer { get; set; }
    }
}
