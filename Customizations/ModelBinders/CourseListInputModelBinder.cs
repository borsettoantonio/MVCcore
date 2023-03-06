using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using MyCourse.Models.InputModels;
using MyCourse.Models.Options;

namespace MyCourse.Customizations.ModelBinders
{
    public class CourseListInputModelBinder : IModelBinder
    {
        private readonly IOptions<CoursesOptions> coursesOptions;
        public CourseListInputModelBinder(IOptions<CoursesOptions> coursesOptions)
        {
            this.coursesOptions = coursesOptions;
        }
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var st = CourseListInputModel.Decode(bindingContext.ValueProvider.GetValue("Input.Stato").FirstValue, coursesOptions.Value);
            // Recuperiamo i valori grazie ai value provider
            string search = bindingContext.ValueProvider.GetValue("Search").FirstValue;
            int page = Convert.ToInt32(bindingContext.ValueProvider.GetValue("Page").FirstValue);
            string orderby = bindingContext.ValueProvider.GetValue("OrderBy").FirstValue;
            bool ascending = Convert.ToBoolean(bindingContext.ValueProvider.GetValue("Ascending").FirstValue);
            if (st != null)
            {
                if (search != null && st.Search != search)
                    page = 1;
                else if (page == 0)
                    page = st.Page;
               
                search = search ?? st.Search;
                orderby = orderby ?? st.OrderBy;
            }

            // Creiamo l'istanza del CourseListInputModel
            var inputModel = new CourseListInputModel(search, page, orderby, ascending, coursesOptions.Value);

            // Impostiamo il risultato per notificare che la creazione è avvenuta con successo
            bindingContext.Result = ModelBindingResult.Success(inputModel);

            // restituiamo un task completato
            return Task.CompletedTask;

        }
    }
}