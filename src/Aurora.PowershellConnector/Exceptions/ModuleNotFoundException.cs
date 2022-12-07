namespace Aurora.PowershellConnector.Exceptions;

public sealed class ModuleNotFoundException : Exception
{
    public ModuleNotFoundException(string message)
        : base(message) { }
}
