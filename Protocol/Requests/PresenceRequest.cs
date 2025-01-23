using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol.Requests;

public class PresenceRequest
{
    [JsonProperty("channel")]
    public string Channel { get; set; }
}