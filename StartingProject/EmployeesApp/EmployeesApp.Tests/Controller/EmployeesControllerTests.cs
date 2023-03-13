using EmployeesApp.Contracts;
using EmployeesApp.Controllers;
using EmployeesApp.Models;
using EmployeesApp.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeesApp.Tests.Controller;

public class EmployeesControllerTests
{
    private readonly Mock<IEmployeeRepository> _mockRepo;
    private readonly IValidator<Employee> _employeeValidator;
    private readonly EmployeesController _controller;

    public EmployeesControllerTests()
    {
        _mockRepo = new Mock<IEmployeeRepository>();
        _employeeValidator = new EmployeeValidator();
        _controller = new EmployeesController(_mockRepo.Object, _employeeValidator);
    }

    [Fact]
    //         [MethodUnderTest_StateUnderTest_ExpectedBehavior]
    public void Index___________ActionExecutes_ReturnsViewForIndex()
    {
        // Arrange, Act
        IActionResult result = _controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    
    public void Index_ActionExecutes_ReturnsExactNumberOfEmployees()
    {
        // Arrange
        _mockRepo.Setup((IEmployeeRepository repo) => repo.GetAll())
            .Returns(new List<Employee> { new Employee(), new Employee() });

        // Act
        IActionResult result = _controller.Index();

        // Assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        IList<Employee> employees = Assert.IsType<List<Employee>>(viewResult.Model);
        Assert.Equal(2, employees.Count);
    }

    [Fact]
    public void Create_ActionExecutes_ReturnsViewForCreate()
    {
        // Arrange, Act
        IActionResult result = _controller.Create();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Create_EmployeeWithNoNameWithInvalidAccountNumber_ReturnsView()
    {
        // Arrange
        var employee = new Employee { Age = 25, AccountNumber = "2255-8547963214-41" };

        // Act
        IActionResult result = _controller.Create(employee);

        // Assert
        ViewResult viewResult = Assert.IsType<ViewResult>(result);
        Employee testEmployee = Assert.IsType<Employee>(viewResult.Model);

        Assert.Single(_controller.ModelState);
        Assert.Equal(employee.AccountNumber, testEmployee.AccountNumber);
        Assert.Equal(employee.Age, testEmployee.Age);
    }

    [Fact]
    public void Create_EmployeeWithEmptyAccountNumber_CreateEmployeeNeverExecutes()
    {
        // Arrange
        var employee = new Employee { Age = 34, AccountNumber = string.Empty };

        // Act
        _controller.Create(employee);

        // Assert
        _mockRepo.Verify((IEmployeeRepository er) => er.CreateEmployee(It.IsAny<Employee>()), Times.Never);
    }

    [Fact]
    public void Create_ModelStateValid_CreateEmployeeCalledOnce()
    {
        // Arrange
        Employee? emp = null;

        _mockRepo.Setup((IEmployeeRepository er) => er.CreateEmployee(It.IsAny<Employee>()))
            .Callback((Employee e) => emp = e); // Interesting, no?

        var employee = new Employee
        {
            Name = "Test Employee",
            Age = 32,
            AccountNumber = "123-5435789603-21"
        };

        // Act
        _controller.Create(employee);

        // Assert
        _mockRepo.Verify((IEmployeeRepository er) => er.CreateEmployee(It.IsAny<Employee>()), Times.Once);

        Assert.Equal(emp!.Name, employee.Name);
        Assert.Equal(emp.Age, employee.Age);
        Assert.Equal(emp.AccountNumber, employee.AccountNumber);
    }

    [Fact]
    public void Create_ActionExecuted_RedirectsToIndexAction()
    {
        // Arrange
        var employee = new Employee
        {
            Name = "Test Employee",
            Age = 45,
            AccountNumber = "123-4356874310-43"
        };

        // Act
        var result = _controller.Create(employee);

        // Assert
        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("Index", redirectToActionResult.ActionName);
    }
}