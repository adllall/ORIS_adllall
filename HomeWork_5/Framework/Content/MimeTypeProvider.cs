namespace ORISS.HomeWork_5.Framework.Content;

public static class MimeTypeProvider
{
    public static string GetMimeType(string filePath)
    {
        string extension = Path.GetExtension(filePath)
            .ToLowerInvariant();

        return extension switch
        {
            ".html" => "text/html; charset=utf-8",
            ".css" => "text/css; charset=utf-8",
            ".js" => "text/javascript; charset=utf-8",
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".svg" => "image/svg+xml",
            ".ico" => "image/x-icon",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };
    }
}