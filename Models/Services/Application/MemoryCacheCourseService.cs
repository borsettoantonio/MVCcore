using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MyCourse.Models.Options;
using MyCourse.Models.ViewModels;

namespace MyCourse.Models.Services.Application
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
            this.options=options;
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

        public Task<List<CourseViewModel>> GetCoursesAsync()
        {
            return memoryCache.GetOrCreateAsync("Course", cacheEntry =>
             {
                cacheEntry.SetSize(1);
                 cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(options.Value.CacheDuration));
                 return courseService.GetCoursesAsync();
             })!;
        }
    }
}