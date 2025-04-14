namespace Aurora.PowershellConnector.Exceptions;

public sealed class ModuleNotFoundException(string message) 
    : Exception(message);