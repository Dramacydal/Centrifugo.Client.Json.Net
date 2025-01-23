using Centrifugo.Client.Json.Enums;
using Centrifugo.Client.Json.Protocol.Enums;
using Centrifugo.Client.Json.Protocol.PushTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Centrifugo.Client.Json.Protocol;

public class Push : IWithSubData
{
    [JsonProperty("channel")]
    public string Channel { get; set; }
    
    #region v1
    
    [JsonProperty("type")]
    public PushType Type { get; set; }

    [JsonProperty("data")]
    public JObject? Data { get; set; }
    
    #endregion
   
    #region  V2

    [JsonProperty("pub")]
    public Publication? Pub { get; set; }
    
    [JsonProperty("join")]
    public Join? Join  { get; set; }
    
    [JsonProperty("leave")]
    public Leave? Leave  { get; set; }
    
    [JsonProperty("unsubscribe")]
    public Unsubscribe? Unsubscribe  { get; set; }
    
    [JsonProperty("message")]
    public Message? Message  { get; set; }
    
    [JsonProperty("subscribe")]
    public Subscribe? Subscribe  { get; set; }
    
    [JsonProperty("connect")]
    public Connect? Connect  { get; set; }
    
    [JsonProperty("disconnect")]
    public Disconnect? Disconnect  { get; set; }
    
    [JsonProperty("refresh")]
    public Refresh? Refresh  { get; set; }
    
    #endregion
    
    public bool HasInlineData => 
        Pub != null || Join != null || Leave != null || Unsubscribe != null || Message != null
        || Subscribe != null || Connect != null || Disconnect != null || Refresh != null;
    
    public ProtocolVersion GuessVersion() => HasInlineData ? ProtocolVersion.V2 : ProtocolVersion.V1;
    
    public T? As<T>()
    {
        return Data.ToObject<T>();
    }
}