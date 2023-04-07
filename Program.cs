using MyCourse.Customizations.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
            builder.Services.AddResponseCaching();
            //builder.Services.AddOutputCache();

            // middleware per MVC
            // builder.Services.AddControllersWithViews();

            // middleware per MVC con aggiunta di un profilo
            builder.Services.AddControllersWithViews(options =>
            {
                var homeProfile= new CacheProfile();
                // per far variare la chiave di caching anche al variare del parametro "page" sulla querystring
               // homeProfile.VaryByQueryKeys = new string[]{"page"};
                // oppure si può usare il profilo
                
                //homeProfile.Duration= builder.Configuration.GetValue<int>("ResponseCache:Home:Duration");
                //homeProfile.Location= builder.Configuration.GetValue<ResponseCacheLocation>("ResponseCache:Home:Location");
                // oppure
                builder.Configuration.Bind("ResponseCache:Home",homeProfile);
                options.CacheProfiles.Add("Home",homeProfile);

                options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());

            });
            builder.Services.AddTransient<ICourseService, AdoNetCourseService>();
            builder.Services.AddTransient<IDatabaseAccessor, SqliteDatabaseAccessor>();
            builder.Services.AddTransient<ICachedCourseService, MemoryCacheCourseService>();

            builder.Services.AddTransient<IImagePersister, InsecureImagePersister>();


            // Opzioni di configurazioine
            builder.Services.Configure<CoursesOptions>(builder.Configuration.GetSection(CoursesOptions.Courses));
            builder.Services.Configure<MemoryCacheOptions>(builder.Configuration.GetSection("MemoryCache"));
            
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            //Nel caso volessi impostare una Culture specifica...
            /*var appCulture = CultureInfo.InvariantCulture;
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(appCulture),
                SupportedCultures = new[] { appCulture }
            });*/

            app.UseResponseCaching();
            //app.UseOutputCache();

            // versione per Core 7
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // senza questa istruzione il programma chiude subito senza mostrare la pagina.
            app.Run();

        }

    }
}
