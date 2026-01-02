namespace HealthcareApp.Models;

public class AuditLog
{
    public int Id { get; set; }
    public string UserRole { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string QueryString { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public int ResponseTime { get; set; }
}
