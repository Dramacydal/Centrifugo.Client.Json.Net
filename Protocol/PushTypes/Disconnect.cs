using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol.PushTypes;

public class Disconnect
{
    [JsonProperty("code")]
    public uint Code { get; set; }
    
    [JsonProperty("reason")]
    public string Reason { get; set; }
    
    [JsonProperty("reconnect")]
    public bool Reconnect { get; set; }
}
