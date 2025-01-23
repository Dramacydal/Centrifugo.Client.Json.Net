using Centrifugo.Client.Json.Protocol.PushTypes;

namespace Centrifugo.Client.Json.Events;

public class MessageEvent
{
    public Message Data { get; set; }
}