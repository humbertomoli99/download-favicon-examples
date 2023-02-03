using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Web.WebView2.Core;

namespace DownloadFavicon
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string url = "https://www.example.com";
            await DownloadFaviconAsync(url);
        }

        private static async Task DownloadFaviconAsync(string url)
        {
            // Crea una instancia de CoreWebView2
            CoreWebView2 webView = new CoreWebView2();
            try
            {
                // Intenta descargar el favicon directamente a partir de la URL https://www.example.com/favicon.ico
                var faviconUrl = $"https://{new Uri(url).Host}/favicon.ico";
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(faviconUrl);

                // Si la descarga directa falla, busca el elemento "link" con "rel=shortcut icon"
                if (!response.IsSuccessStatusCode)
                {
                    // Buscar el elemento "link" con "rel=shortcut icon"
                    var result = await webView.ExecuteScriptAsync($"document.querySelector('link[rel*=\'icon\']').href");
                    faviconUrl = result.ToString();

                    // Si no se encuentra, muestra un error
                    if (string.IsNullOrEmpty(faviconUrl))
                    {
                        Console.WriteLine("No se encontró ningún favicon en la página");
                        return;
                    }
                }

                // Determinar el formato de descarga basándose en la extensión original
                var format = GetFaviconFormat(faviconUrl);

                // Si el formato no es soportado, muestra un error
                if (format == "")
                {
                    Console.WriteLine("Formato de favicon no soportado");
                    return;
                }

                // Descargar el favicon
                response = await client.GetAsync(faviconUrl);
                var favicon = await response.Content.ReadAsByteArrayAsync();

                // Guardar el favicon en el disco local
                System.IO.File.WriteAllBytes($"favicon.{format}", favicon);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Liberar la instancia de CoreWebView2
                webView.Close();
            }
        }

        private string GetFaviconFormat(string faviconUrl)
        {
            Dictionary<string, string> supportedFormats = new Dictionary<string, string>
            {
                { "ico", "ico" },
                { "png", "png" },
                { "svg", "svg" },
                { "gif", "gif" },
                { "jpg", "jpg" }
            };

            string extension = Path.GetExtension(faviconUrl).TrimStart('.');

            return supportedFormats.ContainsKey(extension) ? supportedFormats[extension] : throw new Exception("Formato de favicon no soportado");
        }
    }
}
