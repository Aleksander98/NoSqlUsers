using MyEmployees.Domain.Models;
using MyEmployees.Domain.Models.Common;
using MyEmployees.Infrastructure.Persistence.Dtos;

namespace MyEmployees.Infrastructure.Persistence.Mapping;

public static class DtoToDomainMapper
{
    public static Employee ToEmployee(this EmployeeDto employeeDto) => new()
    {
        Username = Username.From(employeeDto.Username),
        FirstName = employeeDto.FirstName,
        LastName = employeeDto.LastName,
        DateOfBirth = DateOnly.FromDateTime(employeeDto.DateOfBirth),
        Email = Email.From(employeeDto.Email),
        ManagerUsername = employeeDto.ManagerUsername is not null 
            ? Username.From(employeeDto.ManagerUsername) 
            : null
    };

    public static Manager ToManager(this ManagerDto managerDto) => new()
    {
        Username = Username.From(managerDto.Username),
        FirstName = managerDto.FirstName,
        LastName = managerDto.LastName,
        DateOfBirth = DateOnly.FromDateTime(managerDto.DateOfBirth),
        Email = Email.From(managerDto.Email)
    };
}