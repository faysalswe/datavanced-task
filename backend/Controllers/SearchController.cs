using Microsoft.AspNetCore.Mvc;
using HealthcareApp.DTOs;
using HealthcareApp.Services;

namespace HealthcareApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ISearchService _service;

    public SearchController(ISearchService service)
    {
        _service = service;
    }

    [HttpGet("unified")]
    public async Task<ActionResult<UnifiedSearchResult>> UnifiedSearch(
        [FromQuery] string? keyword,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var request = new SearchRequest
        {
            Keyword = keyword,
            Page = page,
            PageSize = pageSize
        };

        var result = await _service.SearchAsync(request);
        return Ok(result);
    }
}
