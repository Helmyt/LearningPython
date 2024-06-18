using LearningPython.BLL.Data.Enums;

namespace LearningPython.Web.ViewModels.LessonVM
{
    public class CreateViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile LessonImage { get; set; }
        public string? URL { get; set; }
        public LessonTag LessonTag { get; set; }
        public string UserId { get; set; }
    }
}
