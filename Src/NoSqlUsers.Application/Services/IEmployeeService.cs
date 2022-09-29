using MyEmployees.Domain.Models;
using MyEmployees.Domain.Models.Common;

namespace MyEmployees.Application.Services;

public interface IEmployeeService
{
    Task<bool> CreateAsync(Employee employee, CancellationToken cancellationToken = default);

    Task<Employee?> GetByUsernameAsync(Username username, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Username username, CancellationToken cancellationToken = default);
}