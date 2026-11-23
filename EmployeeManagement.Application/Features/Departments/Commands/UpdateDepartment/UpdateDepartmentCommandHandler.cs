using EmployeeManagement.Application.Abstractions.Persistence;
using EmployeeManagement.Application.Common.Results;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Domain.Entities;
using MediatR;

namespace EmployeeManagement.Application.Features.Departments.Commands.UpdateDepartment;
public class UpdateDepartmentCommandHandler
    : IRequestHandler<
        UpdateDepartmentCommand,
        ApiResponse<bool>>
{
    private readonly IDepartmentRepository _repository;

    public UpdateDepartmentCommandHandler(
        IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<bool>> Handle(
        UpdateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var existing = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (existing is null)
        {
            return ApiResponse<bool>.ErrorResponse(
                "Department not found.",
                new[]
                {
                    $"Department with ID {request.Id} was not found."
                });
        }

        var department = new DepartmentDto
        {
            Id = request.Id,
            Name = request.Name.Trim(),
            Description = request.Description?.Trim()
        };

        var result = await _repository.UpdateDepartment(
            department,
            cancellationToken);

        if (result == 2)
        {
            return ApiResponse<bool>.ErrorResponse(
                "Department could not be updated.",
                new[]
                {
                    $"A department with this name already exists."
                });
        }

        if (result == -1)
        {
            return ApiResponse<bool>.ErrorResponse(
                "Department Not Found.",
                new[]
                {
                    $"Department with ID {request.Id} was not found."
                });
        }

        return ApiResponse<bool>.SuccessResponse(
            true,
            "Department updated successfully.");
    }
}

