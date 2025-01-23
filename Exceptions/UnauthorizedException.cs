namespace Centrifugo.Client.Json.Exceptions;

public class UnauthorizedException : CentrifugoException
{
    public UnauthorizedException(int replyCode, string replyMessage)
        : base(replyCode, replyMessage)
    {
    }
    
    public UnauthorizedException(string? message)
        : base(message)
    {
    }
    
    public UnauthorizedException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}