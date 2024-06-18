using LearningPython.DAL.Models;

namespace LearningPython.Web.ViewModels.HomeVM
{
    public class HomeViewModel
    {
        public List<Lesson> Lessons { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public int TotalLessons { get; set; }
        public int Category { get; set; }
        public bool HasPreviousPage => Page > 1;

        public bool HasNextPage => Page < TotalPages;
    }
}
