using System.Net;
using System.Text;

namespace LanAdmin;

public interface IGUI_responder
{
    public string Name { get; }
    public void ResponderAction(HttpListenerContext _context);
}

public class GUI_listener
{
    public static string UrlBase = "http://localhost:80/";
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
        Task.Run(() => //answer http requests on other thread
        {
            while (listener.IsListening)//task will end if listening stops
            {
                HttpListenerContext context = listener.GetContext();
                try
                {
                    if (context == null) return;//null check
                    byte[] respond = Encoding.UTF8.GetBytes(responderMethod(context));//genarate respont

                    //send it use stream
                    context.Response.ContentLength64 = respond.Length;
                    context.Response.OutputStream.Write(respond, 0, respond.Length);
                }
                catch { }
                if (context != null) context.Response.OutputStream.Close();//close stream after sending or error
            }
        });

    }
}

public static class GUI_helpers
{
    public static void SendHtmlRespond(HttpListenerContext _context, string _html)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(_html);

        _context.Response.ContentLength64 = buffer.Length;
        _context.Response.OutputStream.Write(buffer, 0, buffer.Length);
    }
    public static void CloseConnection(HttpListenerContext _context, string _html)
    {

    }
}
