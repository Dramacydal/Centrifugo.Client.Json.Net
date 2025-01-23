using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol.Requests;

public class PresenceStatsRequest
{
    [JsonProperty("channel")]
    public string Channel { get; set; }
}