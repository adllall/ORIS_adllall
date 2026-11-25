using MiniHttpServer.Framework;
using MiniHttpServer.Framework.Configuration;
using MiniHttpServer.Framework.Handler;

namespace ORISS.HomeWork_6;

public class HomeWork6
{
    public static void Run()
    {
        var settings = ConfigurationManager.Instance.Settings;

        var staticHandler = new StaticFilesHandler(settings);
        var controllersHandler = new ControllersHandler();
        var notFoundHandler = new NotFoundHandler();

        staticHandler.SetNext(controllersHandler);
        controllersHandler.SetNext(notFoundHandler);

        var server = new HttpServer(settings, staticHandler);

        server.Start();

        while (Console.ReadLine() is string command)
        {
            if (command == "stop")
                break;
        }

        server.Stop();
    }
}