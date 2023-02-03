# download-favicon-examples
Este código se encarga de descargar el favicon de una página web dada su URL.

La función downloadFavicon recibe como parámetro la URL de la página web y devuelve una promesa que se resuelve con un objeto que contiene la URL del blob del favicon y su formato.

El proceso de descarga del favicon se realiza en dos etapas:

Intenta descargar directamente el favicon a partir de la URL https://www.example.com.mx/favicon.ico.
Si la descarga directa falla, busca el elemento "link" con "rel=shortcut icon" en la página y obtiene la URL del favicon a partir de él.
En caso de que no se encuentre el favicon en la página, se rechazará la promesa con un error "No se encontró ningún favicon en la página".

En caso de que el formato del favicon no sea soportado (ico, png, svg, gif o jpg), se rechazará la promesa con un error "Formato de favicon no soportado".

Finalmente, se crea un enlace para descargar el favicon y se invoca su método click para que se lance la descarga.

En caso de error en cualquiera de las etapas, se muestra un mensaje de error en la consola.
