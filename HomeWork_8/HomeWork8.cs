using System.Text.Json;
using MiniHttpServer.Framework;
using MiniHttpServer.Framework.Configuration;
using MiniHttpServer.Framework.Handler;
using MiniHttpServer.ORM;
using ORISS.HomeWork_8.Models;
namespace ORISS.HomeWork_8;

public class HomeWork8
{
    public static void Main()
    {
        string path = Path.Combine(
            AppContext.BaseDirectory,
            "HomeWork_8",
            "settings.json");

        var settings = JsonSerializer.Deserialize<ServerSettings>(
            File.ReadAllText(path))!;
        //TestOrm(settings.ConnectionString)
          //  .GetAwaiter().GetResult();
        var controllersHandler = new ControllersHandler(
            "ORISS.HomeWork_8.Controllers");

        var notFoundHandler = new NotFoundHandler();

        controllersHandler.SetNext(notFoundHandler);

        var server = new HttpServer(settings, controllersHandler);

        server.Start();

        Console.WriteLine("http://localhost:8894/user/list");
        Console.WriteLine("Нажмите Enter для остановки");

        Console.ReadLine();

        server.Stop();
    }
    private static async Task TestOrm(string connectionString)
    {
        IOrmContext orm = new OrmContext(connectionString);

        var product = new Product
        {
            Name = "TestProduct_" + Guid.NewGuid().ToString("N"),
            Price = 1000
        };

        int inserted = await orm.InsertAsync("Products", product);
        Console.WriteLine($"Добавлено: {inserted}");

        var products = await orm.GetAllAsync<Product>("Products");

        var item = products.First(p => p.Name == product.Name);

        item.Price = 2000;

        int updated = await orm.UpdateAsync(
            "Products", item.Id, item);

        Console.WriteLine($"Изменено: {updated}");

        int deleted = await orm.DeleteAsync("Products", item.Id);

        Console.WriteLine($"Удалено: {deleted}");

        int repeated = await orm.DeleteAsync("Products", item.Id);

        Console.WriteLine($"Повторное удаление: {repeated}");
        Console.WriteLine("\nПроверка Users:");

        var user = new User
        {
            Name = "TestUser_" + Guid.NewGuid().ToString("N"),
            Email = "test@example.com",
            Age = 20
        };

        int userInserted = await orm.InsertAsync("Users", user);
        Console.WriteLine($"Добавлено: {userInserted}");

        var users = await orm.GetAllAsync<User>("Users");

        var testUser = users.First(u => u.Name == user.Name);

        testUser.Age = 25;

        int userUpdated = await orm.UpdateAsync(
            "Users", testUser.Id, testUser);

        Console.WriteLine($"Изменено: {userUpdated}");

        int userDeleted = await orm.DeleteAsync(
            "Users", testUser.Id);

        Console.WriteLine($"Удалено: {userDeleted}");

        int userRepeated = await orm.DeleteAsync(
            "Users", testUser.Id);

        Console.WriteLine($"Повторное удаление: {userRepeated}");
    }
}