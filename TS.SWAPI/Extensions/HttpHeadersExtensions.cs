using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace TS.SWAPI.Extensions;
internal static  class HttpHeadersExtensions
{
    public static string ToJson(this HttpHeaders httpHeaders) 
        => JsonConvert.SerializeObject(httpHeaders.ToDictionary(k => k.Key, v => v.Value));
}
