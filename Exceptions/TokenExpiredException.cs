namespace Centrifugo.Client.Json.Exceptions;

public class TokenExpiredException : CentrifugoException
{
    public TokenExpiredException(int replyCode, string replyMessage)
        : base(replyCode, replyMessage)
    {
    }
    
    public TokenExpiredException(string? message)
        : base(message)
    {
    }
    
    public TokenExpiredException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
