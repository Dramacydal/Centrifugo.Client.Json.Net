using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol.PushTypes;

public class Join
{
    [JsonProperty("info")]
    public ClientInfo Info { get; set; }
}