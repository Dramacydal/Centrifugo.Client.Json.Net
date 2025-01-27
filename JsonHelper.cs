using System.Collections;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Centrifugo.Client.Json;

public static class JsonHelper
{
    private static readonly JsonSerializerSettings _settings;

    class ShouldSerializeContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            var valueGetter = (object obj) =>
            {
                if (member is PropertyInfo propertyInfo)
                    return propertyInfo.GetValue(obj);
                if (member is FieldInfo fieldInfo)
                    return fieldInfo.GetValue(obj);

                throw new Exception("Unknown member type");
            };

            if (property.PropertyType == typeof(string))
            {
                // Do not include empty strings
                property.ShouldSerialize = instance => !string.IsNullOrEmpty(valueGetter(instance) as string);
            }
            else if (property.PropertyType == typeof(Guid))
            {
                property.ShouldSerialize = instance =>
                {
                    var guid = valueGetter(instance);
                    if (guid == null)
                        return false;

                    return (Guid)guid != Guid.Empty;
                };
            }
            else if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
            {
                property.ShouldSerialize = instance => valueGetter(instance) is IEnumerable enumerable
                    ? enumerable.GetEnumerator().MoveNext()
                    : false;
            }

            return property;
        }
    }

    static JsonHelper()
    {
        _settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            ContractResolver = new ShouldSerializeContractResolver(),
        };
    }
    
    public static string Serialize(object? obj)
    {
        return JsonConvert.SerializeObject(obj, _settings);
    }

    public static T? Deserialize<T>(string json)
    {
        return JsonConvert.DeserializeObject<T>(json, _settings);
    }
}