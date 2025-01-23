using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Centrifugo.Client.Json.Protocol.PushTypes;

public class Publication
{
    #region v1
    
    [JsonProperty("seq")]
    [Obsolete("Old V1 protocol field")]
    public uint Seq { get; set; }
    
    [JsonProperty("gen")]
    [Obsolete("Old V1 protocol field")]
    public uint Gen { get; set; }
    
    [JsonProperty("uid")]
    [Obsolete("Old V1 protocol field")]
    public uint Uid { get; set; }
    
    #endregion
    
    [JsonProperty("data")]
    public JObject Data { get; set; }
    
    [JsonProperty("info")]
    public ClientInfo Info { get; set; }
    
    [JsonProperty("offset")]
    public ulong Offset { get; set; }

    #region v2
    
    [JsonProperty("tags")]
    public Dictionary<string, string> tags;
    
    [JsonProperty("delta")]
    public bool Delta { get; set; }
    
    [JsonProperty("time")]
    public long Time { get; set; }
    
    [JsonProperty("channel")]
    public string Channel { get; set; }
    
    #endregion
}
