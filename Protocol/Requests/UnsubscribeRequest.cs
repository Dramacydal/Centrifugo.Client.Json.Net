using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol.Requests;

public class UnsubscribeRequest
{
    [JsonProperty("channel")]
    public string Channel { get; set; }
}