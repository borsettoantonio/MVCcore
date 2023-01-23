/*
Ho aggiunto il namespace, la classe Program, ed il metodo Main al template del dotnet.
Ho fatto qualche prova su come mandare del testo in output.
*/

namespace CorsoMVCcore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            // aggiungendo il codice successivo, app.MapGet non funziona.
            // togliendo app.Use() e app.Run() l'istruzione produce l'output.
            // Probabile che funzioni solo per Minimal APIs apps
            //app.MapGet("", () => "Ciao ciao!");
            //app.MapGet("",HandleMapTest2);

            // Map invece funziona e crea un branch sulla pipeline dei middleware
            app.Map("/map1", HandleMapTest1);
            
            //  ATTENZIONE!!
            //  Map e MapGet usano due diversi tipi di delegati

            //  Use() permette di inserire un middleware e poi con next ne posso aggiungere un altro.
            app.Use(async (context, next) =>
                {
                    await context.Response.WriteAsync(context.Request.QueryString.ToString() + "    \n");
                    string z = Microsoft.Extensions.Hosting.Environments.Development; // = Development
                    if (app.Environment.IsDevelopment())                              // presa da "ASPNETCORE_ENVIRONMENT":
                                                                                      // in launch.json e confrontata con z  
                        await context.Response.WriteAsync(z + "\n");
                    await context.Response.WriteAsync(z + "\n");
                    // in questo modo posso controllare un nome arbitrario per il development environment
                    if (app.Environment.IsEnvironment("Developmentxxx"))
                        await context.Response.WriteAsync("Developmentxxx" + "\n");
                    // next chiama il successivo middleware  
                    await next.Invoke();
                    // questo codice è eseguito al ritorno nella catena dei middleware
                    await context.Response.WriteAsync("dopo nome" + "\n");
                });

            app.Run(async context =>
                {
                    await context.Response.WriteAsync("pippo");
                });

            // senza questa istruzione il programma chiude subito senza mostrare la pagina.
            app.Run();

        }

        static void HandleMapTest1(IApplicationBuilder app)
        {
             app.Use(async (context, next) =>
             {
                await context.Response.WriteAsync("primo middleware " + "\n");
                await next.Invoke();
             });

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Map Test 1");
            });
        }

        static async void HandleMapTest2(HttpContext context) 
        {
            await context.Response.WriteAsync("Map Test 1");
        }
    }
}
