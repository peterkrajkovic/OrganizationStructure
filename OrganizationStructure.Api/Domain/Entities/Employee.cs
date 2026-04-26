namespace OrganizationStructure.Api.Domain.Entities;

public class Employee
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public ICollection<Company> CompaniesAsDirector { get; set; } = new List<Company>();
    public ICollection<Division> DivisionsAsManager { get; set; } = new List<Division>();
    public ICollection<Project> ProjectsAsManager { get; set; } = new List<Project>();
    public ICollection<Department> DepartmentsAsManager { get; set; } = new List<Department>();
}