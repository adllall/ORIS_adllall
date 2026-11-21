using System.Net;
using System.Text;

namespace ORISS.HomeWork_4.Framework.Handlers;

public class NotFoundHandler : RequestHandler
{
    public override async Task HandleAsync(HttpListenerContext context)
    {
        HttpListenerResponse response = context.Response;

        byte[] data = Encoding.UTF8.GetBytes("404 Not Found");

        response.StatusCode = 404;
        response.ContentType = "text/plain; charset=utf-8";
        response.ContentLength64 = data.Length;

        await response.OutputStream.WriteAsync(data);

        Console.WriteLine("404 Not Found: " + context.Request.Url);
    }
}