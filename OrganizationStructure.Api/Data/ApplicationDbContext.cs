using Microsoft.EntityFrameworkCore;
using OrganizationStructure.Api.Domain.Entities;

namespace OrganizationStructure.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Division> Divisions { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Employee configuration
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(50);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        // Company configuration
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
            entity.Property(c => c.Code).IsRequired().HasMaxLength(20);
            entity.HasIndex(c => c.Code).IsUnique();
            entity.Property(c => c.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            
            entity.HasOne(c => c.Director)
                .WithMany(e => e.CompaniesAsDirector)
                .HasForeignKey(c => c.DirectorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Division configuration
        modelBuilder.Entity<Division>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(200);
            entity.Property(d => d.Code).IsRequired().HasMaxLength(20);
            entity.HasIndex(d => new { d.CompanyId, d.Code }).IsUnique();
            entity.Property(d => d.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            
            entity.HasOne(d => d.Company)
                .WithMany(c => c.Divisions)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(d => d.Manager)
                .WithMany(e => e.DivisionsAsManager)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Project configuration
        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Code).IsRequired().HasMaxLength(20);
            entity.HasIndex(p => new { p.DivisionId, p.Code }).IsUnique();
            entity.Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            
            entity.HasOne(p => p.Division)
                .WithMany(d => d.Projects)
                .HasForeignKey(p => p.DivisionId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(p => p.Manager)
                .WithMany(e => e.ProjectsAsManager)
                .HasForeignKey(p => p.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Department configuration
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.Name).IsRequired().HasMaxLength(200);
            entity.Property(d => d.Code).IsRequired().HasMaxLength(20);
            entity.HasIndex(d => new { d.ProjectId, d.Code }).IsUnique();
            entity.Property(d => d.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            
            entity.HasOne(d => d.Project)
                .WithMany(p => p.Departments)
                .HasForeignKey(d => d.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(d => d.Manager)
                .WithMany(e => e.DepartmentsAsManager)
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}