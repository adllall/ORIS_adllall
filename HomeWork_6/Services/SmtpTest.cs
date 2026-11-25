using System.Net;
using System.Net.Mail;
using MiniHttpServer.Framework.Configuration;

namespace ORISS.HomeWork_6.Services;

public static class SmtpTest
{
    public static void Run()
    {
        var settings = ConfigurationManager.Instance.Settings;

        string password =
            Environment.GetEnvironmentVariable("SMTP_PASSWORD")
            ?? throw new Exception("Пароль SMTP не настроен");

        using var mail = new MailMessage(
            settings.EmailFrom,
            settings.EmailTo);

        mail.Subject = "HomeWork 5 - SMTP Test";
        mail.Body = "Тестовое письмо. Проверка работы SMTP.";

        using var smtp = new SmtpClient(
            settings.SmtpHost,
            settings.SmtpPort);

        smtp.EnableSsl = true;
        smtp.UseDefaultCredentials = false;

        smtp.Credentials = new NetworkCredential(
            settings.EmailFrom,
            password);

        smtp.Timeout = 60000;
        Console.WriteLine("SMTP: " + settings.SmtpHost);
        Console.WriteLine("Порт: " + settings.SmtpPort);
        Console.WriteLine("Начинается отправка письма...");

       
        smtp.Send(mail);

        Console.WriteLine("Тестовое письмо отправлено!");
    }
}