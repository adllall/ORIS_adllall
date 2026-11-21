using System;
using System.IO;
using System.Text.Json;

namespace ORISS.HomeWork_2;

public class HomeWork2
{
    public static async Task Run()
    {
        try
        {
            string json = await File.ReadAllTextAsync(
                Path.Combine(AppContext.BaseDirectory, "HomeWork_2", "settings.json"));

            ServerSettings settings =
                JsonSerializer.Deserialize<ServerSettings>(json)
                ?? throw new Exception("Не удалось прочитать настройки");

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