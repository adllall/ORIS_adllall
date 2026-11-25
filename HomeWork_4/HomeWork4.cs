using ORISS.HomeWork_4.Framework.Configuration;
using ORISS.HomeWork_4.Framework.Handler;

namespace ORISS.HomeWork_4;

public class HomeWork4
{
    public static void Run()
    {
        try
        {
            var settings = ConfigurationManager.Instance.Settings;

            var loginHandler = new LoginHandler();
            var staticHandler = new StaticFilesHandler(settings);
            var notFoundHandler = new NotFoundHandler();

            loginHandler.SetNext(staticHandler);
            staticHandler.SetNext(notFoundHandler);

            var server = new HttpServer(settings, loginHandler);

            server.Start();

            while (Console.ReadLine() is string command)
            {
                if (command == "stop")
                    break;
            }

            server.Stop();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
    }
}