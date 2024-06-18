namespace LearningPython.Web.ViewModels.LessonSectionViewVM
{
    public class CreateViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public IFormFile? Image { get; set; }
        public string? Code { get; set; }
        public IFormFile? JsFile { get; set; }
        public int? JsWidth { get; set; }
        public int? JsHeight { get; set; }
        public int LessonId { get; set; }
    }
}
