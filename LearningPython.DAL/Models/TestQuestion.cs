using LearningPython.BLL.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPython.DAL.Models
{
    public class TestQuestion
    {
        [Key]
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? Image { get; set; }
        public TestCategory Category { get; set; } = TestCategory.Quiz;
        public string? Answer { get; set; }
        public List<string>? Options { get; set; }
        [ForeignKey("Lesson")]
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
