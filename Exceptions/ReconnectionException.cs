namespace Centrifugo.Client.Json.Exceptions;

public class ReconnectionException(string message) : CentrifugoException(message)
{
}