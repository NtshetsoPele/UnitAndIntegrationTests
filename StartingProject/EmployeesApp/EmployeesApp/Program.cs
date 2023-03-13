using EmployeesApp.Contracts;
using EmployeesApp.Extensions;
using EmployeesApp.Models;
using EmployeesApp.Repository;
using EmployeesApp.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EmployeeContext>(opts =>
			   opts.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<IValidator<Employee>, EmployeeValidator>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Employees}/{action=Index}/{id?}");

app.MigrateDatabase();
app.Run();


#if DEBUG
// The .NET 6 compiler generates the Program class behind the scenes as an internal class,
// thus making it inaccessible in our integration testing project.
public partial class Program { }
#endif