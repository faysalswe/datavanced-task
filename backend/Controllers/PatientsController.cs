using Microsoft.AspNetCore.Mvc;
using HealthcareApp.DTOs;
using HealthcareApp.Services;

namespace HealthcareApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _service;

    public PatientsController(IPatientService service)
    {
        _service = service;
    }

    [HttpGet("search")]
    public async Task<ActionResult<SearchResult<PatientDto>>> Search(
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

        var result = await _service.SearchPatientsAsync(request);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PatientDto>> GetById(int id)
    {
        var patient = await _service.GetByIdAsync(id);
        if (patient == null) return NotFound();
        
        return Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<PatientDto>> Create([FromBody] CreatePatientDto dto)
    {
        var patient = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PatientDto>> Update(int id, [FromBody] UpdatePatientDto dto)
    {
        if (id != dto.Id) return BadRequest("ID mismatch");

        var patient = await _service.UpdateAsync(dto);
        if (patient == null) return NotFound();
        
        return Ok(patient);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result) return NotFound();
        
        return NoContent();
    }
}
