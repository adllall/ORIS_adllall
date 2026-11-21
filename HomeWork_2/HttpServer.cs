
using System.Net;
using System.Text;

namespace ORISS.HomeWork_2;

public class HttpServer
{
    private readonly HttpListener listener = new HttpListener();
    private readonly ServerSettings settings;

    public HttpServer(ServerSettings settings)
    {
        this.settings = settings;
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
            try
            {
                HttpListenerContext context =
                    await listener.GetContextAsync();

                Console.WriteLine("Получен запрос: " +
                                  context.Request.Url);

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
            string html = await File.ReadAllTextAsync(
                Path.Combine(AppContext.BaseDirectory, "HomeWork_2", "Knowledge.html"));

            byte[] data = Encoding.UTF8.GetBytes(html);

            response.StatusCode = 200;
            response.ContentType = "text/html; charset=utf-8";
            response.ContentLength64 = data.Length;

            await response.OutputStream.WriteAsync(data);

            Console.WriteLine("Страница Knowledge.html отправлена");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка обработки: " + ex.Message);

            try
            {
                response.StatusCode = 500;
            }
            catch (HttpListenerException)
            {
            }
        }
        finally
        {
            response.Close();
        }
    }

    public void Stop()
    {
        listener.Stop();
        listener.Close();

        Console.WriteLine("Сервер остановлен");
    }
}
