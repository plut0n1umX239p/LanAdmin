using LanAdmin;
using System.Net;
namespace LanAdminPackage.Tester;

public class GUI : IGUI_responder
{
    public string Name => "template";

    public void ResponderAction(HttpListenerContext _context)
    {
        GUI_helpers.SendHtmlRespond(_context, 
            $"<!DOCTYPE html>" +
            $"<html lang=\"en-US\">" +

            $"<head>" +
                $"<meta charset=\"utf-8\">" +
                $"<title>{Name}/GUI</title>" +
                $"{GUI_helpers.BootStrapInternal()}" +
            $"</head>" +

            $"<body>" +
                $"<div class=\"container p-5 my-5 bg-primary text-white\">" +
                    $"<h1 class=\"text-center\">Hello World!</h1>" +
                $"</div>" +
            $"</body>" +

            $"</html>");
    }
}
