using LearningPython.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace LearningPython.Web.ViewModels.AccountVM
{
    public class ProfileViewModel
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Image { get; set; }
        public IFormFile ImageFile { get; set; }
        public List<TestHistorySummaryViewModel> TestHistorySummaries { get; set; }
    }
}
