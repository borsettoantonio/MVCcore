using MyCourse.Models.InputModels;
using MyCourse.Models.ViewModels;

namespace pgm3.Models.ViewModels.Courses
{
    public class CourseListViewModel : IPaginationInfo
    {
        public ListViewModel<CourseViewModel> Courses { get; set; }
        public CourseListInputModel Input { get; set; }

        int IPaginationInfo.CurrentPage => Input.Page;

        int IPaginationInfo.TotalResults => Courses.TotalCount;

        int IPaginationInfo.ResultsPerPge => Input.Limit;

        string IPaginationInfo.Search => Input.Search;

        string IPaginationInfo.OrderBy => Input.OrderBy;

        bool IPaginationInfo.Ascending => Input.Ascending;
        string IPaginationInfo.Stato => Input.Stato;
    }
}