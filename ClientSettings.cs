using Centrifugo.Client.Json.Enums;
using Websocket.Client;

namespace Centrifugo.Client.Json;

public class ClientSettings
{
    public IWebsocketClient WebsocketClient { get; set; }
    
    public PingMethod PingMethod { get; set; }
    
    public ProtocolVersion ProtocolVersion { get; set; }
}
