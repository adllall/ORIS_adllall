using System.Text.Json;

namespace ORISS.HomeWork_4.Framework.Configuration;

public sealed class ConfigurationManager
{
    private static readonly Lazy<ConfigurationManager> instance =
        new(() => new ConfigurationManager());

    public static ConfigurationManager Instance => instance.Value;

    public ServerSettings Settings { get; }

    private ConfigurationManager()
    {
        string path = Path.Combine(
            AppContext.BaseDirectory,
            "HomeWork_4",
            "settings.json");

        string json = File.ReadAllText(path);

        Settings = JsonSerializer.Deserialize<ServerSettings>(json)
                   ?? throw new Exception("Не удалось загрузить настройки");
    }
}