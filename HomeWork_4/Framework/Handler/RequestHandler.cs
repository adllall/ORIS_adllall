using System.Net;

namespace ORISS.HomeWork_4.Framework.Handlers;

public abstract class RequestHandler
{
    protected RequestHandler? Next { get; private set; }

    public void SetNext(RequestHandler next)
    {
        Next = next;
    }

    protected async Task HandleNextAsync(HttpListenerContext context)
    {
        if (Next != null)
            await Next.HandleAsync(context);
    }

    public abstract Task HandleAsync(HttpListenerContext context);
}