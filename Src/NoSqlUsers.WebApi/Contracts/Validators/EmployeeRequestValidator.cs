using FluentValidation;
using MyEmployees.WebApi.Contracts.Requests;

namespace MyEmployees.WebApi.Contracts.Validators;

public sealed class EmployeeRequestValidator : AbstractValidator<EmployeeRequest>
{
    public EmployeeRequestValidator()
    {
        RuleFor(request => request.Username).NotEmpty();

        RuleFor(request => request.FirstName).NotEmpty();

        RuleFor(request => request.LastName).NotEmpty();

        RuleFor(request => request.Email).EmailAddress();

        RuleFor(request => request.DateOfBirth).NotEmpty();
    }
}