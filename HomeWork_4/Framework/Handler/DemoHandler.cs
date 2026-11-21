
using System.Net;
using System.Text;
using System.Text.Json;

namespace ORISS.HomeWork_4.Framework.Handlers;

public class DemoHandler : RequestHandler
{
    public override async Task HandleAsync(HttpListenerContext context)
    {
        string path = context.Request.Url?.AbsolutePath ?? "";

        if (context.Request.HttpMethod != "POST" ||
            path != "/connection/demo")
        {
            await HandleNextAsync(context);
            return;
        }

        // Разрешаем только локальные запросы.
        if (!IPAddress.IsLoopback(
                context.Request.RemoteEndPoint?.Address
                ?? IPAddress.None))
        {
            context.Response.StatusCode = 403;
            return;
        }

        // Ограничиваем размер запроса.
        if (context.Request.ContentLength64 > 4096)
        {
            context.Response.StatusCode = 413;
            return;
        }

        try
        {
            using StreamReader reader = new StreamReader(
                context.Request.InputStream,
                Encoding.UTF8);

            string json = await reader.ReadToEndAsync();

            using JsonDocument document = JsonDocument.Parse(json);

            string login = document.RootElement
                .GetProperty("login")
                .GetString() ?? "";

            string password = document.RootElement
                .GetProperty("password")
                .GetString() ?? "";

            Console.WriteLine();
            Console.WriteLine("Получен POST-запрос");
            Console.WriteLine("Логин: " + login);
            Console.WriteLine("Пароль: " + password);
            Console.WriteLine();

            byte[] data = Encoding.UTF8.GetBytes("OK");

            context.Response.StatusCode = 200;
            context.Response.ContentType =
                "text/plain; charset=utf-8";
            context.Response.ContentLength64 = data.Length;

            await context.Response.OutputStream.WriteAsync(data);
        }
        catch (Exception ex) when (
            ex is JsonException ||
            ex is KeyNotFoundException ||
            ex is InvalidOperationException)
        {
            context.Response.StatusCode = 400;
        }
    }
}
