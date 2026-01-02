namespace HealthcareApp.Models;

public class Office
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    // Navigation properties
    public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    public ICollection<Caregiver> Caregivers { get; set; } = new List<Caregiver>();
}
