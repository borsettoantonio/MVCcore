using MyCourse.Models.Options;
using MyCourse.Models.Services.Application;
using MyCourse.Models.Services.Infrastructure;

namespace CorsoMVCcore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<ICourseService,AdoNetCourseService>();
            builder.Services.AddTransient<IDatabaseAccessor,SqliteDatabaseAccessor>();

            // Opzioni di configurazioine
            builder.Services.Configure<CoursesOptions>(builder.Configuration.GetSection(CoursesOptions.Courses));
       
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else{
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            
            // versine per Core 7
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // senza questa istruzione il programma chiude subito senza mostrare la pagina.
            app.Run();

        }

    }
}
