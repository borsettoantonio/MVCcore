namespace MyCourse.Models.ViewModels
{
    public class CourseDetailModel : CourseViewModel
    {
        public string? Description { get; set; }
        public List<LessonViewModel>? Lessons { get; set; }
        public TimeSpan TotalCourseDuration
        {
            get => TimeSpan.FromSeconds(Lessons?.Sum(l => l.Duration.TotalSeconds) ?? 0);
        }
    }
} 