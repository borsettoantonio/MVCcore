using System.Data;
using MyCourse.Models.Enums;
using MyCourse.Models.ValueObjects;
using pgm3.Models.ViewModels.Lessons;
using pgm3.Models.ViewModels.Courses;

namespace pgm3.Models.ViewModels.Courses
{
    public class CourseDetailModel : CourseViewModel
    {
        public string? Description { get; set; }
        public List<LessonViewModel>? Lessons { get; set; }
        public TimeSpan TotalCourseDuration
        {
            get => TimeSpan.FromSeconds(Lessons?.Sum(l => l.Duration.TotalSeconds) ?? 0);
        }

        /*
        public static new CourseDetailModel FromDataRow(DataRow courseRow)
        {
            var courseDetailViewModel = new CourseDetailModel
            {
                Title = Convert.ToString(courseRow["Title"]),
                Description = Convert.ToString(courseRow["Description"]),
                ImagePath = Convert.ToString(courseRow["ImagePath"]),
                Author = Convert.ToString(courseRow["Author"]),
                Rating = Convert.ToDouble(courseRow["Rating"]),
                FullPrice = new Money(
                    Enum.Parse<Currency>(Convert.ToString(courseRow["FullPrice_Currency"])),
                    Convert.ToDecimal(courseRow["FullPrice_Amount"])
                ),
                CurrentPrice = new Money(
                    Enum.Parse<Currency>(Convert.ToString(courseRow["CurrentPrice_Currency"])),
                    Convert.ToDecimal(courseRow["CurrentPrice_Amount"])
                ),
                Id = Convert.ToInt32(courseRow["Id"]),
                Lessons = new List<LessonViewModel>(),
                Status = Convert.ToString(courseRow["Status"]!),
            };
            return courseDetailViewModel;
        }
        */
    }
}