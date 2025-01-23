namespace Centrifugo.Client.Json.Protocol.Enums;

public enum PushType
{
    Publication = 0,
    Join = 1,
    Leave = 2,
    Unsubscribe = 3,
    Message = 4,
    Subscribe = 5,
    Connect = 6,
    Disconnect = 7,
    Refresh = 8,
}
