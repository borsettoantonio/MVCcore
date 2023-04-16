# MVCcore Corso Udemy MVC Core - WebHD_16-15 Sanitizzazione dell'input senza tag helper.
16.04.2023

Ho modificato la versione precedente richiamando la sanitizzazione nel controller

 inputModel.Sanitizza();

 e implementandola dentro la classe CourseEditInputModel.

 Ho quindi tolto il tag helper HtmlSanitizeTagHelper dalle view Detail ed Edit
 in quando il contenuto della textarea è controllato prima di memorizzarlo nel database.