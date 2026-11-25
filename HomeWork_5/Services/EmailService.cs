using System.Net.Mail;
using ORISS.HomeWork_5.Framework.Configuration;

namespace ORISS.HomeWork_5.Services;

public class EmailService : IEmailService
{
    public void SendEmail(string demoUser, string message)
    {
        var settings =
            ConfigurationManager.Instance.Settings;

        using var mail = new MailMessage(
            settings.EmailFrom,
            settings.EmailTo);

        mail.Subject = "HomeWork 5";

        mail.Body =
            $"Пользователь: {demoUser}\n" +
            $"Сообщение: {message}";

        using var smtp = new SmtpClient(
            settings.SmtpHost,
            settings.SmtpPort);

        smtp.EnableSsl = settings.SmtpSsl;
        smtp.Timeout = 10000;

        smtp.Send(mail);

        Console.WriteLine("Письмо отправлено");
    }
}