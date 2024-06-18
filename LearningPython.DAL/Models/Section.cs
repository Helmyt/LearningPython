using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPython.DAL.Models
{
    public class Section
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Image { get; set; }
        public string? Text { get; set; }
        public string? Code { get; set; }
        public string? JsFileUrl { get; set; }
        public int? JsCanvasWidth { get; set; }
        public int? JsCanvasHeight { get; set; }
        [ForeignKey("Lesson")]
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
