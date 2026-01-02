using Microsoft.EntityFrameworkCore;
using HealthcareApp.Data;
using HealthcareApp.DTOs;
using HealthcareApp.Models;

namespace HealthcareApp.Repositories;

public class CaregiverRepository : ICaregiverRepository
{
    private readonly ApplicationDbContext _context;

    public CaregiverRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SearchResult<CaregiverDto>> SearchCaregiversAsync(SearchRequest request)
    {
        // Start with base query - using IQueryable for deferred execution
        var query = _context.Caregivers
            .Include(c => c.Office)
            .AsQueryable();

        // Apply keyword search with parameterized queries (prevents SQL injection)
        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var keyword = request.Keyword.Trim().ToLower();
            query = query.Where(c =>
                c.FirstName.ToLower().Contains(keyword) ||
                c.LastName.ToLower().Contains(keyword) ||
                c.Phone.Contains(keyword));
        }

        // Get total count before pagination
        var totalCount = await query.CountAsync();

        // Apply sorting
        query = request.SortBy?.ToLower() switch
        {
            "firstname" => request.IsDescending ? query.OrderByDescending(c => c.FirstName) : query.OrderBy(c => c.FirstName),
            "lastname" => request.IsDescending ? query.OrderByDescending(c => c.LastName) : query.OrderBy(c => c.LastName),
            "phone" => request.IsDescending ? query.OrderByDescending(c => c.Phone) : query.OrderBy(c => c.Phone),
            _ => query.OrderBy(c => c.LastName)
        };

        // Apply pagination
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CaregiverDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Phone = c.Phone,
                Email = c.Email,
                Specialization = c.Specialization,
                OfficeId = c.OfficeId,
                OfficeName = c.Office.Name
            })
            .ToListAsync();

        return new SearchResult<CaregiverDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    public async Task<CaregiverDto?> GetByIdAsync(int id)
    {
        return await _context.Caregivers
            .Include(c => c.Office)
            .Where(c => c.Id == id)
            .Select(c => new CaregiverDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Phone = c.Phone,
                Email = c.Email,
                Specialization = c.Specialization,
                OfficeId = c.OfficeId,
                OfficeName = c.Office.Name
            })
            .FirstOrDefaultAsync();
    }

    public async Task<List<CaregiverDto>> GetByIdsAsync(List<int> ids)
    {
        return await _context.Caregivers
            .Include(c => c.Office)
            .Where(c => ids.Contains(c.Id))
            .Select(c => new CaregiverDto
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Phone = c.Phone,
                Email = c.Email,
                Specialization = c.Specialization,
                OfficeId = c.OfficeId,
                OfficeName = c.Office.Name
            })
            .ToListAsync();
    }
}
