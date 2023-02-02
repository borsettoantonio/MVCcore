using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
public interface ICourseService
{
    public List<CourseViewModel> GetCourses();
    public CourseDetailModel GetCourse(int id);
}
}