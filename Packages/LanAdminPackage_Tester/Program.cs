using LanAdmin;
using System.Net;

namespace LanAdminPackage.Tester;

public class Tester : IPackage
{
    public string Name => "Tester";

    public IGUI_responder GUI => throw new NotImplementedException();

    public void CLI_responder(string[] _args)
    {

    }
}