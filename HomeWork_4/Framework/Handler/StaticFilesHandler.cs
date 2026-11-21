
using System.Net;
using ORISS.HomeWork_4.Framework.Configuration;
using ORISS.HomeWork_4.Framework.Content;

namespace ORISS.HomeWork_4.Framework.Handlers;

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

    public override async Task HandleAsync(HttpListenerContext context)
    {
        string urlPath = context.Request.Url!.AbsolutePath;

        if (!urlPath.StartsWith(
                basePath,
                StringComparison.OrdinalIgnoreCase))
        {
            await HandleNextAsync(context);
            return;
        }

        string relativePath = Uri.UnescapeDataString(
            urlPath.Substring(basePath.Length));

        relativePath = relativePath.Replace('\\', '/');

        if (relativePath == "HtmlKnowledge/")
            relativePath += "Knowledge.html";
        else if (relativePath == "Rockstar/")
            relativePath += "rockstar.html";
        else if (relativePath == "SearchEnginer/")
            relativePath += "time.html";
        else if (relativePath == "")
            relativePath = "HtmlKnowledge/Knowledge.html";
        else if (relativePath.EndsWith("/"))
            relativePath += "index.html";

        string filePath = Path.GetFullPath(
            Path.Combine(staticFolder, relativePath));

        string folderPrefix =
            Path.TrimEndingDirectorySeparator(staticFolder)
            + Path.DirectorySeparatorChar;

        if (!filePath.StartsWith(
                folderPrefix,
                StringComparison.OrdinalIgnoreCase))
        {
            await HandleNextAsync(context);
            return;
        }

        if (!File.Exists(filePath))
        {
            await HandleNextAsync(context);
            return;
        }

        HttpListenerResponse response = context.Response;

        response.StatusCode = 200;
        response.ContentType = MimeTypeProvider.GetMimeType(filePath);
        response.Headers["Server"] = "MiniHttpServer/1.0";

        using FileStream file = File.OpenRead(filePath);

        response.ContentLength64 = file.Length;

        await file.CopyToAsync(response.OutputStream);

        Console.WriteLine("200 OK: " + relativePath);
    }
}
