using System.Net;
using System.Text;
namespace LanAdmin;

/// <summary> every gui class must inhrait this in order to be functional </summary>
public interface IGUI_responder
{
    /// <summary> name of package gone use this </summary>
    public string Name { get; }
    public void ResponderAction(HttpListenerContext _context);
}

/// <summary> a simple web server to listen to gui http calls </summary>
public class GUI_listener
{
    /// <summary> if this changed all GUI_listener must reset to make effect </summary>
    public static string UrlBase = "http://localhost:8080/";
    public GUI_listener(IGUI_responder _responder)
    {
        if (!HttpListener.IsSupported)
            throw new Exception("\"WindowsXP SP2\" or \"Server 2003\" is required to use the GUI.");

        listener.Prefixes.Add(UrlBase + _responder.Name + '/');
        responder = _responder;
    }

    private HttpListener listener = new();
    private IGUI_responder responder;

    public void Start()
    {
        listener.Start();
        Task.Run(() =>
        {
            while (listener.IsListening)
            {
                HttpListenerContext context = listener.GetContext();
                if(context == null) continue;

                responder.ResponderAction(context);
                context.Response.OutputStream.Close();
            }
        });
    }
    public void Stop()
    {
        listener.Stop();
    }
}

/// <summary> contine functions that helps in gui dev </summary>
public static class GUI_helpers
{
    public static void SendHtmlRespond(HttpListenerContext _context, string _html)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(_html);

        _context.Response.ContentLength64 = buffer.Length;
        _context.Response.OutputStream.Write(buffer, 0, buffer.Length);
    }
}
