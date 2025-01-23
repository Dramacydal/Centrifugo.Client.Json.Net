using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Reactive.Subjects;
using Centrifugo.Client.Json.Enums;
using Centrifugo.Client.Json.Events;
using Centrifugo.Client.Json.Exceptions;
using Centrifugo.Client.Json.Helpers;
using Centrifugo.Client.Json.Protocol;
using Centrifugo.Client.Json.Protocol.Enums;
using Centrifugo.Client.Json.Protocol.PushTypes;
using Centrifugo.Client.Json.Protocol.Requests;
using Centrifugo.Client.Json.Protocol.Results;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace Centrifugo.Client.Json;

public class CentrifugoClient
{
    private readonly IWebsocketClient? _ws;

    private string _token;

    private string _name;

    private bool _authorized;
    
    private int _nextOperationId = 0;

    private readonly ConcurrentDictionary<int, TaskCompletionSource<JObject>> _operations = new();

    private readonly ConcurrentDictionary<string, Subscription> _channels = new();

    private Subject<ConnectedEvent>? _connectedEventSource;

    private Subject<MessageEvent>? _messageEventsSource;

    private Subject<ErrorEvent>? _errorEventsSource;

    private Subject<PingEvent>? _pingEventsSource;

    private Subject<SubscribedEvent>? _subscribedEventsSource;
    
    private Subject<UnsubscribedEvent>? _unsubscribedEventsSource;
    
    public CentrifugoClient(ClientSettings settings)
    {
        _ws = settings.WebsocketClient;
        _pingMethod = settings.PingMethod;
        _protocolVersion = settings.ProtocolVersion;
        // _ws.IsTextMessageConversionEnabled = false;
        
        HandleConnection();
        HandleSocketMessages();
    }
    
    #region Events

    // OnRefresh.. when need to refresh token.
    // OnMessage

    public void OnError(Func<ErrorEvent, Task> handler)
    {
        _errorEventsSource ??= new();
        _errorEventsSource.SubscribeAsync(handler);
    }

    public void OnError(Action<ErrorEvent> handler)
    {
        _errorEventsSource ??= new();
        _errorEventsSource.Subscribe(handler);
    }

    public void OnMessage(Func<MessageEvent, Task> handler)
    {
        _messageEventsSource ??= new();
        _messageEventsSource.SubscribeAsync(handler);
    }

    public void OnMessage(Action<MessageEvent> handler)
    {
        _messageEventsSource ??= new();
        _messageEventsSource.Subscribe(handler);
    }
    
    public void OnConnect(Func<ConnectedEvent, Task> handler)
    {
        _connectedEventSource ??= new();
        _connectedEventSource.SubscribeAsync(handler);
    }

    public void OnConnect(Action<ConnectedEvent> handler)
    {
        _connectedEventSource ??= new();
        _connectedEventSource.Subscribe(handler);
    }
    
    public void OnPing(Func<PingEvent, Task> handler)
    {
        _pingEventsSource ??= new();
        _pingEventsSource.SubscribeAsync(handler);
    }

    public void OnPing(Action<PingEvent> handler)
    {
        _pingEventsSource ??= new();
        _pingEventsSource.Subscribe(handler);
    }
    
    public void OnSubscribe(Func<SubscribedEvent, Task> handler)
    {
        _subscribedEventsSource ??= new();
        _subscribedEventsSource.SubscribeAsync(handler);
    }

    public void OnSubscribe(Action<SubscribedEvent> handler)
    {
        _subscribedEventsSource ??= new();
        _subscribedEventsSource.Subscribe(handler);
    }
    
    public void OnUnsubscribe(Func<UnsubscribedEvent, Task> handler)
    {
        _unsubscribedEventsSource ??= new();
        _unsubscribedEventsSource.SubscribeAsync(handler);
    }

    public void OnUnsubscribe(Action<UnsubscribedEvent> handler)
    {
        _unsubscribedEventsSource ??= new();
        _unsubscribedEventsSource.Subscribe(handler);
    }

    #endregion Events

