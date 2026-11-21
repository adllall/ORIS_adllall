using System.Net;
using System.Text;
using System.Text.Json;

namespace ORISS.HomeWork_4.Framework.Handlers;

public class LoginHandler : RequestHandler
{
    public override async Task HandleAsync(HttpListenerContext context)
    {
        string path = context.Request.Url?.AbsolutePath ?? "";

        if (context.Request.HttpMethod != "POST" ||
            path != "/connection/Rockstar/login")
        {
            await HandleNextAsync(context);
            return;
        }

        if (context.Request.ContentLength64 > 4096)
        {
            context.Response.StatusCode = 413;
            return;
        }

        string body;
        using (StreamReader reader = new StreamReader(
                   context.Request.InputStream,
                   context.Request.ContentEncoding))
        {
            body = await reader.ReadToEndAsync();
        }

        string login;

        try
        {
            using JsonDocument document = JsonDocument.Parse(body);

            login = document.RootElement
                .GetProperty("login")
                .GetString() ?? "";
        }
        catch (JsonException)
        {
            context.Response.StatusCode = 400;
            return;
        }
        catch (KeyNotFoundException)
        {
            context.Response.StatusCode = 400;
            return;
        }

        Console.WriteLine("Получен POST-запрос");
        Console.WriteLine("Логин: " + login);
        Console.WriteLine("Пароль: [скрыт]");

        byte[] responseData =
            Encoding.UTF8.GetBytes("Запрос обработан");

        context.Response.StatusCode = 200;
        context.Response.ContentType = "text/plain; charset=utf-8";
        context.Response.ContentLength64 = responseData.Length;

        await context.Response.OutputStream.WriteAsync(responseData);
    }
}