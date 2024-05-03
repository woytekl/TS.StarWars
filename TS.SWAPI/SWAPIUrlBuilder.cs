namespace TS.SWAPI;

public static class SWAPIUrlBuilder
{
    public static Uri GetSearchPeopleUri(string baseUrl, string searchCriteria) 
    {
        return new Uri($"{baseUrl}{SWAPIConstants.PeopleResourceName}?{SWAPIConstants.SearchKeyword}={Uri.EscapeDataString(searchCriteria)}");
    }
}
