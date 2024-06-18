using LearningPython.BLL.Data.Enums;
using System.Net;

namespace LearningPython.Web.ViewModels.LessonVM
{
    public class EditViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? LessonImage { get; set; }
        public string? URL { get; set; }
        public LessonTag LessonTag { get; set; }
    }
}
