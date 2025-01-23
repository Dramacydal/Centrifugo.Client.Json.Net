using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol.Results;

public class PresenceResult
{
    [JsonProperty("presence")]
    public Dictionary<string, ClientInfo> Presence { get; set; }
}