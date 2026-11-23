using EmployeeManagement.Application.Abstractions.Persistence;
using EmployeeManagement.Application.Common.Results;
using EmployeeManagement.Application.DTOs;
using MediatR;

namespace EmployeeManagement.Application.Features.Departments.Queries.GetDepartmentById;

public class GetDepartmentByIdQueryHandler
    : IRequestHandler<
        GetDepartmentByIdQuery,
        ApiResponse<DepartmentDto>>
{
    private readonly IDepartmentRepository _repository;

    public GetDepartmentByIdQueryHandler(
        IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<DepartmentDto>> Handle(
        GetDepartmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var department = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (department is null)
        {
            return ApiResponse<DepartmentDto>.ErrorResponse(
                "Department not found.",
                [
                    $"Department with ID {request.Id} was not found."
                ]);
        }

        var departmentDto = new DepartmentDto
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description
        };

        return ApiResponse<DepartmentDto>.SuccessResponse(
            departmentDto,
            "Department retrieved successfully.");
    }
}