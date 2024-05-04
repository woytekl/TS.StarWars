using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Mime;
using TS.SWAPI.Extensions;
using TS.SWAPI.Serialization;

namespace TS.SWAPI.Client;

internal class SWAPIClient : ISWAPIClient
{
    private readonly HttpClient _httpClient;

    public SWAPIClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async virtual Task<T> GetAsync<T>(Uri requestUri) where T : class, new()
    {
        return await GetAsync<T>(requestUri, CancellationToken.None);
    }

    public async virtual Task<T> GetAsync<T>(Uri requestUri, CancellationToken cancellationToken) where T : class, new()
    {
        using var request = new HttpRequestMessage();
        request.Method = HttpMethod.Get;
        request.RequestUri = requestUri;
        request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(MediaTypeNames.Application.Json));

        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        try
        {
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return await ReadObjectAsync<T>(request, response, cancellationToken).ConfigureAwait(false);
                default:
                    throw new SWAPIException($"Http code: {response.StatusCode} returned.", request.Headers.ToJson(), response.StatusCode, response.Content.Headers.ToJson(),
                        await GetResponseString(response));
            }
        }
        finally
        {
            response.Dispose();
        }
    }

    private static async Task<string?> GetResponseString(HttpResponseMessage response) => await response.Content.ReadAsStringAsync().ConfigureAwait(false);

    private async Task<T> ReadObjectAsync<T>(HttpRequestMessage request, HttpResponseMessage response, CancellationToken cancellationToken) where T : class, new()
    {
        try
        {
            using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            using var streamReader = new StreamReader(responseStream);
            using var jsonTextReader = new JsonTextReader(streamReader);

            var serializer = JsonSerializer.Create(new JsonSerializerSettings()
            {
                ContractResolver = new DisallowNull2DefaultJsonPropertyContractResolver()
            });
            return serializer.Deserialize<T>(jsonTextReader) ?? throw new JsonException("Null deserialization result.");
        }
        catch (JsonException exception)
        {
            throw new SWAPIException($"Deserialization of '{typeof(T).Name}' exception.", request.Headers?.ToJson(), response.StatusCode, response.Content.Headers.ToJson(),
                string.Empty, exception);
        }
    }
}
