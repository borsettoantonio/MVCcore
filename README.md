# MVCcore Corso Udemy MVC Core - WebHD_16-15 Sanitizzazione dell'input
16.04.2023

Sanitizzazione dell'input contenente del codice Html, usando un tag helper,
HtmlSanitizeTagHelper attivato dalla presenza di un particolare attributo.

Uso della libreria HtmlSanitizer (presa da NuGet) che toglie i tag non voluti
dall'html da sanitizzare.

E suo utilizzo sia in Detail.cshtml che in Edit.cshtml.

La classe video-container applicata dal tag helper nel metodo ProcessIFrames 
di post-processing permette di ridimensionare il video al ridimensionamento
della finestra.

Ho avuto dei problemi nella view Edit.cshtml a far convivere
sulla textarea "Description" l'attributo asp-for ( per collegare la textarea 
ai controlli di validazione), 
con l'attributo data-summernote (per collegare la textarea all'editor summernote)
e con l'attributo html-sanitize (per sanitizzare il campo che contiene html).

Bisogna togliere asp-for="Decription" e aggiundere 

id="Description"  name="Description"

Su CourseEditInputModel.cs ho dovuto mettere l'attributo:

[MinLength(21, ErrorMessage = "La descrizione dev'essere di almeno 10 caratteri")]

sul campo Description, perche anche con la text area vuota arrivano 11 caratteri
e quindi ho dovuto mettere 21 come lunghezza minima per avere al minimo 10 caratteri
inseriti dall'utente.

Devo provare a fare il controllo dell'input prima di memorizzare al textarea,
probabilmente tolgo tanti problemi.