using Microsoft.AspNetCore.Mvc;
using MyCourse.Customizations.ModelBinders;
using MyCourse.Models.Options;

namespace MyCourse.Models.InputModels
{
    [ModelBinder(BinderType = typeof(CourseListInputModelBinder))]
    public class CourseListInputModel
    {
        public CourseListInputModel(string search, int page, string orderby, bool ascending, int limit,CoursesOptions coursesOption)
        {
            // Sanitizzazione
            var orderOptions = coursesOption.Order;
            if (!orderOptions.Allow.Contains(orderby))
            {
                orderby = orderOptions.By;
                ascending = orderOptions.Ascending;
            }

            Search = search ?? "";
            Page = Math.Max(1, page);
            Limit=Math.Max(1,limit);
            OrderBy = orderby;
            Ascending = ascending;

            Offset = (Page - 1) * Limit;
        }
        public string Search { get; }
        public int Page { get; }
        public string OrderBy { get; }
        public bool Ascending { get; }
        public int Limit { get; }
        public int Offset { get; }
        public string Stato
        {
            get
            {
                return $"{Search};{Page};{OrderBy};{Ascending};{Limit};{Offset}";
            }
        }
        public static CourseListInputModel Decode(string stato, CoursesOptions coursesOption)
        {
            if(stato==null)
                return null;
            string[] dati = stato.Split(';');
            return new CourseListInputModel(
                dati[0],
                Convert.ToInt32(dati[1]),
                dati[2],
                Convert.ToBoolean(dati[3]),
                1,
                coursesOption
            );
        } 
    }
}