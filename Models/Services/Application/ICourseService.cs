using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
public interface ICourseService
{
    public Task<List<CourseViewModel>> GetCoursesAsync();
    public Task<CourseDetailModel> GetCourseAsync(int id);
}
}