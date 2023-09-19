using System.Collections.Generic;
using pgm3.Models.ViewModels.Courses;

namespace pgm3.Models.ViewModels.Home
{
    public class HomeViewModel : CourseViewModel
    {
        public List<CourseViewModel> MostRecentCourses { get; set; }
        public List<CourseViewModel> BestRatingCourses { get; set; }
    }
}