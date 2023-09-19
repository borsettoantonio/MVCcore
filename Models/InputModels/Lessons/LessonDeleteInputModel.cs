using System.ComponentModel.DataAnnotations;

namespace pgm3.Models.InputModels.Lessons
{
    public class LessonDeleteInputModel
    {
        [Required]
        public int Id { get; set; }
        public int CourseId { get; set; }
    }
}