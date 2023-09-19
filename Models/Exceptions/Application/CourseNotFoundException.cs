namespace pgm3.Models.Exceptions.Application
{
    public class CourseNotFoundException : Exception
    {
        public CourseNotFoundException(int courseId) : base($"Course {courseId} not found")
        {
        }
    }

}