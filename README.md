# MVCcore
Corso Udemy MVC Core - WebHD_09-3 Fine sezione 9

02.02.2023

Dependency injection.
Uso di  builder.Services.AddTransient<CourseService>();
oppure AddScoped per avere un oggetto che vive per tutta e sola la richiesta
oppure AddSingleton per un oggetto unico anche tra diverse richieste.
Con AddSingleton bisogna usare classi thread-safe.
Ad esempio usare Interlocked.Increment(ref count); per rendere thread-safe
un ipotetico contatore.