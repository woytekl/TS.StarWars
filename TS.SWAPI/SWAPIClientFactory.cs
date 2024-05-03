using TS.SWAPI.Client;

namespace TS.SWAPI
{
    public class SWAPIClientFactory
    {
        public SWAPIClientFactory()
        {
        }

        public ISWAPIClient Create(HttpClient httpClient) 
        {
            return new SWAPIClient(httpClient);
        }
    }
}
