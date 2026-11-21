
using System.Net;
using ORISS.HomeWork_4.Framework.Configuration;
using ORISS.HomeWork_4.Framework.Handlers;

namespace ORISS.HomeWork_4.Framework;

public class HttpServer
{
    private readonly HttpListener listener = new HttpListener();
    private readonly ServerSettings settings;
    private readonly RequestHandler handler;

    public HttpServer(
        ServerSettings settings,
        RequestHandler handler)
    {
        this.settings = settings;
        this.handler = handler;
    }

    public void Start()
    {
        string address =
            $"http://127.0.0.1:{settings.Port}/{settings.Path.Trim('/')}/";

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
            HttpListenerContext context;

            try
            {
                context = await listener.GetContextAsync();
            }
            catch (HttpListenerException)
            {
                if (!listener.IsListening)
                    break;

                Console.WriteLine("Ошибка получения запроса");
                continue;
            }
            catch (ObjectDisposedException)
            {
                break;
            }

            try
            {
                await handler.HandleAsync(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка обработки: " + ex.Message);

                try
                {
                    context.Response.StatusCode = 500;
                }
                catch
                {
                }
            }
            finally
            {
                try
                {
                    context.Response.Close();
                }
                catch
                {
                }
            }
        }
    }

    public void Stop()
    {
        listener.Stop();
        listener.Close();

        Console.WriteLine("Сервер остановлен");
    }
}
