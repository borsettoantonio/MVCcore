using MyCourse.Models.InputModels;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
public interface ICourseService
{
    public Task<List<CourseViewModel>> GetCoursesAsync(CourseListInputModel model);
    public Task<CourseDetailModel> GetCourseAsync(int id);
}
}