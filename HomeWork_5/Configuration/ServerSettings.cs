namespace ORISS.HomeWork_5.Framework.Configuration;

public class ServerSettings
{
    public int Port { get; set; }

    public string Path { get; set; } = "";

    public string PublicDirectoryPath { get; set; } = "static";

    public string SmtpHost { get; set; } = "";

    public int SmtpPort { get; set; }

    public bool SmtpSsl { get; set; }

    public string EmailFrom { get; set; } = "";

    public string EmailTo { get; set; } = "";
}