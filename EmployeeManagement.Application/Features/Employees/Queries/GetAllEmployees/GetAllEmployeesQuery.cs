using EmployeeManagement.Application.Common.Results;
using EmployeeManagement.Application.DTOs;
using MediatR;

namespace EmployeeManagement.Application.Features.Employees.Queries.GetAllEmployees;

public record GetAllEmployeesQuery : IRequest<ApiResponse<IReadOnlyList<EmployeeDto>>>;
