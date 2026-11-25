namespace ORISS.HomeWork_4.Framework.Content;

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
            ".webp" => "image/webp",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };
    }
}