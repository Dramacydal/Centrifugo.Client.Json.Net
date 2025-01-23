namespace Centrifugo.Client.Json.Protocol.Enums;

public enum ErrorCodes
{
    InternalServerError = 100,
    Unauthorized = 101,
    UnknownChannel = 102,
    PermissionDenied = 103,
    MethodNotFound = 104,
    AlreadySubscribed = 105,
    LimitExceeded = 106,
    BadRequest = 107,
    NotAvailable = 108,
    TokenExpired = 109,
    Expired = 110,
    TooManyRequests = 111,
    UnrecoverablePosition = 112,
}