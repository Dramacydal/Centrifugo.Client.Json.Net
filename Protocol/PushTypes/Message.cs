using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Centrifugo.Client.Json.Protocol.PushTypes;

public class Message : IWithSubData
{
    [JsonProperty("data")]
    public JObject? Data { get; set; }

    public T? As<T>()
    {
        return Data!.ToObject<T>();
    }
}