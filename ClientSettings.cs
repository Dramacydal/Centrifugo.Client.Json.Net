using Centrifugo.Client.Json.Enums;
using NLog;
using Websocket.Client;

namespace Centrifugo.Client.Json;

public class ClientSettings
{
    public IWebsocketClient WebsocketClient { get; set; }
    
    public PingMethod PingMethod { get; set; }

    public ProtocolVersion ProtocolVersion { get; set; } = ProtocolVersion.V1;
    
    public ILogger? Logger { get; set; }
}
