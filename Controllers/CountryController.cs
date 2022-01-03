using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CountryAPI.Controllers;

[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    const string CONTRIES_KEY = "Countries";
    const string COUNTRIES_URL = "https://restcountries.com/v2/all";
    readonly IMemoryCache _memoryCache;
    readonly HttpClient _countryHttpClient;

    public CountryController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        _countryHttpClient = new HttpClient { BaseAddress = new Uri(COUNTRIES_URL) };
    }

    [HttpGet]
    public async Task<IActionResult> GetCountries()
    {
        if (_memoryCache.TryGetValue(CONTRIES_KEY, out object countriesObject))
            return Ok(countriesObject);

        var response = await _countryHttpClient.GetAsync(string.Empty);
        var countries = await response.Content.ReadFromJsonAsync<IList<Country>>();

        var memoryCacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(3600),
            SlidingExpiration = TimeSpan.FromSeconds(1200)
        };

        _memoryCache.Set(CONTRIES_KEY, countries, memoryCacheOptions);

        return Ok(countries);
    }
}