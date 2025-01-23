using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol;

public class ClientInfo
{
    [JsonProperty("user")]
    public string User { get; set; }
    
    [JsonProperty("client")]
    public string Client { get; set; }
    
    [JsonProperty("conn_info")]
    public string ConnInfo { get; set; }
    
    [JsonProperty("chan_info")]
    public string ChanInfo { get; set; }
}
