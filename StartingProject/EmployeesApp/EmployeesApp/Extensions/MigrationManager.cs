using EmployeesApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesApp.Extensions;

public static class MigrationManager
{
    public static WebApplication MigrateDatabase(this WebApplication webApp)
    {
        using IServiceScope scope = webApp.Services.CreateScope();
        
        using var appContext = scope.ServiceProvider.GetRequiredService<EmployeeContext>();

        if (appContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
        {
            appContext.Database.Migrate();
        }

        return webApp;
    }
}
