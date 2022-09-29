using MyEmployees.Domain.Models;
using MyEmployees.WebApi.Contracts.Responses;

namespace MyEmployees.WebApi.Mapping;

public static class DomainToApiContractMapper
{
    public static EmployeeResponse ToEmployeeResponse(this Employee employee) => new()
    {
        Username = employee.Username.Value,
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        DateOfBirth = employee.DateOfBirth.ToDateTime(TimeOnly.MinValue),
        Email = employee.Email.Value, 
        ManagerUsername = employee.ManagerUsername?.Value
    };

    public static ManagerResponse ToManagerResponse(this Manager manager) => new()
    {
        Username = manager.Username.Value,
        FirstName = manager.FirstName,
        LastName = manager.LastName,
        DateOfBirth = manager.DateOfBirth.ToDateTime(TimeOnly.MinValue),
        Email = manager.Email.Value
    };
}