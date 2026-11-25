using System.Net;
using ORISS.HomeWork_5.Framework.Configuration;
using ORISS.HomeWork_5.Framework.Handler;

namespace ORISS.HomeWork_5;

public class HttpServer
{
    private readonly HttpListener listener = new();

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
            $"http://127.0.0.1:{settings.Port}/";

        listener.Prefixes.Add(address);
        listener.Start();

        Console.WriteLine("Сервер запущен: " + address);
        Console.WriteLine("Для остановки введите stop");

        _ = ListenAsync();
    }

    private async Task ListenAsync()
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
                break;
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
                Console.WriteLine("Ошибка: " + ex.Message);

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
                context.Response.Close();
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