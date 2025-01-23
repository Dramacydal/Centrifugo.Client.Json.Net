using Centrifugo.Client.Json.Exceptions;

namespace Centrifugo.Client.Json.Helpers;

public static class NullTaskResult
{
    public static Task<T> Instance<T>() => Task<T>.FromResult(default(T));

    public static Task<T> NotConnected<T>()  => Task.FromException<T>(new NotConnectedException());
}