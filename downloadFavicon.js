var url = document.location.href;

function downloadFavicon(url) {
  return new Promise((resolve, reject) => {
    // Intenta descargar el favicon directamente a partir de la URL https://www.example.com.mx/favicon.ico
    fetch("https://" + url.split("//")[1].split("/")[0] + "/favicon.ico")
      .then(response => {
        if (!response.ok) {
          throw new Error(`Error HTTP ${response.status}: ${response.statusText}`);
        }
        return response.blob();
      })
      .then(blob => {
        var blobURL = URL.createObjectURL(blob);
        resolve({ blobURL, format: "ico" });
      })
      .catch(error => {
        // Buscar el elemento "link" con "rel=shortcut icon"
        var link = document.querySelector("link[rel*='icon']");
    
        // Si no se encuentra, rechazar la promesa
        if (!link) {
          reject(new Error("No se encontró ningún favicon en la página"));
        }
    
        // Obtener la URL del favicon
        var faviconURL = link.href;
    
        // Determinar el formato de descarga basándose en la extensión original
        var extension = faviconURL.split(".").pop();
        var format = ["ico", "png", "svg", "gif", "jpg"].includes(extension) ? extension : reject(new Error("Formato de favicon no soportado"));
          
        // Descargar el favicon
        fetch(faviconURL)
          .then(response => response.blob())
          .then(blob => {
            var blobURL = URL.createObjectURL(blob);
            resolve({ blobURL, format });
          })
          .catch(error => reject(error));
      });
  });
}

downloadFavicon(url)
  .then(({ blobURL, format }) => {
    // Crear un enlace para descargar el favicon
    var a = document.createElement("a");
    a.href = blobURL;
    a.download = "favicon." + format;
    a.click();
  })
  .catch(error => console.error(error));
