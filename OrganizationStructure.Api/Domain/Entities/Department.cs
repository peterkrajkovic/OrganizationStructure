namespace OrganizationStructure.Api.Domain.Entities;

public class Department
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public Guid ProjectId { get; set; }
    public Guid ManagerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public Project Project { get; set; } = null!;
    public Employee Manager { get; set; } = null!;
}