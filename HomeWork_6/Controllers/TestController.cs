using MiniHttpServer.Framework.Attributes;
using MiniHttpServer.Framework.Controllers;
using MiniHttpServer.Framework.Results;

namespace ORISS.HomeWork_6.Controllers;

[Controller]
public class TestController : BaseController
{
    [Get]
    public IHttpResult Index()
    {
        return Html("<h1>Hello World!</h1>");
    }

    [Get]
    public IHttpResult Data()
    {
        return Json(new
        {
            Name = "Alex",
            Age = 20
        });
    }

    [Get]
    public IHttpResult Details(int id, string name)
    {
        return Json(new
        {
            Id = id,
            Name = name
        });
    }

    [Post]
    public string Submit()
    {
        return "OK";
    }
}