using Centrifugo.Client.Json.Protocol.Results;

namespace Centrifugo.Client.Json.Events;

public class UnsubscribedEvent
{
    public Subscription Subscription { get; }
    
    public UnsubscribeResult Data { get; }

    public UnsubscribedEvent(Subscription subscription, UnsubscribeResult unsubscribe)
    {
        Subscription = subscription;
        Data = unsubscribe;
    }
}
