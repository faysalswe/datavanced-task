namespace HealthcareApp.Models;

// Junction table for many-to-many relationship
public class PatientCaregiver
{
    public int PatientId { get; set; }
    public int CaregiverId { get; set; }
    public DateTime AssignedAt { get; set; }
    
    // Navigation properties
    public Patient Patient { get; set; } = null!;
    public Caregiver Caregiver { get; set; } = null!;
}
