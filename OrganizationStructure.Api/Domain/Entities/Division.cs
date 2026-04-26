namespace OrganizationStructure.Api.Domain.Entities;

public class Division
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public Guid CompanyId { get; set; }
    public Guid ManagerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public Company Company { get; set; } = null!;
    public Employee Manager { get; set; } = null!;
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}