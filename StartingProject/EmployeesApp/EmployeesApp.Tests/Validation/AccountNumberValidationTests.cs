using EmployeesApp.Models;
using EmployeesApp.Validation;
using FluentValidation;
using FluentValidation.Results;

namespace EmployeesApp.Tests.Validation;

public class AccountNumberValidationTests
{
    private readonly IValidator<Employee> _employeeValidator;

    public AccountNumberValidationTests() =>
        _employeeValidator = new EmployeeValidator();

    [Theory]
    [InlineData("123-4543234576-23")]
    //         [MethodUnderTest_StateUnderTest_____ExpectedBehavior]
    public void IsValid_________ValidAccountNumber_ReturnsTrue(string accountNumber)
    {
        // Arrange
        Employee employee = CreateEmployee(accountNumber);

        // Act
        ValidationResult validationResult = _employeeValidator.Validate(employee);

        // Assert
        Assert.True(validationResult.IsValid);
    }

    [Theory]
    [InlineData("1234-3454565676-23")]
    [InlineData("12-3454565676-23")]
    public void IsValid_AccountNumberFirstPartWrong_ReturnsFalse(string accountNumber)
    {
        // Arrange
        Employee employee = CreateEmployee(accountNumber);

        // Act
        ValidationResult validationResult = _employeeValidator.Validate(employee);

        // Assert
        Assert.False(validationResult.IsValid);
    }

    [Theory]
    [InlineData("123-345456567-23")]
    [InlineData("123-345456567633-23")]
    public void IsValid_AccountNumberMiddlePartWrong_ReturnsFalse(string accountNumber)
    {
        // Arrange
        Employee employee = CreateEmployee(accountNumber);

        // Act
        ValidationResult validationResult = _employeeValidator.Validate(employee);

        // Assert
        Assert.False(validationResult.IsValid);
    }

    [Theory]
    [InlineData("123-3434545656-2")]
    [InlineData("123-3454565676-233")]
    public void IsValid_AccountNumberLastPartWrong_ReturnsFalse(string accountNumber)
    {
        // Arrange
        Employee employee = CreateEmployee(accountNumber);

        // Act
        ValidationResult validationResult = _employeeValidator.Validate(employee);

        // Assert
        Assert.False(validationResult.IsValid);
    }

    [Theory]
    [InlineData("1233434545656-2")]
    [InlineData("123+3454565676-23")]
    [InlineData("123-3454565676=23")]
    public void IsValid_InvalidDelimiters_ReturnsFalse(string accountNumber)
    {
        // Arrange
        Employee employee = CreateEmployee(accountNumber);

        // Act
        ValidationResult validationResult = _employeeValidator.Validate(employee);

        // Assert
        Assert.False(validationResult.IsValid);
    }

    private static Employee CreateEmployee(string accountNumber) =>
        new()
        {
            AccountNumber = accountNumber
        };
}