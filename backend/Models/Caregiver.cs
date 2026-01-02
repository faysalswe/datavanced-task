namespace HealthcareApp.Models;

public class Caregiver
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public int OfficeId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public Office Office { get; set; } = null!;
    public ICollection<PatientCaregiver> PatientCaregivers { get; set; } = new List<PatientCaregiver>();
}
