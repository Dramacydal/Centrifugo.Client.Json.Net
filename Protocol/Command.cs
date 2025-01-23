using Centrifugo.Client.Json.Enums;
using Centrifugo.Client.Json.Protocol.Enums;
using Centrifugo.Client.Json.Protocol.Requests;
using Newtonsoft.Json;

namespace Centrifugo.Client.Json.Protocol;

public class Command<T, V>
{
    [JsonProperty("id")]
    public int Id { get; set; }

    #region v1

    [JsonProperty("method")]
    public MethodType Method { get; set; }

    [JsonProperty("params")]
    public T? Params { get; set; }

    #endregion

    #region v2

    [JsonProperty("connect")]
    public ConnectRequest? Connect { get; set; }

    [JsonProperty("subscribe")]
    public SubscribeRequest? Subscribe { get; set; }

    [JsonProperty("unsubscribe")]
    public UnsubscribeRequest? Unsubscribe { get; set; }

    [JsonProperty("publish")]
    public PublishRequest? Publish { get; set; }

    [JsonProperty("presence")]
    public PresenceRequest? Presence { get; set; }

    [JsonProperty("presence_stats")]
    public PresenceStatsRequest? PresenceStats { get; set; }

    [JsonProperty("history")]
    public HistoryRequest? History { get; set; }

    [JsonProperty("ping")]
    public PingRequest? Ping { get; set; }

    [JsonProperty("send")]
    public SendRequest? Send { get; set; }

    [JsonProperty("rpc")]
    public RPCRequest? Rpc { get; set; }

    [JsonProperty("refresh")]
    public RefreshRequest? Refresh { get; set; }

    [JsonProperty("sub_refresh")]
    public SubRefreshRequest? SubRefresh { get; set; }

    #endregion

    public bool HasInlineRequest =>
        Connect != null || Subscribe != null || Unsubscribe != null || Publish != null || Presence != null
        || PresenceStats != null || History != null || Ping != null || Send != null || Rpc != null
        || Refresh != null || SubRefresh != null;

    public ProtocolVersion GuessVersion() => HasInlineRequest ? ProtocolVersion.V2 : ProtocolVersion.V1;

    internal async Task<V> Handle(CentrifugoClient client, CancellationToken cancellationToken)
    {
        return await client.HandleCommand(this, cancellationToken);
    }
}