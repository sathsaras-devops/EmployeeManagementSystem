using EmployeeManagement.Api.Contracts.Departments;
using EmployeeManagement.API.Contracts.Departments;
using EmployeeManagement.Application.Common.Results;
using EmployeeManagement.Application.Features.Departments.Commands.CreateDepartment;
using EmployeeManagement.Application.Features.Departments.Commands.DeleteDepartment;
using EmployeeManagement.Application.Features.Departments.Commands.UpdateDepartment;
using EmployeeManagement.Application.Features.Departments.Queries.GetAllDepartments;
using EmployeeManagement.Application.Features.Departments.Queries.GetDepartmentById;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<DepartmentsController> _logger;

    public DepartmentsController(
        IMediator mediator,
        ILogger<DepartmentsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }


    [HttpPost]
    public async Task<ActionResult<ApiResponse<int>>> Create(
    CreateDepartmentRequest request,
    CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Creating department {Name}",
            request.Name);

        var result = await _mediator.Send(
            new CreateDepartmentCommand(
                request.Name,
                request.Description),
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
    public async Task<ActionResult<ApiResponse<IEnumerable<DepartmentDto>>>> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAllDepartmentsQuery(),
            cancellationToken);

        //return Ok(result);

        return Ok(
        ApiResponse<IEnumerable<DepartmentDto>>
            .SuccessResponse(
                result,
                "Departments retrieved successfully."));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<DepartmentDto>>> GetById(
    int id,
    CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting department with Id {Id}",
            id);

        var result = await _mediator.Send(
            new GetDepartmentByIdQuery(id),
            cancellationToken);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> Update(int id, UpdateDepartmentRequest request,CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Updating department with Id {Id}",
            id);

        var result = await _mediator.Send(
            new UpdateDepartmentCommand(
                id,
                request.Name,
                request.Description),
            cancellationToken);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id,CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Deleting department with Id {Id}",
            id);

        var result = await _mediator.Send(
            new DeleteDepartmentCommand(id),
            cancellationToken);

        if (!result.Success)
        {
            if (result.Errors.Any(error =>
                    error.Contains("Employees are assigned")))
            {
                return Conflict(result);
            }

            return NotFound(result);
        }

        return Ok(result);
    }

}