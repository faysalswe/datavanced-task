using Microsoft.AspNetCore.Mvc;
using HealthcareApp.DTOs;
using HealthcareApp.Services;

namespace HealthcareApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaregiversController : ControllerBase
{
    private readonly ICaregiverService _service;

    public CaregiversController(ICaregiverService service)
    {
        _service = service;
    }

    /// <summary>
    /// Search caregivers with keyword, pagination, and sorting
    /// </summary>
    /// <param name="keyword">Search by FirstName, LastName, or Phone</param>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Items per page (default: 10)</param>
    /// <param name="sortBy">Sort field (firstname, lastname, phone)</param>
    /// <param name="isDescending">Sort descending (default: false)</param>
    /// <returns>Paginated search results with total count</returns>
    [HttpGet("search")]
    public async Task<ActionResult<SearchResult<CaregiverDto>>> Search(
        [FromQuery] string? keyword,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? sortBy = null,
        [FromQuery] bool isDescending = false)
    {
        var request = new SearchRequest
        {
            Keyword = keyword,
            Page = page,
            PageSize = pageSize,
            SortBy = sortBy,
            IsDescending = isDescending
        };

        var result = await _service.SearchCaregiversAsync(request);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CaregiverDto>> GetById(int id)
    {
        var caregiver = await _service.GetByIdAsync(id);
        if (caregiver == null) return NotFound();
        
        return Ok(caregiver);
    }
}
