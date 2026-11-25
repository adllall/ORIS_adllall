using System.Text.Json;

namespace ORISS.HomeWork_5.Framework.Configuration;

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
            "HomeWork_5",
            "settings.json");

        string json = File.ReadAllText(path);

        Settings = JsonSerializer.Deserialize<ServerSettings>(json)
                   ?? throw new Exception("Ошибка загрузки настроек");
    }
}