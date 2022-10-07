using Microsoft.AspNetCore.Mvc;
using MyEmployees.Application.Services;
using MyEmployees.Domain.Models.Common;
using MyEmployees.WebApi.Contracts.Requests;
using MyEmployees.WebApi.Contracts.Responses;
using MyEmployees.WebApi.Mapping;

namespace MyEmployees.WebApi.Controllers;

[ApiController]
[Route("managers")]
public class ManagerController : ControllerBase
{
    private readonly IManagerService _managerService;

    public ManagerController(IManagerService employeeService)
    {
        _managerService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ManagerRequest managerRequest)
    {
        var manager = managerRequest.ToManager();

        await _managerService.CreateAsync(manager, HttpContext.RequestAborted);

        var response = manager.ToManagerResponse();

        return CreatedAtAction("Get", new { response.Username }, response);
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetAsync([FromRoute] string username)
    {
        var manager = await _managerService
            .GetByUsernameAsync(Username.From(username), HttpContext.RequestAborted);

        if (manager is null)
        {
            return NotFound();
        }

        return Ok(manager.ToManagerResponse());
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllWithPaginationAsync([FromQuery] int pageSize, [FromQuery] string? pageToken)
    {
        var paginatedManagers = await _managerService
            .GetAllWithPaginationAsync(pageSize, pageToken, HttpContext.RequestAborted);
        
        var managerResponses = paginatedManagers.Items
            .Select(manager => manager.ToManagerResponse())
            .ToList();
        var paginationResponse = new PaginationResponse(pageSize, paginatedManagers.Pagination.PageToken, paginatedManagers.Pagination.NextPageToken);
        var paginatedListResponse = new PaginatedListResponse<ManagerResponse>(managerResponses, paginationResponse);
        
        return Ok(paginatedListResponse);
    }
    
    [HttpPut("{username}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] string username, [FromBody] ManagerRequest managerRequest)
    {
        if (await _managerService.GetByUsernameAsync(Username.From(username)) is null)
        {
            return NotFound();
        }

        var manager = managerRequest.ToManager();

        await _managerService.UpdateAsync(manager, HttpContext.RequestAborted);

        var response = manager.ToManagerResponse();

        return Ok(response);
    }
    
    [HttpDelete("{username}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string username)
    {
        var deleted = await _managerService.DeleteAsync(Username.From(username), HttpContext.RequestAborted); 
        
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}