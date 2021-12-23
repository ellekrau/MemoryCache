using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CountryAPI.Controllers;

[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    const string CONTRIES_KEY = "Countries";
    const string COUNTRIES_URL = "";
    readonly IMemoryCache _memoryCache;

    public CountryController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<IActionResult> GetCountries()
    {
        if (_memoryCache.TryGetValue(CONTRIES_KEY, out object countriesObject))
            return Ok(countriesObject);

        
    }
}