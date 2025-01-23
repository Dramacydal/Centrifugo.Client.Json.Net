using Centrifugo.Client.Json.Protocol.Results;

namespace Centrifugo.Client.Json.Events;

public class SubscribedEvent
{
    public Subscription Subscription { get; }
    
    public SubscribeResult Data { get; }

    public SubscribedEvent(Subscription subscription, SubscribeResult subscribe)
    {
        Subscription = subscription;
        Data = subscribe;
    }
}