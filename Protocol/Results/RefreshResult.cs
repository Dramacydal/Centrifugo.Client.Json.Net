using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol.Results;

public class RefreshResult
{
    [JsonProperty("client")]
    public string Client { get; set; }

    [JsonProperty("version")]
    public string Version { get; set; }

    [JsonProperty("expires")]
    public bool Expires { get; set; }

    [JsonProperty("ttl")]
    public uint Ttl { get; set; }
}