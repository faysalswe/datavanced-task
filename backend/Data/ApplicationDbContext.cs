using Microsoft.EntityFrameworkCore;
using HealthcareApp.Models;

namespace HealthcareApp.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Office> Offices { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Caregiver> Caregivers { get; set; }
    public DbSet<PatientCaregiver> PatientCaregivers { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure many-to-many relationship
        modelBuilder.Entity<PatientCaregiver>()
            .HasKey(pc => new { pc.PatientId, pc.CaregiverId });

        modelBuilder.Entity<PatientCaregiver>()
            .HasOne(pc => pc.Patient)
            .WithMany(p => p.PatientCaregivers)
            .HasForeignKey(pc => pc.PatientId);

        modelBuilder.Entity<PatientCaregiver>()
            .HasOne(pc => pc.Caregiver)
            .WithMany(c => c.PatientCaregivers)
            .HasForeignKey(pc => pc.CaregiverId);

        // Configure indexes for search optimization
        modelBuilder.Entity<Patient>()
            .HasIndex(p => new { p.FirstName, p.LastName });

        modelBuilder.Entity<Caregiver>()
            .HasIndex(c => new { c.FirstName, c.LastName, c.Phone });

        modelBuilder.Entity<Office>()
            .HasIndex(o => o.Name);
    }
}
