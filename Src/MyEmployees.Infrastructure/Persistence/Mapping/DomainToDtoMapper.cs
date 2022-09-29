using MyEmployees.Domain.Models;
using MyEmployees.Infrastructure.Persistence.Dtos;

namespace MyEmployees.Infrastructure.Persistence.Mapping;

public static class DomainToDtoMapper
{
    public static EmployeeDto ToEmployeeDto(this Employee employee) => new()
    {
        Username = employee.Username.Value,
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        DateOfBirth = employee.DateOfBirth.ToDateTime(TimeOnly.MinValue),
        Email = employee.Email.Value, 
        ManagerUsername = employee.ManagerUsername?.Value
    };

    public static ManagerDto ToManagerDto(this Manager manager) => new()
    {
        Username = manager.Username.Value,
        FirstName = manager.FirstName,
        LastName = manager.LastName,
        DateOfBirth = manager.DateOfBirth.ToDateTime(TimeOnly.MinValue),
        Email = manager.Email.Value
    };
}