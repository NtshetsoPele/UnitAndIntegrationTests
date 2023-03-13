using EmployeesApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; // IServiceCollection, ServiceDescriptor, ServiceProvider,
//IServiceScope, AddDbCotext<TDbContext>, BuildServiceProvider

namespace EmployeesApp.IntegrationTests;

// Used to bootstrap an application in memory for functional end-to-end tests
public class TestingWebAppFactory<TEntryPoint> : WebApplicationFactory<Program> where TEntryPoint : Program
{
    // Configure the application before it gets built
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices((IServiceCollection services) =>
        {
            //  Remove the EmployeeContext registration from the Program class
            ServiceDescriptor? dbContextOptionsDescriptor = 
                services.SingleOrDefault((ServiceDescriptor serviceDescriptor) =>
            {
                return serviceDescriptor.ServiceType == typeof(DbContextOptions<EmployeeContext>);
            });

            if (dbContextOptionsDescriptor != null)
            {
                services.Remove(dbContextOptionsDescriptor);
            }

            // Add the database context to the service container and instruct it to use the in-memory
            // database instead of the real database
            services.AddDbContext<EmployeeContext>((DbContextOptionsBuilder options) =>
            {
                options.UseInMemoryDatabase("InMemoryEmployeeTest");
            });

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            using IServiceScope serviceScope = serviceProvider.CreateScope();
            using EmployeeContext context = 
                serviceScope.ServiceProvider.GetRequiredService<EmployeeContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated(); // Includes seeding data
        });
    }
}
