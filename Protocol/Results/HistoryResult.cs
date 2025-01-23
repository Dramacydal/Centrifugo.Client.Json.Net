using Centrifugo.Client.Json.Protocol.PushTypes;
using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol.Results;

public class HistoryResult
{
    [JsonProperty("publications")]
    public List<Publication> Publications { get; set; }
}