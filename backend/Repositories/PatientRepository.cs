using Microsoft.EntityFrameworkCore;
using HealthcareApp.Data;
using HealthcareApp.DTOs;
using HealthcareApp.Models;

namespace HealthcareApp.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly ApplicationDbContext _context;

    public PatientRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SearchResult<PatientDto>> SearchPatientsAsync(SearchRequest request)
    {
        var query = _context.Patients
            .Include(p => p.Office)
            .Include(p => p.PatientCaregivers)
                .ThenInclude(pc => pc.Caregiver)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var keyword = request.Keyword.Trim().ToLower();
            query = query.Where(p =>
                p.FirstName.ToLower().Contains(keyword) ||
                p.LastName.ToLower().Contains(keyword) ||
                p.Phone.Contains(keyword) ||
                p.Email.ToLower().Contains(keyword));
        }

        var totalCount = await query.CountAsync();

        query = request.SortBy?.ToLower() switch
        {
            "firstname" => request.IsDescending ? query.OrderByDescending(p => p.FirstName) : query.OrderBy(p => p.FirstName),
            "lastname" => request.IsDescending ? query.OrderByDescending(p => p.LastName) : query.OrderBy(p => p.LastName),
            "dateofbirth" => request.IsDescending ? query.OrderByDescending(p => p.DateOfBirth) : query.OrderBy(p => p.DateOfBirth),
            _ => query.OrderBy(p => p.LastName)
        };

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new PatientDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                DateOfBirth = p.DateOfBirth,
                Phone = p.Phone,
                Email = p.Email,
                Address = p.Address,
                OfficeId = p.OfficeId,
                OfficeName = p.Office.Name,
                Caregivers = p.PatientCaregivers.Select(pc => new CaregiverDto
                {
                    Id = pc.Caregiver.Id,
                    FirstName = pc.Caregiver.FirstName,
                    LastName = pc.Caregiver.LastName,
                    Phone = pc.Caregiver.Phone,
                    Email = pc.Caregiver.Email,
                    Specialization = pc.Caregiver.Specialization,
                    OfficeId = pc.Caregiver.OfficeId,
                    OfficeName = pc.Caregiver.Office.Name
                }).ToList()
            })
            .ToListAsync();

        return new SearchResult<PatientDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    public async Task<PatientDto?> GetByIdAsync(int id)
    {
        return await _context.Patients
            .Include(p => p.Office)
            .Include(p => p.PatientCaregivers)
                .ThenInclude(pc => pc.Caregiver)
                    .ThenInclude(c => c.Office)
            .Where(p => p.Id == id)
            .Select(p => new PatientDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                DateOfBirth = p.DateOfBirth,
                Phone = p.Phone,
                Email = p.Email,
                Address = p.Address,
                OfficeId = p.OfficeId,
                OfficeName = p.Office.Name,
                Caregivers = p.PatientCaregivers.Select(pc => new CaregiverDto
                {
                    Id = pc.Caregiver.Id,
                    FirstName = pc.Caregiver.FirstName,
                    LastName = pc.Caregiver.LastName,
                    Phone = pc.Caregiver.Phone,
                    Email = pc.Caregiver.Email,
                    Specialization = pc.Caregiver.Specialization,
                    OfficeId = pc.Caregiver.OfficeId,
                    OfficeName = pc.Caregiver.Office.Name
                }).ToList()
            })
            .FirstOrDefaultAsync();
    }

    public async Task<PatientDto> CreateAsync(CreatePatientDto dto)
    {
        var patient = new Patient
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            Phone = dto.Phone,
            Email = dto.Email,
            Address = dto.Address,
            OfficeId = dto.OfficeId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        // Assign caregivers
        if (dto.CaregiverIds.Any())
        {
            var patientCaregivers = dto.CaregiverIds.Select(cid => new PatientCaregiver
            {
                PatientId = patient.Id,
                CaregiverId = cid,
                AssignedAt = DateTime.UtcNow
            }).ToList();

            _context.PatientCaregivers.AddRange(patientCaregivers);
            await _context.SaveChangesAsync();
        }

        return (await GetByIdAsync(patient.Id))!;
    }

    public async Task<PatientDto?> UpdateAsync(UpdatePatientDto dto)
    {
        var patient = await _context.Patients
            .Include(p => p.PatientCaregivers)
            .FirstOrDefaultAsync(p => p.Id == dto.Id);

        if (patient == null) return null;

        patient.FirstName = dto.FirstName;
        patient.LastName = dto.LastName;
        patient.DateOfBirth = dto.DateOfBirth;
        patient.Phone = dto.Phone;
        patient.Email = dto.Email;
        patient.Address = dto.Address;
        patient.OfficeId = dto.OfficeId;
        patient.UpdatedAt = DateTime.UtcNow;

        // Update caregiver assignments
        var existingAssignments = patient.PatientCaregivers.ToList();
        var newCaregiverIds = dto.CaregiverIds;

        // Remove old assignments
        var toRemove = existingAssignments.Where(pc => !newCaregiverIds.Contains(pc.CaregiverId)).ToList();
        _context.PatientCaregivers.RemoveRange(toRemove);

        // Add new assignments
        var existingIds = existingAssignments.Select(pc => pc.CaregiverId).ToList();
        var toAdd = newCaregiverIds.Where(id => !existingIds.Contains(id)).Select(cid => new PatientCaregiver
        {
            PatientId = patient.Id,
            CaregiverId = cid,
            AssignedAt = DateTime.UtcNow
        }).ToList();

        _context.PatientCaregivers.AddRange(toAdd);
        await _context.SaveChangesAsync();

        return await GetByIdAsync(patient.Id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) return false;

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();
        return true;
    }
}
