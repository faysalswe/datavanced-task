using HealthcareApp.DTOs;
using HealthcareApp.Repositories;

namespace HealthcareApp.Services;

public interface IPatientService
{
    Task<SearchResult<PatientDto>> SearchPatientsAsync(SearchRequest request);
    Task<PatientDto?> GetByIdAsync(int id);
    Task<PatientDto> CreateAsync(CreatePatientDto dto);
    Task<PatientDto?> UpdateAsync(UpdatePatientDto dto);
    Task<bool> DeleteAsync(int id);
}

public class PatientService : IPatientService
{
    private readonly IPatientRepository _repository;
    private readonly ICacheService _cache;

    public PatientService(IPatientRepository repository, ICacheService cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<SearchResult<PatientDto>> SearchPatientsAsync(SearchRequest request)
    {
        var cacheKey = $"patient:search:{request.Keyword}:{request.Page}:{request.PageSize}:{request.SortBy}:{request.IsDescending}";
        
        var cached = await _cache.GetAsync<SearchResult<PatientDto>>(cacheKey);
        if (cached != null) return cached;

        var result = await _repository.SearchPatientsAsync(request);
        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        
        return result;
    }

    public async Task<PatientDto?> GetByIdAsync(int id)
    {
        var cacheKey = $"patient:{id}";
        var cached = await _cache.GetAsync<PatientDto>(cacheKey);
        if (cached != null) return cached;

        var patient = await _repository.GetByIdAsync(id);
        if (patient != null)
        {
            await _cache.SetAsync(cacheKey, patient, TimeSpan.FromMinutes(10));
        }

        return patient;
    }

    public async Task<PatientDto> CreateAsync(CreatePatientDto dto)
    {
        var patient = await _repository.CreateAsync(dto);
        
        // Invalidate search cache
        await _cache.RemoveByPatternAsync("patient:search:*");
        
        return patient;
    }

    public async Task<PatientDto?> UpdateAsync(UpdatePatientDto dto)
    {
        var patient = await _repository.UpdateAsync(dto);
        
        if (patient != null)
        {
            // Invalidate caches
            await _cache.RemoveAsync($"patient:{dto.Id}");
            await _cache.RemoveByPatternAsync("patient:search:*");
        }
        
        return patient;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _repository.DeleteAsync(id);
        
        if (result)
        {
            await _cache.RemoveAsync($"patient:{id}");
            await _cache.RemoveByPatternAsync("patient:search:*");
        }
        
        return result;
    }
}
