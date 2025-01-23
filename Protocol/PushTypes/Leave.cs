using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol.PushTypes;

public class Leave
{
    [JsonProperty("info")]
    public ClientInfo Info { get; set; }
}