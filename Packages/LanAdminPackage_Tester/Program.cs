using LanAdmin;
using System.Net;

namespace LanAdminPackage.Tester;

public class Tester : IPackage
{
    public string Name => "template";

    public IGUI_responder GUI => throw new NotImplementedException();
    public ICLI_responder ClI => throw new NotImplementedException();
}