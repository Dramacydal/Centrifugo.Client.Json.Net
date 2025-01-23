using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol.Requests;

public class HistoryRequest
{
    [JsonProperty("channel")]
    public string Channel { get; set; } = string.Empty;

    #region v1
    
    [Obsolete("Old V1 protocol field")]
    [JsonProperty("use_since")]
    public bool UseSince { get; set; }
    
    [Obsolete("Old V1 protocol field")]
    [JsonProperty("offset")]
    public ulong Offset { get; set; }
    
    [Obsolete("Old V1 protocol field")]
    [JsonProperty("epoch")]
    public string Epoch { get; set; }
    
    [Obsolete("Old V1 protocol field")]
    [JsonProperty("use_limit")]
    public bool UseLimit { get; set; }
    
    #endregion
    
    [JsonProperty("limit")]
    public int Limit { get; set; }
    
    [JsonProperty("since")]
    public StreamPosition Since { get; set; }
    
    [JsonProperty("reverse")]
    public bool Reverse { get; set; }
}