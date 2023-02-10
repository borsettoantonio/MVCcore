# MVCcore
Corso Udemy MVC Core - WebHD_12-2 Servizio di configurazione con json
10.02.2023

Uso del servizio di configurazione.

Primo metodo:

usato nel metodo SqliteDatabaseAccessor.QueryAsync: il costruttore
riceve per dependency injection una istanza di IConfiguration config 
con cui accedere alle sezioni del file appsettings.jason con

config.GetSection("ConnectionStrings").GetValue<string>("Default")


Secondo metodo:

usato nel metodo AdoNetCourseService.GetCourseAsync: si utilizza una classe 
che conterrà le opzioni e se ne crea una istanza; 
come prima si ottine un oggetto IConfiguration coursesOptions
per dependency injection, e si collega l'oggetto delle opzioni con il file
appsettins.jason con:

CoursesOptions opzioni=new CoursesOptions();

coursesOptions.GetSection(CoursesOptions.Position).Bind(opzioni);

Terzo metodo:

nel Main si usa:

builder.Services.Configure<CoursesOptions>(builder.Configuration.GetSection(CoursesOptions.Courses));

per collegare il file jason con la classe CoursesOptions. Quindi nelle classi dove serve
si inietta una istanza di IOptions<CoursesOptions> options, che una istanza della classe 
CoursesOptions che descrive le opzioni.


