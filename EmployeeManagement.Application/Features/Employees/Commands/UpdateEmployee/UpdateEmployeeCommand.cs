using EmployeeManagement.Application.Common.Results;
using MediatR;

namespace EmployeeManagement.Application.Features.Employees.Commands.UpdateEmployee;

public record UpdateEmployeeCommand(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    decimal Salary,
    DateTime DateOfBirth,
    DateTime DateOfJoining,
    int DepartmentId) : IRequest<ApiResponse<bool>>;
