using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol.Requests;

public class SubRefreshRequest
{
    [JsonProperty("channel")]
    public string Channel { get; set; }

    [JsonProperty("token")]
    public string Token { get; set; }
}