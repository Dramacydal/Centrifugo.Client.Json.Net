using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol.PushTypes;

public class Unsubscribe
{
    #region v1
    
    [Obsolete("Very old V1 protocol")]
    [JsonProperty("resubscribe")]
    public bool Resubscribe { get; set; }
    
    #endregion
    
    #region v2
    
    [JsonProperty("code")]
    public uint Code { get; set; }
    
    [JsonProperty("reason")]
    public uint Reason { get; set; }
    
    #endregion
}