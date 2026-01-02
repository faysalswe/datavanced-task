using HealthcareApp.DTOs;

namespace HealthcareApp.Repositories;

public interface ICaregiverRepository
{
    Task<SearchResult<CaregiverDto>> SearchCaregiversAsync(SearchRequest request);
    Task<CaregiverDto?> GetByIdAsync(int id);
    Task<List<CaregiverDto>> GetByIdsAsync(List<int> ids);
}
