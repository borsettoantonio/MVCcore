using MyCourse.Models.InputModels;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
{
    public interface ICourseService
    {
        public Task<ListViewModel<CourseViewModel>> GetCoursesAsync(CourseListInputModel model);
        public Task<CourseDetailModel> GetCourseAsync(int id);
        Task<List<CourseViewModel>> GetBestRatingCoursesAsync();
        Task<List<CourseViewModel>> GetMostRecentCoursesAsync();
        Task<CourseDetailModel> CreateCourseAsysnc(CourseCreateInputModel inputModel);
    }
}