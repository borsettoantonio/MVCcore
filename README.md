# MVCcore
Corso Udemy MVC Core - WebHD_12-6 Servizio di caching
17.02.2023

Utilizzo del servizio IMemoryCache.

Uso di SetSize(n) per limitare l'uso di RAM, e di MemoryCache.Remove(key)
per liberare della memoria.

Servizio di cache distribuita.

Response Caching, esempio sull'HomeController. Per fare il cache della pagina html 
sul browser.

ResponseCachingMiddleware per fare il caching di pagine html per tutta l'applicazione.

L'action deve avere specificato "Cache_Control: public" con l'attributo ResponseCache con Location = Any.


