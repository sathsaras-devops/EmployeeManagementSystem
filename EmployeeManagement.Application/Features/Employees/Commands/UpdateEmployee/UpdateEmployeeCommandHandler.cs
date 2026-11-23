using EmployeeManagement.Application.Abstractions.Persistence;
using EmployeeManagement.Application.Common.Results;
using EmployeeManagement.Application.Features.Employees.Commands.CreateEmployee;
using EmployeeManagement.Domain.Entities;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using EmployeeManagement.Application.DTOs;

namespace EmployeeManagement.Application.Features.Employees.Commands.UpdateEmployee;

public class UpdateEmployeeCommandHandler
    : IRequestHandler<
        UpdateEmployeeCommand,
        ApiResponse<bool>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILogger<CreateEmployeeCommandHandler> _logger;


    public UpdateEmployeeCommandHandler(
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository,
        ILogger<CreateEmployeeCommandHandler> logger)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _logger = logger;
    }

    public async Task<ApiResponse<bool>> Handle(
        UpdateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await _employeeRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (existing is null)
        {
            return ApiResponse<bool>.ErrorResponse(
                "Employee not found.",
                new[]
                {
                    $"Employee with ID {request.Id} was not found."
                });
        }

        var department = await _departmentRepository.GetByIdAsync(
            request.DepartmentId,
            cancellationToken);

        if (department is null)
        {
            return ApiResponse<bool>.ErrorResponse(
                "Department not found.",
                new[]
                {
                    $"Department with ID {request.DepartmentId} was not found."
                });
        }

        var employee = new EmployeeDto
        {
            Id = request.Id,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = request.Email.Trim(),
            Salary = request.Salary,
            DateOfBirth = request.DateOfBirth,
            DateOfJoining = request.DateOfJoining,
            DepartmentId = request.DepartmentId
        };

        try
        {
            await _employeeRepository.UpdateAsync(
                employee,
                cancellationToken);

            _logger.LogInformation(
                "Employee updated successfully. EmployeeId: {EmployeeId}",
                request.Id);

            return ApiResponse<bool>.SuccessResponse(
                true,
                "Employee updated successfully.");
        }
        catch (SqlException ex)
        {
            _logger.LogError(
                ex,
                "Database error while updating employee. EmployeeId: {EmployeeId}",
                request.Id);

            return ApiResponse<bool>.ErrorResponse(
                "Employee could not be updated.",
                new[]
                {
                ex.Message
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while updating employee. EmployeeId: {EmployeeId}",
                request.Id);

            return ApiResponse<bool>.ErrorResponse(
                "An unexpected error occurred.");
        }
    }
}

