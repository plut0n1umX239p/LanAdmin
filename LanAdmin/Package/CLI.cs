namespace LanAdmin;

/// <summary> every cli class must inhrait this in order to be functional </summary>
public interface ICLI_responder
{
    /// <summary> name of package gone use this </summary>
    public string Name { get; }
    public void ResponderAction(string[] _args);
}

public class CLI_listener
{
    public CLI_listener(ICLI_responder _responder)
    {
        responder = _responder;
    }

    private ICLI_responder responder;

    public Task? Check(string _commend, string[] _args)
    {
        if(responder.Name != _commend) return null;

        return Task.Run(() => { responder.ResponderAction(_args); });
    }
}

/// <summary> contine functions that helps in cli dev </summary>
public static class CLI_helpers
{

}
