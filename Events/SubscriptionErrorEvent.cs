﻿namespace Centrifugo.Client.Json.Events;

public class SubscriptionErrorEvent
{
    public string Channel { get; }

    public Exception? Exception { get; }

    public SubscriptionErrorEvent(string channel, Exception? exception = null)
    {
        Channel = channel;
        Exception = exception;
    }
}