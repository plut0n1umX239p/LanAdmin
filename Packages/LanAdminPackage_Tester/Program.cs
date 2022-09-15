using LanAdmin;
using System.Net;

namespace LanAdminPackage.Tester;

public class Tester : IPackage
{
    public string Name => "template";

    public IGUI_responder GUI => new GUI();
    public ICLI_responder CLI => new CLI();
}
