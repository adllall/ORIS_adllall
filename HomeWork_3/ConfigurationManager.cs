using System.Text.Json;

namespace ORISS.HomeWork_3;

public sealed class ConfigurationManager
{
    private static readonly Lazy<ConfigurationManager> instance =
        new Lazy<ConfigurationManager>(() => new ConfigurationManager());

    public static ConfigurationManager Instance => instance.Value;

    public ServerSettings Settings { get; }

    private ConfigurationManager()
    {
        string path = Path.Combine(
            AppContext.BaseDirectory,
            "HomeWork_3",
            "settings.json");

        string json = File.ReadAllText(path);

        Settings = JsonSerializer.Deserialize<ServerSettings>(json)
                   ?? throw new Exception("Не удалось прочитать настройки");
    }
}