using MyEmployees.Domain.Models;
using MyEmployees.Domain.Models.Common;

namespace MyEmployees.Application.Repositories;

public interface IEmployeeRepository
{
    Task<bool> CreateAsync(Employee employee, CancellationToken cancellationToken = default);

    Task<Employee?> GetByUsernameAsync(Username username, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Username username, CancellationToken cancellationToken = default);
}