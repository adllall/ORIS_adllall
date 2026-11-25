using System.Net;
using System.Reflection;
using System.Text;
using System.Globalization;
using ORISS.HomeWork_5.Framework.Attributes;

namespace ORISS.HomeWork_5.Framework.Handler;

public class ControllersHandler : RequestHandler
{
    public override async Task HandleAsync(
        HttpListenerContext context)
    {
        string path = context.Request.Url?.AbsolutePath ?? "";

        string[] parts = path.Trim('/').Split('/');

        if (parts.Length != 2)
        {
            await HandleNextAsync(context);
            return;
        }

        string controllerName = parts[0] + "Controller";
        string methodName = parts[1];

        Type? controllerType = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .FirstOrDefault(type =>
                type.IsClass &&
                !type.IsAbstract &&
                type.Namespace == "ORISS.HomeWork_5.Controllers" &&
                type.Name.Equals(
                    controllerName,
                    StringComparison.OrdinalIgnoreCase) &&
                type.GetCustomAttribute<ControllerAttribute>()
                    != null);

        if (controllerType == null)
        {
            await HandleNextAsync(context);
            return;
        }

        MethodInfo? method = controllerType
            .GetMethods(BindingFlags.Public |
                        BindingFlags.Instance |
                        BindingFlags.DeclaredOnly)
            .FirstOrDefault(m =>
                m.Name.Equals(
                    methodName,
                    StringComparison.OrdinalIgnoreCase) &&
                (
                    context.Request.HttpMethod == "GET" &&
                    m.GetCustomAttribute<GetAttribute>() != null
                    ||
                    context.Request.HttpMethod == "POST" &&
                    m.GetCustomAttribute<PostAttribute>() != null
                ));

        if (method == null)
        {
            await HandleNextAsync(context);
            return;
        }

        var values = new Dictionary<string, string>(
            StringComparer.OrdinalIgnoreCase);

        foreach (string? key in context.Request.QueryString.AllKeys)
        {
            if (key != null)
            {
                values[key] =
                    context.Request.QueryString[key] ?? "";
            }
        }

        if (context.Request.HttpMethod == "POST" &&
            context.Request.HasEntityBody)
        {
            if (context.Request.ContentLength64 < 0 ||
                context.Request.ContentLength64 > 8192)
            {
                await WriteResponse(
                    context, "400 Bad Request", 400);
                return;
            }

            if (context.Request.ContentType?.StartsWith(
                "application/x-www-form-urlencoded",
                StringComparison.OrdinalIgnoreCase) != true)
            {
                await WriteResponse(
                    context, "400 Bad Request", 400);
                return;
            }

            using var reader = new StreamReader(
                context.Request.InputStream,
                context.Request.ContentEncoding);

            string body = await reader.ReadToEndAsync();

            foreach (string pair in body.Split('&'))
            {
                string[] field = pair.Split('=', 2);

                if (field.Length == 2)
                {
                    string key = WebUtility.UrlDecode(field[0]);
                    string value = WebUtility.UrlDecode(field[1]);

                    values[key] = value;
                }
            }
        }

        ParameterInfo[] parameters = method.GetParameters();

        object?[] arguments = new object?[parameters.Length];

        try
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                string name = parameters[i].Name ?? "";

                if (!values.TryGetValue(name, out string? value) ||
                    string.IsNullOrWhiteSpace(value))
                {
                    await WriteResponse(
                        context,
                        "Отсутствует параметр: " + name,
                        400);

                    return;
                }

                arguments[i] = Convert.ChangeType(
                    value,
                    parameters[i].ParameterType,
                    CultureInfo.InvariantCulture);
            }
        }
        catch
        {
            await WriteResponse(
                context, "Некорректные параметры", 400);

            return;
        }

        object? controller =
            Activator.CreateInstance(controllerType);

        object? result =
            method.Invoke(controller, arguments);

        Console.WriteLine(
            controllerType.Name + "." + method.Name);

        await WriteResponse(
            context,
            result?.ToString() ?? "OK",
            200);
    }

    private static async Task WriteResponse(
        HttpListenerContext context,
        string text,
        int statusCode)
    {
        byte[] data = Encoding.UTF8.GetBytes(text);

        context.Response.StatusCode = statusCode;

        context.Response.ContentType =
            "text/plain; charset=utf-8";

        context.Response.ContentLength64 = data.Length;

        await context.Response.OutputStream.WriteAsync(data);
    }
}