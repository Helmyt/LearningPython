using LearningPython.DAL.Models;

namespace LearningPython.Web.ViewModels.LessonVM
{
    public class IndexViewModel
    {
        public IEnumerable<Lesson> Lessons { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalLessons { get; set; }
        public int Tag { get; set; }
        public bool HasPreviousPage => Page > 1;
        public string SearchTerm { get; set; }
        public bool HasNextPage => Page < TotalPages;
    }
}
