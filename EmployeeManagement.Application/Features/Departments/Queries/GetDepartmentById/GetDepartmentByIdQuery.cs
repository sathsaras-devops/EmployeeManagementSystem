using EmployeeManagement.Application.Common.Results;
using EmployeeManagement.Application.DTOs;
using MediatR;

namespace EmployeeManagement.Application.Features.Departments.Queries.GetDepartmentById;

public record GetDepartmentByIdQuery(
    int Id) : IRequest<ApiResponse<DepartmentDto>>;

