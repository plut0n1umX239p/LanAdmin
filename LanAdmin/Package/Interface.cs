using System.Net;

namespace LanAdmin;

public interface IPackage
{
    public string Name { get; }

    IGUI_responder GUI { get; }
    ICLI_responder ClI { get; }
}
