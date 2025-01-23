namespace Centrifugo.Client.Json.Protocol.Enums;

public enum MethodType
{
    Connect = 0,
    Subscribe = 1,
    Unsubscribe = 2,
    Publish = 3,
    Presence = 4,
    PresenceStats = 5,
    History = 6,
    Ping = 7,
    Send = 8,
    Rpc = 9,
    Refresh = 10,
    SubRefresh = 11,
}
