using Centrifugo.Client.Json.Protocol;

namespace Centrifugo.Client.Json.Events;

public class ErrorEvent
{
    public Subscription Subscription { get; set; }

    public Exception Exception { get; set; }

    public Error Data { get; set; }
}
