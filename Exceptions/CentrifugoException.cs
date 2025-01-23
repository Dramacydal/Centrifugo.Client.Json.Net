namespace Centrifugo.Client.Json.Exceptions;

public class CentrifugoException : Exception
{
    public int? ReplyCode { get; }

    public string? ReplyMessage { get; }

    public CentrifugoException(int replyCode, string replyMessage)
        : base($"Error from centrifugo with {replyCode} - {replyMessage}")
    {
        ReplyCode = replyCode;
        ReplyMessage = replyMessage;
    }

    public CentrifugoException(string? message)
        : base(message)
    {
    }
    
    public CentrifugoException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}