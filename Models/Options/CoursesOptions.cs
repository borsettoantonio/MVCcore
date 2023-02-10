namespace MyCourse.Models.Options
{
    public partial class CoursesOptions
    {
        public const string Courses = "Courses";
        public long PerPage { get; set; }

        public CoursesOrderOptions Order { get; set; }
    }

    public partial class CoursesOrderOptions
    {
        public string By { get; set; }

        public bool Ascending { get; set; }

        public string[] Allow { get; set; }
    }

}