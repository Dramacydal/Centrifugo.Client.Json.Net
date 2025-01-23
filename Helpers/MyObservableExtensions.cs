using System.Reactive.Linq;

namespace Centrifugo.Client.Json.Helpers;

public static class MyObservableExtensions
{
    public static IDisposable SubscribeAsync<T>(this IObservable<T> source, Func<T, Task> onNextAsync) =>
        source
            .Select(number => Observable.FromAsync(() => onNextAsync(number)))
            .Concat()
            .Subscribe();
}
