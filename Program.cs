using MyCourse.Customizations.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MyCourse.Models.Options;
using MyCourse.Models.Services.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Identity;
using MyCourse.Models.Entities;
using MyCourse.Customizations.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using pgm3.Models.Services.Application.Courses;
using MyCourse.Models.Services.Application.Lessons;

namespace CorsoMVCcore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AuthorizationPolicyBuilder policyBuilder = new();
            AuthorizationPolicy policy = policyBuilder.RequireAuthenticatedUser().Build();
            AuthorizeFilter filter = new(policy);

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(filter);
            });

            builder.Services.AddResponseCaching();
            //builder.Services.AddOutputCache();

            // middleware per MVC
            // builder.Services.AddControllersWithViews();

            // middleware per MVC con aggiunta di un profilo
            builder.Services.AddControllersWithViews(options =>
            {
                var homeProfile = new CacheProfile();
                // per far variare la chiave di caching anche al variare del parametro "page" sulla querystring
                // homeProfile.VaryByQueryKeys = new string[]{"page"};
                // oppure si può usare il profilo

                //homeProfile.Duration= builder.Configuration.GetValue<int>("ResponseCache:Home:Duration");
                //homeProfile.Location= builder.Configuration.GetValue<ResponseCacheLocation>("ResponseCache:Home:Location");
                // oppure
                builder.Configuration.Bind("ResponseCache:Home", homeProfile);
                options.CacheProfiles.Add("Home", homeProfile);

                options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
            });

            builder.Services.AddTransient<ICourseService, AdoNetCourseService>();
            builder.Services.AddTransient<ILessonService, AdoNetLessonService>();
            builder.Services.AddTransient<IDatabaseAccessor, SqliteDatabaseAccessor>();
            builder.Services.AddTransient<ICachedCourseService, MemoryCacheCourseService>();
            builder.Services.AddTransient<ICachedLessonService, MemoryCacheLessonService>();

            builder.Services.AddTransient<IImagePersister, InsecureImagePersister>();
            builder.Services.AddSingleton<IImagePersister, MagickNetImagePersister>();
            builder.Services.AddSingleton<IEmailSender, MailKitEmailSender>();
            builder.Services.AddSingleton<IEmailClient, MailKitEmailSender>();

            // Identity Service
            // ApplicationUser deve essere il tipo che descrive l'utente
            var identityBuilder = builder.Services.AddDefaultIdentity<ApplicationUser>(option =>
            {
                option.Password.RequireDigit = true;
                option.Password.RequiredLength = 8;
            });
            //Imposta l'AdoNetUserStore come servizio di persistenza per Identity
            identityBuilder.AddUserStore<AdoNetUserStore>().AddClaimsPrincipalFactory<CustomClaimsPrincipalFactory>();

            builder.Services.AddRazorPages();

            // Opzioni di configurazioine
            builder.Services.Configure<CoursesOptions>(builder.Configuration.GetSection(CoursesOptions.Courses));
            builder.Services.Configure<MemoryCacheOptions>(builder.Configuration.GetSection("MemoryCache"));
            builder.Services.Configure<KestrelServerOptions>(builder.Configuration.GetSection("Kestrel"));
            builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("Smtp"));

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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // versione per Core 7
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            // senza questa istruzione il programma chiude subito senza mostrare la pagina.
            app.Run();

        }

    }
}
