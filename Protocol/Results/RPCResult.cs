using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol.Results;

public class RPCResult
{
    [JsonProperty("data")]
    public JsonObject Data { get; set; }
}