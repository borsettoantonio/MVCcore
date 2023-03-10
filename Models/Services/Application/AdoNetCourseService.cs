using System.Data;
using MyCourse.Models.Services.Infrastructure;
using MyCourse.Models.ViewModels;
using System.Linq;
using MyCourse.Models.ValueObjects;
using MyCourse.Models.Enums;
using Microsoft.Extensions.Options;
using MyCourse.Models.Options;
using MyCourse.Models.Exceptions;
using MyCourse.Models.InputModels;

namespace MyCourse.Models.Services.Application
{
    public class AdoNetCourseService : ICourseService
    {
        private readonly ILogger<AdoNetCourseService> logger;
        private readonly IDatabaseAccessor db;
        private readonly IConfiguration coursesOptions;
        IOptions<CoursesOptions> options;

        public AdoNetCourseService(IDatabaseAccessor db, IConfiguration coursesOptions, IOptions<CoursesOptions> options,
                                    ILogger<AdoNetCourseService> logger)
        {
            this.db = db;
            this.coursesOptions=coursesOptions;
            this.options = options;
            this.logger = logger;
        }

        public async Task<CourseDetailModel> GetCourseAsync(int id)
        {
            logger.LogInformation("Course {id} requested",id);

            // prova accesso alle opzioni di configurazione
            CoursesOptions opt = new CoursesOptions();
            coursesOptions.GetSection(CoursesOptions.Courses).Bind(opt);

            FormattableString query = $@"SELECT Id, Title, Description, ImagePath, Author, Rating, FullPrice_Amount, FullPrice_Currency, CurrentPrice_Amount, CurrentPrice_Currency FROM Courses WHERE Id={id}
            ; SELECT Id, Title, Description, Duration FROM Lessons WHERE CourseId={id}";

            DataSet dataSet = await db.QueryAsync(query);

            //Course
            var courseTable = dataSet.Tables[0];
            if (courseTable.Rows.Count != 1)
            {
                throw new CourseNotFoundException(id);
            }

            /*
            var courseRow = courseTable.Rows[0];
            var courseDetailViewModel = CourseDetailModel.FromDataRow(courseRow);
            //Course lessons
          
            var lessonDataTable = dataSet.Tables[1];

            foreach (DataRow lessonRow in lessonDataTable.Rows)
            {
                LessonViewModel lessonViewModel = LessonViewModel.FromDataRow(lessonRow);
                courseDetailViewModel.Lessons.Add(lessonViewModel);
            }
            */

            // oppure con LINQ
            var courseDetailViewModel = courseTable.AsEnumerable().Select(x => new CourseDetailModel
            {
                Title = Convert.ToString(x["Title"]),
                Description = Convert.ToString(x["Description"]),
                ImagePath = Convert.ToString(x["ImagePath"]),
                Author = Convert.ToString(x["Author"]),
                Rating = Convert.ToDouble(x["Rating"]),
                FullPrice = new Money(
                    Enum.Parse<Currency>(Convert.ToString(x["FullPrice_Currency"])),
                    Convert.ToDecimal(x["FullPrice_Amount"])
                ),
                CurrentPrice = new Money(
                    Enum.Parse<Currency>(Convert.ToString(x["CurrentPrice_Currency"])),
                    Convert.ToDecimal(x["CurrentPrice_Amount"])
                ),
                Id = Convert.ToInt32(x["Id"]),
                Lessons = dataSet.Tables[1].AsEnumerable().Select(x => new LessonViewModel
                {
                    Id = Convert.ToInt32(x["Id"]),
                    Title = Convert.ToString(x["Title"]),
                    Description = Convert.ToString(x["Description"]),
                    Duration = TimeSpan.Parse(Convert.ToString(x["Duration"]))
                }).ToList()
            }).First();

            return courseDetailViewModel;
        }

        public async Task<ListViewModel<CourseViewModel>> GetCoursesAsync(CourseListInputModel model)
        {
            string orderby=model.OrderBy;
            if(model.OrderBy=="CurrentPrice")
            {
                orderby="CurrentPrice_Amount";
            }
            string direction= model.Ascending ? "ASC" : "DESC";

            FormattableString query = $@"SELECT Id,Title,ImagePath,Author,Rating, FullPrice_Amount,FullPrice_Currency,CurrentPrice_Amount,CurrentPrice_Currency  FROM COURSES WHERE Title LIKE {"%" +  model.Search + "%"}  ORDER BY {(Sql)orderby} {(Sql)direction} LIMIT {model.Limit} OFFSET {model.Offset};
             SELECT Count(*)  FROM COURSES WHERE Title LIKE {"%" +  model.Search + "%"} " ;          
            DataSet dataSet = await db.QueryAsync(query);
             var table = dataSet.Tables[0];
            /*
            var lista = new List<CourseViewModel>();
            foreach (DataRow r in table.Rows)
            {
                CourseViewModel course = CourseViewModel.FromDataRow(r);
                lista.Add(course);
            }
            */
            // oppure con LINQ
            var lista =  table.AsEnumerable().Select(course => new CourseViewModel{
                Id = Convert.ToInt32(course["Id"]),
                Title = (string)course["Title"],
                ImagePath = (string)course["ImagePath"],
                Author = (string)course["Author"],
                Rating = (double)course["Rating"],
                CurrentPrice = new Money(
                  Enum.Parse<Currency>(Convert.ToString(course["CurrentPrice_Currency"])),
                  Convert.ToDecimal(course["CurrentPrice_Amount"])),
                FullPrice = new Money(
                  Enum.Parse<Currency>(Convert.ToString(course["FullPrice_Currency"])),
                  Convert.ToDecimal(course["FullPrice_Amount"])),
            }).ToList();
            
            ListViewModel<CourseViewModel> result = new ListViewModel<CourseViewModel>{
                Results = lista,
                TotalCount =Convert.ToInt32( dataSet.Tables[1].Rows[0][0])
            };

            return result;
        }
   
        public async Task<List<CourseViewModel>> GetMostRecentCoursesAsync()
        {
            CourseListInputModel inputModel = new CourseListInputModel(
                search:"",
                page:1,
                orderby:"Id",
                ascending:false,
                limit:options.Value.InHome,
                coursesOption: options.Value );
            ListViewModel<CourseViewModel> result = await GetCoursesAsync(inputModel);
            return result.Results;
        }

        public async Task<List<CourseViewModel>> GetBestRatingCoursesAsync()
        {
            CourseListInputModel inputModel = new CourseListInputModel(
                search:"",
                page:1,
                orderby:"Rating",
                ascending:false,
                limit:options.Value.InHome,
                coursesOption: options.Value );
            ListViewModel<CourseViewModel> result = await GetCoursesAsync(inputModel);
            return result.Results;
        }
    }

}