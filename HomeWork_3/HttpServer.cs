using System.Net;
using System.Text;

namespace ORISS.HomeWork_3;

public class HttpServer
{
    private readonly HttpListener listener = new HttpListener();
    private readonly ServerSettings settings;
    private readonly string staticFolder;
    private readonly string basePath;

    public HttpServer(ServerSettings settings)
    {
        this.settings = settings;

        staticFolder = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "HomeWork_3",
            settings.PublicDirectoryPath));

        basePath = "/" + settings.Path.Trim('/') + "/";
    }

    public void Start()
    {
        string address =
            $"http://127.0.0.1:{settings.Port}{basePath}";

        listener.Prefixes.Add(address);
        listener.Start();

        Console.WriteLine("Сервер запущен: " + address);
        Console.WriteLine("Для остановки введите stop");

        _ = Listen();
    }

    private async Task Listen()
    {
        while (listener.IsListening)
        {
            try
            {
                HttpListenerContext context =
                    await listener.GetContextAsync();

                Console.WriteLine("Запрос: " + context.Request.Url);

                await HandleRequest(context);
            }
            catch (HttpListenerException)
            {
                if (!listener.IsListening)
                    break;

                Console.WriteLine("Ошибка HTTP-сервера");
            }
            catch (ObjectDisposedException)
            {
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }
    }

    private async Task HandleRequest(HttpListenerContext context)
    {
        HttpListenerResponse response = context.Response;

        try
        {
            string urlPath = context.Request.Url!.AbsolutePath;

            string relativePath = Uri.UnescapeDataString(
                urlPath.Substring(basePath.Length));

            relativePath = relativePath.Replace('\\', '/');

           
            if (relativePath.EndsWith("/"))
            {
                if (relativePath == "HtmlKnowledge/")
                    relativePath += "Knowledge.html";
                else if (relativePath == "Rockstar/")
                    relativePath += "rockstar.html";
                else if (relativePath == "SearchEnginer/")
                    relativePath += "time.html";
                else
                    relativePath += "index.html";
            }
            else if (relativePath == "")
            {
                relativePath = "index.html";
            }


            string filePath = Path.GetFullPath(
                Path.Combine(staticFolder, relativePath));

            string folderPrefix =
                Path.TrimEndingDirectorySeparator(staticFolder)
                + Path.DirectorySeparatorChar;

            if (!filePath.StartsWith(
                    folderPrefix,
                    StringComparison.OrdinalIgnoreCase))
            {
                await SendError(response, 403, "403 Forbidden");
                return;
            }

            if (!File.Exists(filePath))
            {
                await SendError(response, 404, "404 Not Found");
                return;
            }

            response.StatusCode = 200;
            response.ContentType = GetMimeType(filePath);
            response.Headers["Server"] = "MiniHttpServer/1.0";

            using FileStream file = File.OpenRead(filePath);

            response.ContentLength64 = file.Length;

            await file.CopyToAsync(response.OutputStream);

            Console.WriteLine("Отправлен файл: " + relativePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка обработки: " + ex.Message);

            try
            {
                await SendError(response, 500, "500 Server Error");
            }
            catch
            {
                // Клиент мог закрыть соединение
            }
        }
        finally
        {
            try
            {
                response.Close();
            }
            catch
            {
                // Соединение уже закрыто
            }
        }
    }

    private static string GetMimeType(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();

        return extension switch
        {
            ".html" or ".htm" => "text/html; charset=utf-8",
            ".css" => "text/css; charset=utf-8",
            ".js" => "text/javascript; charset=utf-8",
            ".json" => "application/json",
            ".png" => "image/png",
            ".jpg" or ".jpeg" => "image/jpeg",
            ".svg" => "image/svg+xml",
            ".gif" => "image/gif",
            ".ico" => "image/x-icon",
            ".woff" => "font/woff",
            ".woff2" => "font/woff2",
            ".ttf" => "font/ttf",
            ".pdf" => "application/pdf",
            _ => "application/octet-stream"
        };
    }

    private static async Task SendError(
        HttpListenerResponse response,
        int statusCode,
        string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);

        response.StatusCode = statusCode;
        response.ContentType = "text/plain; charset=utf-8";
        response.Headers["Server"] = "MiniHttpServer/1.0";
        response.ContentLength64 = data.Length;

        await response.OutputStream.WriteAsync(data);
    }

    public void Stop()
    {
        listener.Stop();
        listener.Close();

        Console.WriteLine("Сервер остановлен");
    }
}
