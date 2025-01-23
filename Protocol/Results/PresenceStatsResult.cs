using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol.Results;

public class PresenceStatsResult
{
    [JsonProperty("num_clients")]
    public uint NumClients { get; set; }
    
    [JsonProperty("num_users")]
    public uint NumUsers { get; set; }
}