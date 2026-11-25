using MiniHttpServer.Framework.Attributes;
using MiniHttpServer.Framework.Controllers;
using MiniHttpServer.Framework.Results;
using MiniHttpServer.ORM;
using ORISS.HomeWork_8.Models;
using System.Text.Json;

namespace ORISS.HomeWork_8.Controllers;

[Controller]
public class UserController : BaseController
{
    [Get]
    public async Task<IHttpResult> List()
    {
        string folder = Path.Combine(
            AppContext.BaseDirectory, "HomeWork_8");

        string json = File.ReadAllText(
            Path.Combine(folder, "settings.json"));

        using var document = JsonDocument.Parse(json);

        string connectionString = document.RootElement
            .GetProperty("ConnectionString").GetString()!;

        IOrmContext orm = new OrmContext(connectionString);

        var users = await orm.GetAllAsync<User>("Users");

        return new PageResult(
            Path.Combine(folder, "Templates", "User", "List.html"),
            new { Users = users });
    }
}