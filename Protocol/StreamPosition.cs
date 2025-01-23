using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol;

public class StreamPosition
{
    [JsonProperty("offset")]
    public ulong Offset { get; set; }

    [JsonProperty("epoch")]
    public string Epoch { get; set; }
}
