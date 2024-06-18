using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPython.DAL.Models
{
    public class User : IdentityUser
    {
        public string? Image { get; set; }
        public List<TestUserAnswer>? TestUserAnswers { get; set; }
        public List<Lesson>? Lessons { get; set; }
    }
}
