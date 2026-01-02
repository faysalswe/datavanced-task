using HealthcareApp.DTOs;
using HealthcareApp.Repositories;

namespace HealthcareApp.Services;

public interface ICaregiverService
{
    Task<SearchResult<CaregiverDto>> SearchCaregiversAsync(SearchRequest request);
    Task<CaregiverDto?> GetByIdAsync(int id);
}

public class CaregiverService : ICaregiverService
{
    private readonly ICaregiverRepository _repository;
    private readonly ICacheService _cache;

    public CaregiverService(ICaregiverRepository repository, ICacheService cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<SearchResult<CaregiverDto>> SearchCaregiversAsync(SearchRequest request)
    {
        // Create cache key based on search parameters
        var cacheKey = $"caregiver:search:{request.Keyword}:{request.Page}:{request.PageSize}:{request.SortBy}:{request.IsDescending}";
        
        // Try to get from cache
        var cached = await _cache.GetAsync<SearchResult<CaregiverDto>>(cacheKey);
        if (cached != null)
        {
            return cached;
        }

        // If not in cache, get from database
        var result = await _repository.SearchCaregiversAsync(request);
        
        // Cache for 5 minutes
        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));
        
        return result;
    }

    public async Task<CaregiverDto?> GetByIdAsync(int id)
    {
        var cacheKey = $"caregiver:{id}";
        var cached = await _cache.GetAsync<CaregiverDto>(cacheKey);
        if (cached != null) return cached;

        var caregiver = await _repository.GetByIdAsync(id);
        if (caregiver != null)
        {
            await _cache.SetAsync(cacheKey, caregiver, TimeSpan.FromMinutes(10));
        }

        return caregiver;
    }
}
