using LearningPython.DAL.Models;

namespace LearningPython.Web.ViewModels.LessonSectionViewVM
{
    public class IndexViewModel
    {
        public int LessonId { get; set; }
        public string Title { get; set; }
        public IEnumerable<Section> Sections { get; set; }
    }
}
