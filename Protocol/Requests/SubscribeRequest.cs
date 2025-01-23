using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Centrifugo.Client.Json.Protocol.Requests;

public class SubscribeRequest
{
    [JsonProperty("channel")]
    public string Channel { get; set; }
    
    [JsonProperty("token")]
    public string? Token { get; set; }
    
    [JsonProperty("recover")]
    public bool Recover { get; set; }
    
    #region v1
    
    [JsonProperty("seq")]
    [Obsolete("Old V1 protocol field")]
    public uint Seq { get; set; }
    
    [JsonProperty("gen")]
    [Obsolete("Old V1 protocol field")]
    public uint Gen { get; set; }
    
    #endregion
    
    [JsonProperty("epoch")]
    public string? Epoch { get; set; }
    
    [JsonProperty("offset")]
    public long? Offset { get; set; }
    
    [JsonProperty("data")]
    public JObject Data { get; set; }
    
    #region v2
    
    [JsonProperty("positioned")]
    public bool Positioned { get; set; }
    
    [JsonProperty("recoverable")]
    public bool Recoverabble { get; set; }
    
    [JsonProperty("join_leave")]
    public bool JoinLeave { get; set; }
    
    [JsonProperty("delta")]
    public bool Delta { get; set; }
    
    #endregion
}
