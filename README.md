# MVCcore Corso Udemy MVC Core - WebHD_16-17 Risoluzione di alcuni problemi con l'immagine del corso
26.04.2023

Ho visto che in Visual Studiio Code la riga

<vc:pagination-bar model="@Model"></vc:pagination-bar>

non funziona:non viene compilata. Se invece si usa Visual Studio funziona !!

Quindi in VC Code si deve usare:

<div>
    @await Component.InvokeAsync("PaginationBar",
                     new { 
                         model=@Model  }
                     )
</div>

In Edit.cshtml il campo input type="file" deve avere style="display:none;" altrimenti,
essendo associato al campo IFormFile Image dell'input model, viene visualizzato un pulsante per la scelta del file,

oltre a un altro pulsante visualizzato con il tag 

<label class="custom-file-label form-control form-control-block" asp-for="Image"></label>

Inoltre nell'input model CourseEditInputModel anche i campi che non hanno l'attributo Required 
vengono impostati a richiesti. Questo comporta la necessità di scegliere sempre una immagine 
per il campo  IFormFile Image.

Pero definendo il tipo nullable:  IFormFile? Image il campo non è più richisto.