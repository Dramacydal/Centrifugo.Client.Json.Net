using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Centrifugo.Client.Json.Protocol.PushTypes;

public class Connect
{
    [JsonProperty("client")]
    public string Client { get; set; }

    [JsonProperty("client")]
    public string Version { get; set; }

    [JsonProperty("data")]
    public JObject Data { get; set; }

    #region v2

    [JsonProperty("ping")]
    public uint Ping { get; set; }

    [JsonProperty("pong")]
    public bool Pong { get; set; }
    
    [JsonProperty("session")]
    public string Session { get; set; }
    
    [JsonProperty("node")]
    public string Node { get; set; }
    
    [JsonProperty("time")]
    public long Time { get; set; }
    
    #endregion
}
