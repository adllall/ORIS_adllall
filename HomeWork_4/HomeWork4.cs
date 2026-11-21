using ORISS.HomeWork_4.Framework;
using ORISS.HomeWork_4.Framework.Configuration;
using ORISS.HomeWork_4.Framework.Handlers;

namespace ORISS.HomeWork_4;

public class HomeWork4
{
    public static async Task Main()
    {
        try
        {
            ServerSettings settings =
                ConfigurationManager.Instance.Settings;

            LoginHandler loginHandler = new LoginHandler();

            StaticFilesHandler staticHandler =
                new StaticFilesHandler(settings);

            NotFoundHandler notFoundHandler =
                new NotFoundHandler();

            loginHandler.SetNext(staticHandler);
            staticHandler.SetNext(notFoundHandler);

            HttpServer server = new HttpServer(settings, loginHandler);

            server.Start();

            while (true)
            {
                string? command = Console.ReadLine();

                if (command == "stop" || command == null)
                {
                    server.Stop();
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
    }
}