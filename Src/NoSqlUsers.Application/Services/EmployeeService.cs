using FluentValidation;
using FluentValidation.Results;
using MyEmployees.Application.Repositories;
using MyEmployees.Domain.Models;
using MyEmployees.Domain.Models.Common;

namespace MyEmployees.Application.Services;

public sealed class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
    }

    public async Task<bool> CreateAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        if (await _employeeRepository.GetByUsernameAsync(employee.Username, cancellationToken) is not null)
        {
            var message = $"Username '{employee.Username}' is already taken.";
            
            throw new ValidationException(message, GenerateValidationFailure(message));
        }

        return await _employeeRepository.CreateAsync(employee, cancellationToken);
    }

    public Task<Employee?> GetByUsernameAsync(Username username, CancellationToken cancellationToken = default)
    {
        return _employeeRepository.GetByUsernameAsync(username, cancellationToken);
    }

    public Task<bool> DeleteAsync(Username username, CancellationToken cancellationToken = default)
    {
        return _employeeRepository.DeleteAsync(username, cancellationToken);
    }

    private static IEnumerable<ValidationFailure> GenerateValidationFailure(string message) => new[]
    {
        new ValidationFailure(nameof(Employee), message)
    };
}