using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MyCourse.Models.InputModels;
using MyCourse.Models.Options;
using MyCourse.Models.ViewModels;
using pgm3.Models.ViewModels.Courses;
using MyCourse.Models.InputModels.Courses;

namespace pgm3.Models.Services.Application.Courses
{
    public class MemoryCacheCourseService : ICachedCourseService
    {
        private readonly ICourseService courseService;
        private readonly IMemoryCache memoryCache;
        IOptions<CoursesOptions> options;
        public MemoryCacheCourseService(ICourseService courseService, IMemoryCache memoryCache, IOptions<CoursesOptions> options)
        {
            this.courseService = courseService;
            this.memoryCache = memoryCache;
            this.options = options;
        }
        public Task<CourseDetailModel> GetCourseAsync(int id)
        {
            return memoryCache.GetOrCreateAsync($"Course{id}", cacheEntry =>
            {
                cacheEntry.SetSize(1);
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(options.Value.CacheDuration));
                return courseService.GetCourseAsync(id);
            })!;
        }

        public Task<ListViewModel<CourseViewModel>> GetCoursesAsync(CourseListInputModel model)
        {
            return memoryCache.GetOrCreateAsync($"Course{model.Search}-{model.Page}-{model.OrderBy}-{model.Ascending}", cacheEntry =>
             {
                 cacheEntry.SetSize(1);
                 cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(options.Value.CacheDuration));
                 return courseService.GetCoursesAsync(model);
             })!;
        }
        public Task<List<CourseViewModel>> GetBestRatingCoursesAsync()
        {
            return memoryCache.GetOrCreateAsync($"BestRatingCourses", cacheEntry =>
            {
                cacheEntry.SetSize(1);
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(options.Value.CacheDuration));
                return courseService.GetBestRatingCoursesAsync();
            })!;
        }
        public Task<List<CourseViewModel>> GetMostRecentCoursesAsync()
        {
            return memoryCache.GetOrCreateAsync($"MostRecentCourses", cacheEntry =>
            {
                cacheEntry.SetSize(1);
                cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(options.Value.CacheDuration));
                return courseService.GetMostRecentCoursesAsync();
            })!;
        }

        public Task<CourseDetailModel> CreateCourseAsysnc(CourseCreateInputModel inputModel)
        {
            return courseService.CreateCourseAsysnc(inputModel);
        }
        public Task<bool> IsTitleAvailableAsync(string title, int id)
        {
            return courseService.IsTitleAvailableAsync(title, id);
        }

        public Task<CourseEditInputModel> GetCourseForEditingAsync(int id)
        {
            return courseService.GetCourseForEditingAsync(id);
        }

        public async Task<CourseDetailModel> EditCourseAsync(CourseEditInputModel inputModel)
        {
            CourseDetailModel viewModel = await courseService.EditCourseAsync(inputModel);
            memoryCache.Remove($"Course{inputModel.Id}");
            return viewModel;
        }

        public Task SendQuestionToCourseAuthorAsync(int courseId, string question)
        {
            return courseService.SendQuestionToCourseAuthorAsync(courseId, question);
        }

        public async Task DeleteCourseAsync(CourseDeleteInputModel inputModel)
        {
            await courseService.DeleteCourseAsync(inputModel);
            memoryCache.Remove($"Course{inputModel.Id}");
        }
    }
}