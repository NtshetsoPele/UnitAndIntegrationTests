using EmployeesApp.Models;
using FluentValidation;

namespace EmployeesApp.Validation;

public class EmployeeValidator : AbstractValidator<Employee>
{
    private const int StartingPartLength = 3;
    private const int MiddlePartLength = 10;
    private const int LastPartLength = 2;

    public EmployeeValidator()
    {
        RuleFor((Employee e) => e.AccountNumber)
            .NotEmpty()
            .Must(HaveValidAccountNumber)
            .WithMessage("Employee account numbers must conform to the following form: 'DDD-DDDDDDDDDD-DD'.");
    }

    private bool HaveValidAccountNumber(string accountNumber)
    {
        int firstDelimiter = accountNumber.IndexOf('-');
        int secondDelimiter = accountNumber.LastIndexOf('-');

        if (firstDelimiter == -1 || firstDelimiter == secondDelimiter)
        {
            return false;
        }

        ReadOnlySpan<char> firstPart = accountNumber.AsSpan()[..firstDelimiter];
        if (firstPart.Length != StartingPartLength)
        {
            return false;
        }

        ReadOnlySpan<char> tempPart = accountNumber.AsSpan()[(StartingPartLength + 1)..];
        ReadOnlySpan<char> middlePart = tempPart[..tempPart.IndexOf('-')];
        if (middlePart.Length != MiddlePartLength)
        {
            return false;
        }

        ReadOnlySpan<char> lastPart = accountNumber.AsSpan()[(secondDelimiter + 1)..];
        if (lastPart.Length != LastPartLength)
        {
            return false;
        }

        return true;
    }
}
