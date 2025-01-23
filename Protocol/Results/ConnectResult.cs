using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol.Results;

public class ConnectResult
{
    [JsonProperty("client")]
    public string Client { get; set; }
    
    [JsonProperty("version")]
    public string Version { get; set; }

    [JsonProperty("expires")]
    public bool Expires { get; set; }

    [JsonProperty("ttl")]
    public uint Ttl { get; set; }
    
    #region v2
    
    [JsonProperty("ping")]
    public uint Ping { get; set; }
    
    [JsonProperty("pong")]
    public bool Pong { get; set; }
    
    [JsonProperty("session")]
    public string Session { get; set; }
    
    [JsonProperty("node")]
    public string Node { get; set; }
    
    [JsonProperty("time")]
    public long Time { get; set; }
    
    #endregion
}