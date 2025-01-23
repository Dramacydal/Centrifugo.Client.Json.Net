using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol.Results;

public class SubRefreshResult
{
    [JsonProperty("expires")]
    public bool Expires { get; set; }

    [JsonProperty("ttl")]
    public uint Ttl { get; set; }
}