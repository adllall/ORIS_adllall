using System.Net;
using System.Text;

namespace ORISS.HomeWork_4.Framework.Handler;

public class LoginHandler : RequestHandler
{
    public override async Task HandleAsync(
        HttpListenerContext context)
    {
        string path = context.Request.Url?.AbsolutePath ?? "";

        if (context.Request.HttpMethod != "POST" ||
            path != "/connection/Rockstar/login")
        {
            await HandleNextAsync(context);
            return;
        }

        Console.WriteLine("Получен POST-запрос");
        Console.WriteLine("Логин: student123");
        Console.WriteLine("Пароль: example456");

        byte[] data = Encoding.UTF8.GetBytes(
            "Демонстрационный запрос обработан");

        context.Response.StatusCode = 200;

        context.Response.ContentType =
            "text/plain; charset=utf-8";

        context.Response.ContentLength64 = data.Length;

        await context.Response.OutputStream.WriteAsync(data);
    }
}