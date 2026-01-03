using HealthcareApp.Data;
using HealthcareApp.DTOs;
using Microsoft.EntityFrameworkCore;

namespace HealthcareApp.Services;

public interface ISearchService
{
    Task<UnifiedSearchResult> SearchAsync(SearchRequest request);
}

public class SearchService : ISearchService
{
    private readonly ApplicationDbContext _context;

    public SearchService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UnifiedSearchResult> SearchAsync(SearchRequest request)
    {
        var keyword = (request.Keyword ?? string.Empty).ToLower();
        var pageSize = request.PageSize;
        var skip = (request.Page - 1) * pageSize;

        // Search offices by name and phone
        var officesQuery = _context.Offices.AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            officesQuery = officesQuery.Where(o =>
                o.Name.ToLower().Contains(keyword) ||
                o.Phone.ToLower().Contains(keyword));
        }

        // Search patients by name and phone
        var patientsQuery = _context.Patients.Include(p => p.Office).AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            patientsQuery = patientsQuery.Where(p =>
                p.FirstName.ToLower().Contains(keyword) ||
                p.LastName.ToLower().Contains(keyword) ||
                p.Phone.ToLower().Contains(keyword));
        }

        // Search caregivers by name and phone
        var caregiversQuery = _context.Caregivers.Include(c => c.Office).AsQueryable();
        if (!string.IsNullOrEmpty(keyword))
        {
            caregiversQuery = caregiversQuery.Where(c =>
                c.FirstName.ToLower().Contains(keyword) ||
                c.LastName.ToLower().Contains(keyword) ||
                c.Phone.ToLower().Contains(keyword));
        }

        // Get total counts
        var officesCount = await officesQuery.CountAsync();
        var patientsCount = await patientsQuery.CountAsync();
        var caregiversCount = await caregiversQuery.CountAsync();
        var totalCount = officesCount + patientsCount + caregiversCount;

        // Execute queries with pagination
        var offices = await officesQuery
            .Skip(skip)
            .Take(pageSize)
            .Select(o => new OfficeSearchResult
            {
                Id = o.Id,
                Name = o.Name,
                Address = o.Address,
                Phone = o.Phone
            })
            .ToListAsync();

        var patients = await patientsQuery
            .Skip(skip)
            .Take(pageSize)
            .Select(p => new PatientSearchResult
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Phone = p.Phone,
                Email = p.Email,
                OfficeName = p.Office.Name
            })
            .ToListAsync();

        var caregivers = await caregiversQuery
            .Skip(skip)
            .Take(pageSize)
            .Select(c => new CaregiverSearchResult
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Phone = c.Phone,
                Email = c.Email,
                Specialization = c.Specialization,
                OfficeName = c.Office.Name
            })
            .ToListAsync();

        return new UnifiedSearchResult
        {
            Offices = offices,
            Patients = patients,
            Caregivers = caregivers,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = pageSize
        };
    }
}
