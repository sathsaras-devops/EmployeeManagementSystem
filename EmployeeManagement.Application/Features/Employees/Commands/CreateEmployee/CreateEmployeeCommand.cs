using EmployeeManagement.Application.Common.Results;
using MediatR;

namespace EmployeeManagement.Application.Features.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand(
    string FirstName,
    string LastName,
    string Email,
    decimal Salary,
    DateTime DateOfBirth,
    DateTime DateOfJoining,
    int DepartmentId) : IRequest<ApiResponse<int>>;
