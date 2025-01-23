using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Centrifugo.Client.Json.Protocol.Requests;

public class SendRequest
{
    [JsonProperty("data")]
    public JObject? Data { get; set; }
}