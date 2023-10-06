using MyCourse.Models.InputModels;
using MyCourse.Models.ViewModels;
using pgm3.Models.ViewModels.Courses;
using MyCourse.Models.InputModels.Courses;

namespace pgm3.Models.Services.Application.Courses
{
    public interface ICourseService
    {
        public Task<ListViewModel<CourseViewModel>> GetCoursesAsync(CourseListInputModel model);
        public Task<CourseDetailModel> GetCourseAsync(int id);
        Task<List<CourseViewModel>> GetBestRatingCoursesAsync();
        Task<List<CourseViewModel>> GetMostRecentCoursesAsync();
        Task<CourseDetailModel> CreateCourseAsysnc(CourseCreateInputModel inputModel);
        Task<CourseEditInputModel> GetCourseForEditingAsync(int id);
        Task<CourseDetailModel> EditCourseAsync(CourseEditInputModel inputModel);
        Task<bool> IsTitleAvailableAsync(string title, int excludeId);
        Task SendQuestionToCourseAuthorAsync(int id, string question);
         Task DeleteCourseAsync(CourseDeleteInputModel inputModel);
    }
}