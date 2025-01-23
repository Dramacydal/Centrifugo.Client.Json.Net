using Centrifugo.Client.Json.Enums;
using Centrifugo.Client.Json.Protocol.Results;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Centrifugo.Client.Json.Protocol;

public class Reply : IWithSubData
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("error")]
    public Error? Error { get; set; }

    #region v1

    [JsonProperty("result")]
    public JObject? Result { get; set; }    

    #endregion
    
    #region V2
    
    [JsonProperty("push")]
    public Push? Push { get; set; }
    
    [JsonProperty("connect")]
    public ConnectResult? Connect { get; set; }
    
    [JsonProperty("subscribe")]
    public SubscribeResult? Subscribe { get; set; }
    
    [JsonProperty("unsubscribe")]
    public UnsubscribeResult? Unsubscribe { get; set; }
    
    [JsonProperty("publish")]
    public PublishResult? Publish { get; set; }
    
    [JsonProperty("presence")]
    public PresenceResult? Presence { get; set; }
    
    [JsonProperty("presence_stats")]
    public PresenceStatsResult? PresenceStats { get; set; }
    
    [JsonProperty("history")]
    public HistoryResult? History { get; set; }
    
    [JsonProperty("ping")]
    public PingResult? Ping { get; set; }
    
    [JsonProperty("rpc")]
    public RPCResult? Rpc { get; set; }
    
    [JsonProperty("refresh")]
    public RefreshResult? Refresh { get; set; }
    
    [JsonProperty("sub_refresh")]
    public SubRefreshResult? SubRefresh { get; set; }

    public bool HasInlineData =>
        Push != null || Connect != null || Subscribe != null || Unsubscribe != null
        || Publish != null || Presence != null || PresenceStats != null
        || History != null || Ping != null || Rpc != null || Refresh != null || SubRefresh != null;

    public ProtocolVersion GuessVersion() => HasInlineData ? ProtocolVersion.V2 : ProtocolVersion.V1;
    
    #endregion
    
    public T? As<T>()
    {
        return Result.ToObject<T>();
    }
}