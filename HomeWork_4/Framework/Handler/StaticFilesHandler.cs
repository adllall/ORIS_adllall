using System.Net;
using ORISS.HomeWork_4.Framework.Configuration;
using ORISS.HomeWork_4.Framework.Content;

namespace ORISS.HomeWork_4.Framework.Handler;

public class StaticFilesHandler : RequestHandler
{
    private readonly string staticFolder;
    private readonly string basePath;

    public StaticFilesHandler(ServerSettings settings)
    {
        staticFolder = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory,
            "HomeWork_4",
            settings.PublicDirectoryPath));

        basePath = "/" + settings.Path.Trim('/') + "/";
    }

    public override async Task HandleAsync(
        HttpListenerContext context)
    {
        string url = context.Request.Url?.AbsolutePath ?? "";

        if (context.Request.HttpMethod != "GET" ||
            !url.StartsWith(basePath))
        {
            await HandleNextAsync(context);
            return;
        }

        string relativePath = Uri.UnescapeDataString(
            url.Substring(basePath.Length));

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