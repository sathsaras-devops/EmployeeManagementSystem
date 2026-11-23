using EmployeeManagement.Application.Common.Results;
using MediatR;

namespace EmployeeManagement.Application.Features.Departments.Commands.UpdateDepartment;

public record UpdateDepartmentCommand(
    int Id,
    string Name,
    string? Description) : IRequest<ApiResponse<bool>>;

