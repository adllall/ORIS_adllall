using MiniHttpServer.Framework.Attributes;
using MiniHttpServer.Framework.Controllers;
using MiniHttpServer.Framework.Results;

namespace ORISS.HomeWork_6.Controllers;

[Controller]
public class AuthController : BaseController
{
    [Post]
    public IHttpResult Login()
    {
        return Html("<h1>Учебный запрос обработан</h1>");
    }
}