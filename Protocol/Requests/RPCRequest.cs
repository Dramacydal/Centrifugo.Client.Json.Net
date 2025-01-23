using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Centrifugo.Client.Json.Protocol.Requests;

public class RPCRequest
{
    [JsonProperty("data")]
    public JObject Data { get; set; }
    
    [JsonProperty("method")]
    public string Method { get; set; }
}