# MVCcore Corso Udemy MVC Core - WebHD_16-14 Gestione input/output con editor evoluto
10.04.2023

Uso di Summernote editor.

In _Summernote.cshtml ho aggiunto $(document).ready(function() {...}
prima della inizializzazioe di summernote, perchè altrimenti non funziona
in quanto viene chiamata prima del completamento del load della pagina.

Uso di Html.Raw(...) in Detail.cshtml per stampare una stringa senza l'encoding html,
in modo da poter rendere anche la formattazione html presente nella stringa.

La modifica di un corso non funziona benissimo perchè occorre inserire sempre
un corso, anche se uno è già presente. 