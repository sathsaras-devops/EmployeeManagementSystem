using EmployeeManagement.Application.Abstractions.Persistence;
using EmployeeManagement.Application.Common.Results;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Domain.Entities;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.Features.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler
    : IRequestHandler<CreateDepartmentCommand, ApiResponse<int>>
{
    private readonly IDepartmentRepository _repository;
    private readonly ILogger<CreateDepartmentCommandHandler> _logger;

    public CreateDepartmentCommandHandler(
        IDepartmentRepository repository,
        ILogger<CreateDepartmentCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task<ApiResponse<int>> Handle(
    CreateDepartmentCommand request,
    CancellationToken cancellationToken)
    {
        var department = new DepartmentDto
        {
            Name = request.Name.Trim(),
            Description = request.Description?.Trim()
        };

        try
        {
            var result = await _repository.CreateDepartment(
                department,
                cancellationToken);

            _logger.LogInformation(
                "Department created successfully. DepartmentId: {DepartmentId}",
                result);

            return ApiResponse<int>.SuccessResponse(
                result,
                "Department created successfully.");
        }
        catch (SqlException ex)
        {
            _logger.LogError(
                ex,
                "Database error while creating department. DepartmentName: {DepartmentName}",
                department.Name);

            return ApiResponse<int>.ErrorResponse(
                "Department could not be created.",
                new[]
                {
                    ex.Message
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while creating department.");

            return ApiResponse<int>.ErrorResponse(
                "Department could not be created.",
                new[]
                {
                    "An unexpected error occurred while creating the department."
                });
        }
    }
}