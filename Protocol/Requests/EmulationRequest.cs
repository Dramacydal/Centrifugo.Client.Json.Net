using Centrifugo.Client.Json.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Centrifugo.Client.Json.Protocol.Requests;

[ProtocolVersion(ProtocolVersion.V2)]
public class EmulationRequest
{
    [JsonProperty("node")]
    public string Node { get; set; }

    [JsonProperty("session")]
    public string Session { get; set; }

    [JsonProperty("data")]
    public JObject Data { get; set; }
}
