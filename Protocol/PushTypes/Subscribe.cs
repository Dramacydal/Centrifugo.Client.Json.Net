using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Centrifugo.Client.Json.Protocol.PushTypes;

public class Subscribe
{
    [JsonProperty("recoverable")]
    public bool Recoverable { get; set; }

    #region v1
    
    [JsonProperty("seq")]
    [Obsolete("Old V1 protocol field")]
    public uint Seq { get; set; }
    
    [JsonProperty("gen")]
    [Obsolete("Old V1 protocol field")]
    public uint Gen { get; set; }
    
    #endregion
    
    [JsonProperty("epoch")]
    public string Epoch { get; set; }

    [JsonProperty("offset")]
    public ulong Offset { get; set; }

    [JsonProperty("positioned")]
    public bool Positioned { get; set; }

    [JsonProperty("data")]
    public JObject Data { get; set; }
}
