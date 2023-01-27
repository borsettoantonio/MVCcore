/*
Ho aggiunto il namespace, la classe Program, ed il metodo Main al template del dotnet.
Ho fatto qualche prova su come mandare del testo in output come pagina HTML.
*/

namespace CorsoMVCcore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
