using Centrifugo.Client.Json.Protocol.PushTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Centrifugo.Client.Json.Protocol.Results;

public class SubscribeResult
{
    [JsonProperty("expires")]
    public bool Expires { get; set; }
    
    [JsonProperty("ttl")]
    public uint Ttl { get; set; }
    
    [JsonProperty("recoverable")]
    public bool Recoverable { get; set; }
    
    #region v1
    
    [Obsolete("Old V1 protocol field")]
    [JsonProperty("seq")]
    public uint Seq { get; set; }
    
    [Obsolete("Old V1 protocol field")]
    [JsonProperty("gen")]
    public uint Gen { get; set; }
    
    #endregion
    
    [JsonProperty("epoch")]
    public string Epoch { get; set; }
    
    [JsonProperty("publications")]
    public List<Publication> Publications { get; set; }
    
    [JsonProperty("recovered")]
    public bool Recovered { get; set; }
    
    [JsonProperty("offset")]
    public ulong Offset { get; set; }
    
    [JsonProperty("positioned")]
    public bool Positioned { get; set; }
    
    [JsonProperty("data")]
    public JObject Data { get; set; }
    
    #region v2
    
    [JsonProperty("was_recovering")]
    public bool WasRecovering { get; set; }
    
    [JsonProperty("delta")]
    public bool Delta { get; set; }
    
    #endregion
}
