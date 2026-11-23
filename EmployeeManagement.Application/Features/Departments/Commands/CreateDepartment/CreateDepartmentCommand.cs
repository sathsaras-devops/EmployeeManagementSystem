using EmployeeManagement.Application.Common.Results;
using MediatR;

namespace EmployeeManagement.Application.Features.Departments.Commands.CreateDepartment;

public record CreateDepartmentCommand(
    string Name,
    string? Description) : IRequest<ApiResponse<int>>;