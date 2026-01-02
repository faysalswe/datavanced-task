using Microsoft.EntityFrameworkCore;
using HealthcareApp.Data;
using HealthcareApp.Models;

namespace HealthcareApp.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Check if data already exists
        if (await context.Offices.AnyAsync())
        {
            return;
        }

        // Seed Offices
        var offices = new List<Office>
        {
            new Office
            {
                Name = "Main Clinic",
                Address = "123 Healthcare Ave, Medical City",
                Phone = "555-0100",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Office
            {
                Name = "Downtown Medical Center",
                Address = "456 Downtown St, Medical City",
                Phone = "555-0200",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        context.Offices.AddRange(offices);
        await context.SaveChangesAsync();

        // Seed Caregivers
        var caregivers = new List<Caregiver>
        {
            new Caregiver
            {
                FirstName = "John",
                LastName = "Smith",
                Phone = "555-1001",
                Email = "john.smith@healthcare.com",
                Specialization = "Cardiology",
                OfficeId = offices[0].Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Caregiver
            {
                FirstName = "Sarah",
                LastName = "Johnson",
                Phone = "555-1002",
                Email = "sarah.johnson@healthcare.com",
                Specialization = "Pediatrics",
                OfficeId = offices[0].Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Caregiver
            {
                FirstName = "Michael",
                LastName = "Brown",
                Phone = "555-1003",
                Email = "michael.brown@healthcare.com",
                Specialization = "Neurology",
                OfficeId = offices[0].Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Caregiver
            {
                FirstName = "Emily",
                LastName = "Davis",
                Phone = "555-1004",
                Email = "emily.davis@healthcare.com",
                Specialization = "Orthopedics",
                OfficeId = offices[1].Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Caregiver
            {
                FirstName = "David",
                LastName = "Wilson",
                Phone = "555-1005",
                Email = "david.wilson@healthcare.com",
                Specialization = "General Practice",
                OfficeId = offices[1].Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        context.Caregivers.AddRange(caregivers);
        await context.SaveChangesAsync();

        // Seed Patients
        var patients = new List<Patient>
        {
            new Patient
            {
                FirstName = "Alice",
                LastName = "Anderson",
                DateOfBirth = new DateTime(1985, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                Phone = "555-2001",
                Email = "alice.anderson@email.com",
                Address = "789 Oak Street, Medical City",
                OfficeId = offices[0].Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Patient
            {
                FirstName = "Bob",
                LastName = "Baker",
                DateOfBirth = new DateTime(1990, 7, 22, 0, 0, 0, DateTimeKind.Utc),
                Phone = "555-2002",
                Email = "bob.baker@email.com",
                Address = "321 Pine Road, Medical City",
                OfficeId = offices[0].Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Patient
            {
                FirstName = "Carol",
                LastName = "Clark",
                DateOfBirth = new DateTime(1978, 11, 8, 0, 0, 0, DateTimeKind.Utc),
                Phone = "555-2003",
                Email = "carol.clark@email.com",
                Address = "654 Maple Drive, Medical City",
                OfficeId = offices[1].Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Patient
            {
                FirstName = "Daniel",
                LastName = "Davis",
                DateOfBirth = new DateTime(1995, 5, 30, 0, 0, 0, DateTimeKind.Utc),
                Phone = "555-2004",
                Email = "daniel.davis@email.com",
                Address = "987 Elm Avenue, Medical City",
                OfficeId = offices[1].Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        context.Patients.AddRange(patients);
        await context.SaveChangesAsync();

        // Seed Patient-Caregiver relationships
        var patientCaregivers = new List<PatientCaregiver>
        {
            new PatientCaregiver
            {
                PatientId = patients[0].Id,
                CaregiverId = caregivers[0].Id,
                AssignedAt = DateTime.UtcNow
            },
            new PatientCaregiver
            {
                PatientId = patients[0].Id,
                CaregiverId = caregivers[1].Id,
                AssignedAt = DateTime.UtcNow
            },
            new PatientCaregiver
            {
                PatientId = patients[1].Id,
                CaregiverId = caregivers[1].Id,
                AssignedAt = DateTime.UtcNow
            },
            new PatientCaregiver
            {
                PatientId = patients[1].Id,
                CaregiverId = caregivers[2].Id,
                AssignedAt = DateTime.UtcNow
            },
            new PatientCaregiver
            {
                PatientId = patients[2].Id,
                CaregiverId = caregivers[3].Id,
                AssignedAt = DateTime.UtcNow
            },
            new PatientCaregiver
            {
                PatientId = patients[3].Id,
                CaregiverId = caregivers[3].Id,
                AssignedAt = DateTime.UtcNow
            },
            new PatientCaregiver
            {
                PatientId = patients[3].Id,
                CaregiverId = caregivers[4].Id,
                AssignedAt = DateTime.UtcNow
            }
        };

        context.PatientCaregivers.AddRange(patientCaregivers);
        await context.SaveChangesAsync();

        Console.WriteLine("Sample data seeded successfully!");
    }
}
