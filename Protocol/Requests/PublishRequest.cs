using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Centrifugo.Client.Json.Protocol.Requests;

public class PublishRequest
{
    [JsonProperty("channel")]
    public string Channel { get; set; }
    
    [JsonProperty("data")]
    public JObject Data { get; set; }
}