using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeesApp.Models;

public class EmployeeContext : DbContext
{
    public EmployeeContext(DbContextOptions options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        // Alternative to the above
        /*
        modelBuilder
            .Entity<Employee>()
            .HasData(
             new Employee
             {
                 Id = Guid.NewGuid(),
                 Name = "Mark",
                 AccountNumber = "123-3452134543-32",
                 Age = 30
             },
             new Employee
             {
                 Id = Guid.NewGuid(),
                 Name = "Evelin",
                 AccountNumber = "123-9384613085-55",
                 Age = 28
             }
            );
        */
    }

    public DbSet<Employee>? Employees { get; set; }
}

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasData(
            new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Mark",
                AccountNumber = "123-3452134543-32",
                Age = 30
            },
            new Employee
            {
                Id = Guid.NewGuid(),
                Name = "Evelin",
                AccountNumber = "123-9384613085-55",
                Age = 28
            });
    }
}