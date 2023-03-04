using HCM.Api.Data.Models;
using HCM.Shared.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HCM.Api.Data;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> dbContextOptions) : base(dbContextOptions) { }

    protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
    {
        dbContextOptionsBuilder.EnableSensitiveDataLogging();
        base.OnConfiguring(dbContextOptionsBuilder);
    }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<Job> Jobs { get; set; }

    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        ConfigureEntityRelations(builder);
        SetDateKindUtcToDateTimeEntities(builder);
    }

    private void SetDateKindUtcToDateTimeEntities(ModelBuilder builder)
    {
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
            v => v.HasValue ? v.Value.ToUniversalTime() : v,
            v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (entityType.IsKeyless)
            {
                continue;
            }

            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
                else if (property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(nullableDateTimeConverter);
                }
            }
        }
    }

    private void ConfigureEntityRelations(ModelBuilder modelBuilder)
    {
        ConfigureEmployees(modelBuilder);
        ConfigureJobs(modelBuilder);
        ConfigureDepartments(modelBuilder);
    }

    private void ConfigureDepartments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.Property(p => p.Name).IsRequired().HasMaxLength(50);
            entity.Property(p => p.Address).IsRequired().HasMaxLength(100);
        });
    }

    private void ConfigureJobs(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Job>(entity =>
        {
            entity.Property(p => p.Title).IsRequired().HasMaxLength(50);
            entity.Property(p => p.MinSalary).IsRequired().HasPrecision(8, 2);
            entity.Property(p => p.MaxSalary).IsRequired().HasPrecision(8, 2);
        });
    }

    private void ConfigureEmployees(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasOne(e => e.Job)
                .WithMany(j => j.Employees)
                .IsRequired()
                .HasForeignKey(e => e.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            entity
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .IsRequired()
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(p => p.Email).IsUnique();
            entity.Property(p => p.FirstName).IsRequired().HasMaxLength(30);
            entity.Property(p => p.LastName).IsRequired().HasMaxLength(30);
            entity.Property(p => p.Email).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Salary).IsRequired().HasPrecision(8, 2);
            entity.Property(p => p.PhoneNumber).HasMaxLength(20);
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        SaveChangesAsync(true, cancellationToken);

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        ApplyAuditInfoRules();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void ApplyAuditInfoRules()
    {
        var changedEntries = ChangeTracker
            .Entries()
            .Where(e =>
                e is { Entity: IAuditInfo, State: EntityState.Added or EntityState.Modified });

        foreach (var entry in changedEntries)
        {
            var entity = (IAuditInfo)entry.Entity;
            if (entry.State == EntityState.Added && entity.CreatedOn == default)
            {
                entity.CreatedOn = DateTime.UtcNow;
            }
            else
            {
                entity.ModifiedOn = DateTime.UtcNow;
            }
        }
    }
}