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
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.Run(async context =>
                {
                    context.Response.ContentType="html";
                    await context.Response.WriteAsync("<html><body><img src='img1.jpg' alt='My image' /></body></html>");
                });

            // senza questa istruzione il programma chiude subito senza mostrare la pagina.
            app.Run();

        }

    }
}
