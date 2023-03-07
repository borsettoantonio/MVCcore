namespace MyCourse.Models.Options
{
    public partial class CoursesOptions
    {
        public const string Courses = "Courses";
        public int PerPage { get; set; }
        public int InHome { get; set; }

        public CoursesOrderOptions Order { get; set; }
        public int CacheDuration { get; set; }
    }

    public partial class CoursesOrderOptions
    {
        public string By { get; set; }

        public bool Ascending { get; set; }

        public string[] Allow { get; set; }
    }

}