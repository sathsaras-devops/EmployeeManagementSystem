using EmployeeManagement.Application.Common.Results;
using EmployeeManagement.Application.DTOs;
using MediatR;

namespace EmployeeManagement.Application.Features.Employees.Queries.GetEmployeeById;

public record GetEmployeeByIdQuery(int Id) : IRequest<ApiResponse<EmployeeDto>>;