    private void HandleConnection()
    {
        // reconnect -> auth and resub: _channels
        _ws.ReconnectionHappened.SubscribeAsync(r =>
        {
            //_clientId = null;

            switch (r.Type)
            {
                case ReconnectionType.Initial:
                    break;
                case ReconnectionType.Lost:
                    break;
                case ReconnectionType.NoMessageReceived:
                    // ping pong нужен
                    break;
                case ReconnectionType.Error:
                    break;
                case ReconnectionType.ByUser:
                    break;
                case ReconnectionType.ByServer:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return Task.CompletedTask;
        });
    }

    private List<string> ExtractParts(string message)
    {
        message = message.Trim();
        
        List<string> parts = new();
        if (string.IsNullOrEmpty(message))
            return parts;

        var start = -1;
        var counter = 0;
        char startChar ='\0',
            endChar = '\0';

        for (var i = 0; i < message.Length; ++i)
        {
            if (start == -1)
            {
                start = i;
                startChar = message[i];
                switch (startChar)
                {
                    case '{':
                        endChar = '}';
                        break;
                    case '[':
                        endChar = ']';
                        break;
                    case ' ':
                        continue;
                    default:
                        throw new Exception("Bad json");
                }
            }

            if (message[i] == startChar)
                ++counter;
            else if (message[i] == endChar)
            {
                --counter;
                if (counter == 0)
                {
                    parts.Add(message.Substring(start, i + 1 - start));
                    start = -1;
                }
            }
        }

        return parts;
    }

    private void HandleSocketMessages()
    {
        _ws.MessageReceived.SubscribeAsync(
            async response =>
            {
                Console.WriteLine();
                Console.WriteLine("------------------------- frame start -------------------------");
                Console.WriteLine(response.Text);
                Console.WriteLine("-------------------------- frame end --------------------------");

                // var parts = ExtractParts(response.Text);
                var parts = response.Text.Split('\n').Select(l => l.Trim());
                foreach (var part in parts)
                {
                    Reply reply;
                    try
                    {
                        reply = JsonHelper.Deserialize<Reply>(part)!;
                    }
                    catch (Exception e)
                    {
                        _errorEventsSource?.OnNext(new ErrorEvent() { Exception = e });
                        Console.WriteLine(e);
                        return;
                    }

                    if (reply.Error != null)
                    {
                        TaskCompletionSource<JObject>? taskCompletionSource = null;
                        if (reply.Id != 0)
                            _operations.TryRemove(reply.Id, out taskCompletionSource);

                        switch ((ErrorCodes)reply.Error.Code)
                        {
                            case ErrorCodes.Unauthorized:
                            {
                                _authorized = false;

                                taskCompletionSource?.TrySetException(
                                    new UnauthorizedException(
                                        reply.Error.Code,
                                        reply.Error.Message
                                    )
                                );
                                break;
                            }
                            case ErrorCodes.LimitExceeded:
                            {
                                // длина канала исчерпана и прочие ошибки limit exceeded
                                taskCompletionSource?.TrySetException(
                                    new CentrifugoException(
                                        reply.Error.Code,
                                        reply.Error.Message
                                    )
                                );
                                break;
                            }
                            case ErrorCodes.BadRequest: // bad request
                            {
                                taskCompletionSource?.TrySetException(
                                    new CentrifugoException(
                                        reply.Error.Code,
                                        reply.Error.Message
                                    )
                                );
                                break;
                            }
                            case ErrorCodes.TokenExpired:
                            {
                                // token expired
                                taskCompletionSource?.TrySetException(
                                    new TokenExpiredException(
                                        reply.Error.Code,
                                        reply.Error.Message
                                    )
                                );
                                break;
                            }
                            default:
                                taskCompletionSource?.TrySetException(
                                    new CentrifugoException(
                                        reply.Error.Code,
                                        reply.Error.Message
                                    )
                                );
                                break;
                        }

                        _errorEventsSource?.OnNext(new ErrorEvent { Data = reply.Error });
                    }
                    else if (reply.Result != null && reply.Result.Count > 0)
                    {
                        if (reply.GuessVersion() != _protocolVersion)
                            throw new Exception("Reply and protocol version mismatch");

                        if (reply.Id == 0) // если 0, то это push
                        {
                            var push = reply.As<Push>()!;

                            // async messages from server
                            if (push.Type == PushType.Message && string.IsNullOrEmpty(push.Channel))
                            {
                                var message = push.As<Message>();
                                _messageEventsSource?.OnNext(new MessageEvent() { Data = message });
                                return;
                            }

                            if (!_channels.TryGetValue(push.Channel, out var channel))
                                return;

                            switch (push.Type)
                            {
                                case PushType.Publication:
                                    var publication = push.As<Publication>();
                                    channel.PublicationPushEventSource?.OnNext(new()
                                        { Channel = push.Channel, Data = publication });
                                    break;
                                case PushType.Join:
                                    var join = push.As<Join>();
                                    channel.JoinPushEventSource?.OnNext(new() { Channel = push.Channel, Data = join });
                                    break;
                                case PushType.Leave:
                                    var leave = push.As<Leave>();
                                    channel.LeavePushEventSource?.OnNext(new()
                                        { Channel = push.Channel, Data = leave });
                                    break;
                                case PushType.Unsubscribe:
                                    var unsub = push.As<Unsubscribe>();
                                    channel.UnsubscribePushEventSource?.OnNext(new()
                                        { Channel = push.Channel, Data = unsub });
                                    break;
                                case PushType.Message:
                                    var message = push.As<Message>();
                                    channel.MessagePushEventSource?.OnNext(new()
                                        { Channel = push.Channel, Data = message });
                                    break;
                                case PushType.Subscribe:
                                    var subscribe = push.As<Subscribe>();
                                    channel.SubscribePushEventSource?.OnNext(new()
                                        { Channel = push.Channel, Data = subscribe });
                                    break;
                                case PushType.Connect:
                                    var connect = push.As<Connect>();
                                    channel.ConnectPushEventSource?.OnNext(new()
                                        { Channel = push.Channel, Data = connect });
                                    break;
                                case PushType.Disconnect:
                                    var disconnect = push.As<Disconnect>();
                                    channel.DisconnectPushEventSource?.OnNext(new()
                                        { Channel = push.Channel, Data = disconnect });
                                    break;
                                case PushType.Refresh:
                                    var refresh = push.As<Refresh>();
                                    channel.RefreshPushEventSource?.OnNext(new()
                                        { Channel = push.Channel, Data = refresh });
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        else if (_operations.TryRemove(reply.Id, out var taskCompletionSource))
                        {
                            taskCompletionSource.SetResult(reply.Result);
                        }
                    }
                    else if (reply.HasInlineData)
                    {
                        if (reply.GuessVersion() != _protocolVersion)
                            throw new Exception("Reply and protocol version mismatch");

                        if (reply.Id == 0) // если 0, то это push
                        {
                            var push = reply.Push;
                            if (push == null)
                            {
                                await HandleServerPing();
                                return;
                            }

                            // async messages from server
                            if (push.Message != null && string.IsNullOrEmpty(push.Channel))
                            {
                                _messageEventsSource?.OnNext(new MessageEvent() { Data = push.Message });
                                return;
                            }

                            if (!_channels.TryGetValue(push.Channel, out var channel))
                                return;

                            if (push.Pub != null)
                                channel.PublicationPushEventSource?.OnNext(new()
                                    { Channel = push.Channel, Data = push.Pub });
                            else if (push.Join != null)
                                channel.JoinPushEventSource?.OnNext(new()
                                    { Channel = push.Channel, Data = push.Join });
                            else if (push.Leave != null)
                                channel.LeavePushEventSource?.OnNext(new()
                                    { Channel = push.Channel, Data = push.Leave });
                            else if (push.Unsubscribe != null)
                                channel.UnsubscribePushEventSource?.OnNext(new()
                                    { Channel = push.Channel, Data = push.Unsubscribe });
                            else if (push.Subscribe != null)
                                channel.SubscribePushEventSource?.OnNext(new()
                                    { Channel = push.Channel, Data = push.Subscribe });
                            else if (push.Connect != null)
                                channel.ConnectPushEventSource?.OnNext(new()
                                    { Channel = push.Channel, Data = push.Connect });
                            else if (push.Disconnect != null)
                                channel.DisconnectPushEventSource?.OnNext(new()
                                    { Channel = push.Channel, Data = push.Disconnect });
                            else if (push.Refresh != null)
                                channel.RefreshPushEventSource?.OnNext(new()
                                    { Channel = push.Channel, Data = push.Refresh });
                        }
                        else if (_operations.TryRemove(reply.Id, out var taskCompletionSource))
                            taskCompletionSource.SetResult(reply.Result);
                    }
                    else if (reply.Id != 0 && _operations.TryRemove(reply.Id, out var taskCompletionSource))
                        taskCompletionSource.TrySetResult(reply.Result);
                }
            });
    }

    private async Task HandleServerPing()
    {
        await SendAsync(null);
    }

    public void SetToken(string token)
    {
        _token = token;
    }

    public void SetName(string name)
    {
        _name = name;
    }

    private CancellationToken _cancellationToken;

    private Task? _pingRunner;
    
    private PingMethod _pingMethod;

    private ProtocolVersion _protocolVersion;

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (_ws?.NativeClient?.State == WebSocketState.Open && _authorized)
            return;

        _cancellationToken = cancellationToken;

        await _ws.StartOrFail();

        if (_pingMethod == PingMethod.ClientOnly || _pingMethod == PingMethod.Both)
            _pingRunner = PingRunner();

        ConnectRequest request = new()
        {
            Token = _token,
            Name = _name,
        };
        
        var connectCommand = new Command<ConnectRequest, ConnectResult>
        {
            Id = Interlocked.Increment(ref _nextOperationId),
        };

        if (_protocolVersion == ProtocolVersion.V2)
            connectCommand.Connect = request;
        else
        {
            connectCommand.Method = MethodType.Connect;
            connectCommand.Params = request;
        }

        // TODO: retry with exponential backoff, reinit connection on connection lost
        var response = await connectCommand.Handle(this, _cancellationToken);

        _authorized = true;
        //_clientId = authResult.Client;

        _connectedEventSource?.OnNext(new ConnectedEvent { Data = response });
    }

    private async Task PingRunner()
    {
        for (; !_cancellationToken.IsCancellationRequested;)
        {
            await Task.Delay(TimeSpan.FromSeconds(25), _cancellationToken);

            try
            {
                await PingAsync();
            }
            catch (Exception ex)
            {
                _errorEventsSource?.OnNext(new ErrorEvent { Exception = ex });
            }
        }
    }

    public async Task RunAsync()
    {
        for (; !_cancellationToken.IsCancellationRequested;)
        {
            await Task.Delay(100);
        }

        if (_pingRunner != null)
            await _pingRunner;
    }

    internal async Task<V> HandleCommand<T, V>(Command<T, V> command, CancellationToken cancellationToken)
    {
        if (_ws?.NativeClient?.State != WebSocketState.Open)
            return await NullTaskResult.NotConnected<V>();

        if (command.GuessVersion() != _protocolVersion)
            throw new Exception("Command and protocol versions mismatch.");
        
        // not awaitable
        if (command.Method == MethodType.Send || command.Send != null)
        {
            var strCommand = JsonHelper.Serialize(command);
            Console.WriteLine(strCommand);
            _ws.Send(strCommand);

            return await NullTaskResult.Instance<V>();
        }

        if (command.Id == default)
            throw new Exception("Command id cannot be empty");

        var tcs = new TaskCompletionSource<JObject>(TaskCreationOptions.RunContinuationsAsynchronously);

        _operations[command.Id] = tcs;
        cancellationToken.Register(() =>
        {
            tcs.TrySetCanceled(cancellationToken);
            _operations.TryRemove(command.Id, out _);
        });
        
        var strCommand2 = JsonHelper.Serialize(command);
        Console.WriteLine(strCommand2);
        _ws.Send(strCommand2);

        var obj = await tcs.Task;

        return obj.ToObject<V>();
    }

    public async Task<SubscribeResult> SubscribeAsync(Subscription subscription, string? token = null, CancellationToken cancellationToken = default)
    {
        subscription.State = SubscriptionState.Subscribing;

        // если канал уже есть, нельзя давать повторно регаться.
        if (_ws?.NativeClient?.State != WebSocketState.Open)
        {
            subscription.State = SubscriptionState.SubscriptionError;

            throw new Exception("Client not conected");
        }

        if (!_authorized)
        {
            subscription.State = SubscriptionState.SubscriptionError;

            throw new Exception("Not authorized");
        }

        // var offset = subscription.Offset;
        // subscription.Offset = Interlocked.Increment(ref offset);

        var request = new SubscribeRequest
        {
            Channel = subscription.Channel,
            // Offset = subscription.Offset,
            // Recover = false,
            Token = subscription.Channel.StartsWith('$') ? token ?? _token : null
        };
        
        var subscribeCommand = new Command<SubscribeRequest, SubscribeResult>
        {
            Id = Interlocked.Increment(ref _nextOperationId),
            Method = MethodType.Subscribe,
            Params = request
        };

        if (_protocolVersion == ProtocolVersion.V2)
            subscribeCommand.Subscribe = request;
        else
        {
            subscribeCommand.Method = MethodType.Subscribe;
            subscribeCommand.Params = request;
        }

        try
        {
            var response = await subscribeCommand.Handle(this, cancellationToken);

            subscription.State = SubscriptionState.Subscribed;
            _channels[subscription.Channel] = subscription;

            _subscribedEventsSource?.OnNext(new SubscribedEvent(subscription, response));

            return response;
        }
        catch (CentrifugoException e)
        {
            subscription.State = SubscriptionState.SubscriptionError;

            _errorEventsSource?.OnNext(new ErrorEvent()
            {
                Subscription = subscription,
                Exception = e
            });

            throw;
        }
    }

    public async Task UnsubscribeAsync(string channel, CancellationToken cancellationToken = default)
    {
        if (_ws?.NativeClient?.State != WebSocketState.Open)
        {
            // try reconnect or refresh, otherwise throw ex
            return;
        }

        if (!_authorized)
        {
            // try reconnect or refresh, otherwise throw ex
            return;
        }

        if (_channels.TryRemove(channel, out var subscription))
        {
            var unsubscribeCommand = new Command<UnsubscribeRequest, UnsubscribeResult>
            {
                Id = Interlocked.Increment(ref _nextOperationId),
                Method = MethodType.Unsubscribe,
                Params = new UnsubscribeRequest
                {
                    Channel = channel
                }
            };

            try
            {
                var result = await unsubscribeCommand.Handle(this, cancellationToken);

                subscription.State = SubscriptionState.Unsubscribed;
                _unsubscribedEventsSource?.OnNext(new UnsubscribedEvent(subscription, result));
                subscription.Dispose();
            }
            catch (Exception e)
            {
                subscription.State = SubscriptionState.UnsubscriptionError;

                _errorEventsSource?.OnNext(new ErrorEvent()
                {
                    Subscription = subscription,
                    Exception = e
                });

                throw;
            }
        }
    }
    
    public async Task PublishAsync(string channel, JObject payload)
    {
        var publishCommand = new Command<PublishRequest, PublishResult>
        {
            Id = Interlocked.Increment(ref _nextOperationId),
            Method = MethodType.Publish,
            Params = new PublishRequest
            {
                Channel = channel,
                Data = payload
            }
        };

        await HandleCommand(
            publishCommand,
            CancellationToken.None
        );
    }

    public async Task SendAsync(JObject? payload)
    {
        var command = new Command<JObject, JObject>();
        if (_protocolVersion == ProtocolVersion.V2)
            command.Send = new SendRequest() { Data = payload };
        else
        {
            command.Method = MethodType.Send;
            command.Params = payload;
        }

        await HandleCommand(command, CancellationToken.None);
    }

    public void Dispose()
    {
        _ws?.Dispose();

        _connectedEventSource?.OnCompleted();
        _connectedEventSource?.Dispose();

        _messageEventsSource?.OnCompleted();
        _messageEventsSource?.Dispose();

        _errorEventsSource?.OnCompleted();
        _errorEventsSource?.Dispose();
        
        _pingEventsSource?.OnCompleted();
        _pingEventsSource?.Dispose();
        
        _subscribedEventsSource?.OnCompleted();
        _subscribedEventsSource?.Dispose();
        
        _unsubscribedEventsSource?.OnCompleted();
        _unsubscribedEventsSource?.Dispose();

        foreach (var taskCompletionSource in _operations)
        {
            // TODO: custom exception
            taskCompletionSource.Value.TrySetException(new Exception("Client disposed."));
        }
        _operations.Clear();

        foreach (var channel in _channels)
        {
            channel.Value.Dispose();
        }
        _channels.Clear();
    }

    private async Task<PingResult> PingAsync()
    {
        if (_ws?.NativeClient?.State != WebSocketState.Open || !_authorized)
        {
            return await NullTaskResult.Instance<PingResult>();
        }

        var pingCommand = new Command<PingRequest, PingResult>();
        pingCommand.Id = Interlocked.Increment(ref _nextOperationId);

        if (_protocolVersion == ProtocolVersion.V2)
            pingCommand.Ping = new();
        else
            pingCommand.Method = MethodType.Ping;

        var timeout = TimeSpan.FromSeconds(5);
        var tokenSource = new CancellationTokenSource(timeout);

        return await HandleCommand(pingCommand, tokenSource.Token)
            .TimeoutAfterAsync(timeout, cancellationToken: tokenSource.Token);
    }
}