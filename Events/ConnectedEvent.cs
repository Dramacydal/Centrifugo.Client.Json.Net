using Centrifugo.Client.Json.Protocol.Results;

namespace Centrifugo.Client.Json.Events;

public class ConnectedEvent
{
    public ConnectResult Data { get; set; }
}
