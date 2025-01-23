using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol.PushTypes;

public class Refresh
{
    [JsonProperty("expires")]
    public bool Expires { get; set; }
    
    [JsonProperty("ttl")]
    public uint Ttl { get; set; }
}