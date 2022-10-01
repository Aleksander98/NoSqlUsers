using Microsoft.AspNetCore.Mvc;
using MyEmployees.Application.Services;
using MyEmployees.Domain.Models.Common;
using MyEmployees.WebApi.Contracts.Requests;
using MyEmployees.WebApi.Mapping;

namespace MyEmployees.WebApi.Controllers;

[ApiController]
[Route("employees")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] EmployeeRequest employeeRequest)
    {
        var employee = employeeRequest.ToEmployee();

        await _employeeService.CreateAsync(employee, HttpContext.RequestAborted);

        var response = employee.ToEmployeeResponse();

        return CreatedAtAction("Get", new { response.Username }, response);
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetAsync([FromRoute] string username)
    {
        var employee = await _employeeService
            .GetByUsernameAsync(Username.From(username), HttpContext.RequestAborted);

        if (employee is null)
        {
            return NotFound();
        }

        return Ok(employee.ToEmployeeResponse());
    }
    
    [HttpPut("{username}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] string username, [FromBody] EmployeeRequest employeeRequest)
    {
        if (await _employeeService.GetByUsernameAsync(Username.From(username)) is null)
        {
            return NotFound();
        }
        
        var employee = employeeRequest.ToEmployee();

        await _employeeService.UpdateAsync(employee, HttpContext.RequestAborted);

        var response = employee.ToEmployeeResponse();

        return Ok(response);
    }

    [HttpDelete("{username}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string username)
    {
        var deleted = await _employeeService.DeleteAsync(Username.From(username), HttpContext.RequestAborted); 
        
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}