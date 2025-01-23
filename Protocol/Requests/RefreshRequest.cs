using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol.Requests;

public class RefreshRequest
{
    [JsonProperty("token")]
    public string Token { get; set; }
}