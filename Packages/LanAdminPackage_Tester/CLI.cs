using LanAdmin;
using System.Net;
namespace LanAdminPackage.Tester;

public class CLI : ICLI_responder
{
    public string Name => "template";

    public void ResponderAction(string[] _args)
    {
        Console.WriteLine("Hello World!");
    }
}
