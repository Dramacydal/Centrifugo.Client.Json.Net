namespace Centrifugo.Client.Json.Events;

public class PushEvent<T>
{
    public string Channel { get; init; }

    public T Data { get; init; }
}
