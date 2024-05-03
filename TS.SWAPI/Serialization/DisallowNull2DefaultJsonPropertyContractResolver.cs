using System.Reflection;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace TS.SWAPI.Serialization;

internal class DisallowNull2DefaultJsonPropertyContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var jsonProp = base.CreateProperty(member, memberSerialization);
        if (jsonProp.Required == Required.DisallowNull)
        {
            jsonProp.Required = Required.Default;
        }
        return jsonProp;
    }
}