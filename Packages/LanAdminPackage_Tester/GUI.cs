using LanAdmin;
using System.Net;
namespace LanAdminPackage.Tester;

public class GUI : IGUI_responder
{
    public string Name => "template";

    public void ResponderAction(HttpListenerContext _context)
    {
        GUI_helpers.SendHtmlRespond(_context, "<p>Hello World!</p>");
    }
}
