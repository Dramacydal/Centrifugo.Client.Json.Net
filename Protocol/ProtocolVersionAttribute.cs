using Centrifugo.Client.Json.Enums;

namespace Centrifugo.Client.Json.Protocol;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property | AttributeTargets.Field)]
public class ProtocolVersionAttribute(ProtocolVersion version) : Attribute
{
}