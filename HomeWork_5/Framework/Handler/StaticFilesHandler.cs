using System.Net;
using ORISS.HomeWork_5.Framework.Configuration;
using ORISS.HomeWork_5.Framework.Content;

namespace ORISS.HomeWork_5.Framework.Handler;

public class StaticFilesHandler : RequestHandler
{
    private readonly string staticFolder;

    public StaticFilesHandler(ServerSettings settings)
    {
        staticFolder = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "HomeWork_5",
            settings.PublicDirectoryPath));
    }

    public override async Task HandleAsync(
        HttpListenerContext context)
    {
        if (context.Request.HttpMethod != "GET")
        {
            await HandleNextAsync(context);
            return;
        }

        string relativePath = Uri.UnescapeDataString(
                context.Request.Url?.AbsolutePath ?? "/")
            .TrimStart('/');

        if (relativePath == "" ||
            relativePath == "Rockstar" ||
            relativePath == "Rockstar/")
        {
            relativePath = "Rockstar/rockstar.html";
        }

        string filePath = Path.GetFullPath(
            Path.Combine(staticFolder, relativePath));

        string folderPrefix =
            Path.TrimEndingDirectorySeparator(staticFolder)
            + Path.DirectorySeparatorChar;

        if (!filePath.StartsWith(
                folderPrefix,
                StringComparison.OrdinalIgnoreCase)
            || !File.Exists(filePath))
        {
            await HandleNextAsync(context);
            return;
        }

        context.Response.StatusCode = 200;

        context.Response.ContentType =
            MimeTypeProvider.GetMimeType(filePath);

        using FileStream file = File.OpenRead(filePath);

        context.Response.ContentLength64 = file.Length;

        await file.CopyToAsync(
            context.Response.OutputStream);

        Console.WriteLine("200 OK: " + relativePath);
    }
}