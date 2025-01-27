using Websocket.Client;

namespace Centrifugo.Client.Json.Events;

public class DisconnectedEvent
{
    public DisconnectionInfo Info { get; set; }
}
