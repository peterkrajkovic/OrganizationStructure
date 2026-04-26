namespace OrganizationStructure.Api.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public Guid DivisionId { get; set; }
    public Guid ManagerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public Division Division { get; set; } = null!;
    public Employee Manager { get; set; } = null!;
    public ICollection<Department> Departments { get; set; } = new List<Department>();
}