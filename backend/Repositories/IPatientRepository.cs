using HealthcareApp.DTOs;
using HealthcareApp.Models;

namespace HealthcareApp.Repositories;

public interface IPatientRepository
{
    Task<SearchResult<PatientDto>> SearchPatientsAsync(SearchRequest request);
    Task<PatientDto?> GetByIdAsync(int id);
    Task<PatientDto> CreateAsync(CreatePatientDto dto);
    Task<PatientDto?> UpdateAsync(UpdatePatientDto dto);
    Task<bool> DeleteAsync(int id);
}
