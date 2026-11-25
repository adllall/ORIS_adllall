using ORISS.HomeWork_5.Framework.Attributes;

namespace ORISS.HomeWork_5.Controllers;

[Controller]
public class TestController
{
    [Get]
    public string Index()
    {
        return "Hello from controller!";
    }

    [Get]
    public string Hello()
    {
        return "Hello World!";
    }

    [Post]
    public string Submit()
    {
        return "POST request processed";
    }

    [Get]
    public string Details(int id, string name)
    {
        return $"ID: {id}, Name: {name}";
    }
}