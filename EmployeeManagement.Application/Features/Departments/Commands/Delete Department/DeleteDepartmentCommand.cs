using EmployeeManagement.Application.Common.Results;
using MediatR;

namespace EmployeeManagement.Application.Features.Departments.Commands.DeleteDepartment;

public record DeleteDepartmentCommand(
    int Id) : IRequest<ApiResponse<bool>>;

