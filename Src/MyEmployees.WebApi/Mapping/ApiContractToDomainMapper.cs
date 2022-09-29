using MyEmployees.Domain.Models;
using MyEmployees.Domain.Models.Common;
using MyEmployees.WebApi.Contracts.Requests;

namespace MyEmployees.WebApi.Mapping;

public static class ApiContractToDomainMapper
{
    public static Employee ToEmployee(this EmployeeRequest request) => new()
    {
        Username = Username.From(request.Username),
        FirstName = request.FirstName,
        LastName = request.LastName,
        DateOfBirth = DateOnly.FromDateTime(request.DateOfBirth),
        Email = Email.From(request.Email),
        ManagerUsername = request.ManagerUsername is not null
            ? Username.From(request.ManagerUsername)
            : null
    };
    
    public static Manager ToManager(this ManagerRequest request) => new()
    {
        Username = Username.From(request.Username),
        FirstName = request.FirstName,
        LastName = request.LastName,
        DateOfBirth = DateOnly.FromDateTime(request.DateOfBirth),
        Email = Email.From(request.Email)
    };
}