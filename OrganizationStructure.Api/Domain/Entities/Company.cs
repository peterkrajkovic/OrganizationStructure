namespace OrganizationStructure.Api.Domain.Entities;

public class Company
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public Guid DirectorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public Employee Director { get; set; } = null!;
    public ICollection<Division> Divisions { get; set; } = new List<Division>();
}