using TS.SWAPI.Models.Resources;

namespace TS.SWAPI;

public class SWAPIUrlBuilder
{
    private readonly Resource _resources;

    public SWAPIUrlBuilder(Resource resources)
    {
        _resources = resources;
    }

    public Uri GetSearchPeopleUri(string searchCriteria) 
    {
        return new Uri($"{_resources.People}?{SWAPIConstants.SearchKeyword}={Uri.EscapeDataString(searchCriteria)}");
    }
}
