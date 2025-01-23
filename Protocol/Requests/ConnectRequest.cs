using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Centrifugo.Client.Json.Protocol.Requests;

public class ConnectRequest
{
    [JsonProperty("token")]
    public string Token { get; set; }
    
    [JsonProperty("data")]
    public JObject Data { get; set; }
    
    [JsonProperty("subs")]
    public List<SubscribeRequest> Subs { get; set; }
    
    [JsonProperty("name")]
    public string Name { get; set; }
    
    [JsonProperty("version")]
    public string Version { get; set; }
    
    #region v2
    
    [JsonProperty("headers")]
    public Dictionary<string, string> Headers { get; set; }
    
    #endregion
}
