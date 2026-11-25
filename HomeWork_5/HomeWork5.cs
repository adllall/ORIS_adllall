using ORISS.HomeWork_5.Framework.Configuration;
using ORISS.HomeWork_5.Framework.Handler;
using ORISS.HomeWork_5.Services;

namespace ORISS.HomeWork_5;

public class HomeWork5
{
    public static void Run()
    {
        try
        {
            var settings =
                ConfigurationManager.Instance.Settings;

            var staticHandler =
                new StaticFilesHandler(settings);

            var controllersHandler =
                new ControllersHandler();

            var notFoundHandler =
                new NotFoundHandler();

            staticHandler.SetNext(controllersHandler);

            controllersHandler.SetNext(notFoundHandler);

            var server = new HttpServer(
                settings, staticHandler);

            server.Start();
            try
            {
                SmtpTest.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка SMTP: " + ex.Message);

                if (ex.InnerException != null)
                {
                    Console.WriteLine(
                        "Причина: " + ex.InnerException.Message);
                }
            }
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