using System.Net;

namespace TS.SWAPI.Client;

[Serializable]
internal class SWAPIException : Exception
{
    internal SWAPIException(string message, string? requestHeaders, HttpStatusCode statusCode, string? contentHeaders, string? responseString, Exception? exception = null)
        : base(message, exception)
    {
        RequestHeaders = requestHeaders;
        StatusCode = statusCode;
        ContentHeaders = contentHeaders;
        ResponseString = responseString;
    }

    public string? RequestHeaders
    {
        get;
    }
    public HttpStatusCode StatusCode
    {
        get;
    }
    public string? ContentHeaders
    {
        get;
    }
    public string? ResponseString
    {
        get;
    }
}
