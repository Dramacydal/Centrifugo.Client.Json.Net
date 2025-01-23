using System.Reactive.Subjects;
using Centrifugo.Client.Json.Enums;
using Centrifugo.Client.Json.Events;
using Centrifugo.Client.Json.Helpers;
using Centrifugo.Client.Json.Protocol.PushTypes;

namespace Centrifugo.Client.Json;

public sealed class Subscription : IDisposable
{
    public string Channel { get; }
    
    public long Offset { get; internal set; }

    public string? Epoch { get; internal set; }

    public SubscriptionState State { get; internal set; }

    #region Events
    
    internal Subject<PushEvent<Publication>>? PublicationPushEventSource { get; private set; }
    public void OnPublicationPush(Func<PushEvent<Publication>, Task> e)
    {
        PublicationPushEventSource ??= new Subject<PushEvent<Publication>>();
        PublicationPushEventSource.SubscribeAsync(e);
    }

    internal Subject<PushEvent<Join>>? JoinPushEventSource { get; private set; }
    public void OnJoinPush(Func<PushEvent<Join>, Task> e)
    {
        JoinPushEventSource ??= new Subject<PushEvent<Join>>();
        JoinPushEventSource.SubscribeAsync(e);
    }

    internal Subject<PushEvent<Leave>>? LeavePushEventSource { get; private set; }
    public void OnLeavePush(Func<PushEvent<Leave>, Task> e)
    {
        LeavePushEventSource ??= new Subject<PushEvent<Leave>>();
        LeavePushEventSource.SubscribeAsync(e);
    }

    internal Subject<PushEvent<Unsubscribe>>? UnsubscribePushEventSource { get; private set; }
    public void OnUnsubscribePush(Func<PushEvent<Unsubscribe>, Task> e)
    {
        UnsubscribePushEventSource ??= new Subject<PushEvent<Unsubscribe>>();
        UnsubscribePushEventSource.SubscribeAsync(e);
    }
    
    internal Subject<PushEvent<Message>>? MessagePushEventSource { get; private set; }
    public void OnMessagePush(Func<PushEvent<Message>, Task> e)
    {
        MessagePushEventSource ??= new Subject<PushEvent<Message>>();
        MessagePushEventSource.SubscribeAsync(e);
    }
    
    internal Subject<PushEvent<Subscribe>>? SubscribePushEventSource { get; private set; }
    public void OnSubscribePush(Func<PushEvent<Subscribe>, Task> e)
    {
        SubscribePushEventSource ??= new Subject<PushEvent<Subscribe>>();
        SubscribePushEventSource.SubscribeAsync(e);
    }

    internal Subject<PushEvent<Connect>>? ConnectPushEventSource { get; private set; }
    public void OnConnectPush(Func<PushEvent<Connect>, Task> e)
    {
        ConnectPushEventSource ??= new Subject<PushEvent<Connect>>();
        ConnectPushEventSource.SubscribeAsync(e);
    }
    
    internal Subject<PushEvent<Disconnect>>? DisconnectPushEventSource { get; private set; }
    public void OnDisconnectPush(Func<PushEvent<Disconnect>, Task> e)
    {
        DisconnectPushEventSource ??= new Subject<PushEvent<Disconnect>>();
        DisconnectPushEventSource.SubscribeAsync(e);
    }
    
    internal Subject<PushEvent<Refresh>>? RefreshPushEventSource { get; private set; }
    public void OnRefreshPush(Func<PushEvent<Refresh>, Task> e)
    {
        RefreshPushEventSource ??= new Subject<PushEvent<Refresh>>();
        RefreshPushEventSource.SubscribeAsync(e);
    }
    
    #endregion Events

    public Subscription(string channel)
    {
        if (string.IsNullOrWhiteSpace(channel) || channel.Length > 255)
        {
            throw new ArgumentException("Channel name must be non empty string, and length not longer that 255.");
        }

        Channel = channel;
        State = SubscriptionState.New;
    }

    #region Disposable support

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        PublicationPushEventSource?.OnCompleted();
        PublicationPushEventSource?.Dispose();
        
        JoinPushEventSource?.OnCompleted();
        JoinPushEventSource?.Dispose();
        
        LeavePushEventSource?.OnCompleted();
        LeavePushEventSource?.Dispose();
        
        UnsubscribePushEventSource?.OnCompleted();
        UnsubscribePushEventSource?.Dispose();
        
        MessagePushEventSource?.OnCompleted();
        MessagePushEventSource?.Dispose();
        
        SubscribePushEventSource?.OnCompleted();
        SubscribePushEventSource?.Dispose();
        
        ConnectPushEventSource?.OnCompleted();
        ConnectPushEventSource?.Dispose();
        
        DisconnectPushEventSource?.OnCompleted();
        DisconnectPushEventSource?.Dispose();
        
        RefreshPushEventSource?.OnCompleted();
        RefreshPushEventSource?.Dispose();
    }

    #endregion Disposable support
}