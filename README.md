# MVCcore
Corso Udemy MVC Core - WebHD_13-7 Mantenimento dello stato / 2
03.03.2023

Ho fatto una variante rispetto al corso, inserendo nella classe
CourseListInputModel una proprietà Stato da inserire in un campo hidden
per quando si utilizza l'input dal form, oppure nella querystring
quando si utilizza un tag a.

In ogni casi il ModelBinder legge i dati di input e prepara l'oggetto
CourseListInputModel aggiungendo ai nuovi dati quelli dello stato precedente.

In tal modo ilcontroller riceve tutte le informazini compreso lo stato.

Inoltre nel file Views.Courses.Index.cshtml ho messo (come commento) 
qualche prova di lettura dei campi del form lato client,
e costruzione di un link con i dati del parametro.