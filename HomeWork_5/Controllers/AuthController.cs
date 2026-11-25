using ORISS.HomeWork_5.Framework.Attributes;
using ORISS.HomeWork_5.Services;

namespace ORISS.HomeWork_5.Controllers;

[Controller]
public class AuthController
{
    private readonly IEmailService emailService =
        new EmailService();

    [Post]
    public string SendEmail(string demoUser, string message)
    {
        if (string.IsNullOrWhiteSpace(demoUser) ||
            string.IsNullOrWhiteSpace(message))
        {
            return "Заполните все поля";
        }

        Console.WriteLine("Получено учебное сообщение");

        try
        {
            emailService.SendEmail(demoUser, message);

            return "Письмо успешно отправлено";
        }
        catch
        {
            return "Ошибка отправки письма";
        }
    }
}