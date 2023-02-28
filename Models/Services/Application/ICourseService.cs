using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
public interface ICourseService
{
    public Task<List<CourseViewModel>> GetCoursesAsync(string search, int page,string orderby,bool ascending);
    public Task<CourseDetailModel> GetCourseAsync(int id);
}
}