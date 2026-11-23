using EmployeeManagement.API.Contracts.Employees;
using EmployeeManagement.Application.Common.Results;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Application.Features.Employees.Commands.CreateEmployee;
using EmployeeManagement.Application.Features.Employees.Commands.DeleteEmployee;
using EmployeeManagement.Application.Features.Employees.Commands.UpdateEmployee;
using EmployeeManagement.Application.Features.Employees.Queries.GetAllEmployees;
using EmployeeManagement.Application.Features.Employees.Queries.GetEmployeeById;
using EmployeeManagement.Application.Features.Employees.Queries.GetEmployeeName;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<EmployeesController> _logger;

    public EmployeesController(
        IMediator mediator,
        ILogger<EmployeesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<ActionResult<ApiResponse<int>>> Create(
        CreateEmployeeRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creating employee with email {Email}",
            request.Email);

        var result = await _mediator.Send(
            new CreateEmployeeCommand(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Salary,
                request.DateOfBirth,
                request.DateOfJoining,
                request.DepartmentId),
            cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Data },
            result);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<DepartmentDto>>>> GetAll(CancellationToken cancellationToken)
    {
        _logger.LogInformation (" Getting the All daparment");
         var result = await _mediator.Send(new GetAllEmployeesQuery(), cancellationToken);
        return Ok(result);

    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<EmployeeDto>>> GetById(int id, CancellationToken cancellationToken) {

        _logger.LogInformation("Retrieving employee with Id {Id}", id);

        var result = await _mediator.Send(new GetEmployeeByIdQuery(id), cancellationToken);

        if (!result.Success)
        {
            return NotFound(result);

        }
        return Ok(result);

    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> Update(int id, UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating employee id {id}", id);

        var result = await _mediator.Send(
            new UpdateEmployeeCommand(
                id,
                request.FirstName,
                request.LastName,
                request.Email,
                request.Salary,
                request.DateOfBirth,
                request.DateOfJoining,
                request.DepartmentId),
            cancellationToken);

        if (!result.Success) {
            return BadRequest(result);
        }
        return Ok(result);
    }

    //[HttpGet("{id:int}")]
    //public async Task<ActionResult<ApiResponse<IReadOnlyList<EmployeeDto>>>> GetEmployeeName(int id, CancellationToken cancellationToken) {
    //    _logger.LogInformation("Getting employee name {id}", id);
        
    //    var result = await _mediator.Send(new GetEmployeeNameQuery(id, cancellationToken));

    //    if (!result.Success) {
    //        return BadRequest(result);
    //    }
    //    return Ok(result);
    //}

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteEmployeeCommand(id),
            cancellationToken);

        return NoContent();
    }
}