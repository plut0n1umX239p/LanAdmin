using System.Net;

namespace LanAdmin;

public interface IPackage
{
    public string Name { get; }

    IGUI_responder GUI { get; }
    void CLI_responder(string[] _args);
}
