using MiniHttpServer.Framework.Attributes;
using MiniHttpServer.Framework.Controllers;
using MiniHttpServer.Framework.Results;

namespace ORISS.HomeWork_6.Controllers;

[Controller]
public class UserController : BaseController
{
    [Get]
    public IHttpResult Profile()
    {
        var model = new
        {
            Name = "Adelinka",
            Age = 20,
            IsActive = true,

            User = new
            {
                City = "Kazan"
            },

            Subjects = new[]
            {
                "Матан",
                "Рус",
                "Окр мир"
            }
        };

        return Page("User/Profile.html", model);
    }

    [Get]
    public IHttpResult List()
    {
        var model = new
        {
            Users = new[]
            {
                new { Name = "Adelinka", IsActive = true },
                new { Name = "Malinka", IsActive = false },
                new { Name = "Super", IsActive = true }
            }
        };

        return Page("User/List.html", model);
    }
}