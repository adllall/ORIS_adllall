using System.Net;
using System.Text;

namespace ORISS.HomeWork_5.Framework.Handler;

public class NotFoundHandler : RequestHandler
{
    public override async Task HandleAsync(
        HttpListenerContext context)
    {
        byte[] data = Encoding.UTF8.GetBytes(
            "404 Not Found");

        context.Response.StatusCode = 404;
        context.Response.ContentType =
            "text/plain; charset=utf-8";

        context.Response.ContentLength64 = data.Length;

        await context.Response.OutputStream.WriteAsync(data);

        Console.WriteLine(
            "404 Not Found: " + context.Request.Url);
    }
}