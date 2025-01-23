using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol;

public class Error
{
    [JsonProperty("code")]
    public int Code { get; set; }
        
    [JsonProperty("message")]
    public string Message { get; set; }
    
    #region v2
    
    [JsonProperty("temporary")]
    public bool Temporary { get; set; }

    #endregion
}
