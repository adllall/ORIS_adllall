using System;

namespace ORISS.HomeWork_3;

public class HomeWork3
{
    public static async Task Run()
    {
        try
        {
            ServerSettings settings =
                ConfigurationManager.Instance.Settings;

            HttpServer server = new HttpServer(settings);

            server.Start();

            while (true)
            {
                string? command = Console.ReadLine();

                if (command == "stop")
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