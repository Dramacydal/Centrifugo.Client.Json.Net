namespace Centrifugo.Client.Json.Enums;

public enum SubscriptionState
{
    New = 0,
    Subscribing = 1,
    Subscribed = 2,
    SubscriptionError = 3,
    Unsubscribed = 4,
    UnsubscriptionError = 5,
}
