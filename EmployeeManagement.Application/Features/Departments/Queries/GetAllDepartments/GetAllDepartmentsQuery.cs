using EmployeeManagement.Application.DTOs;
using MediatR;

namespace EmployeeManagement.Application.Features.Departments.Queries.GetAllDepartments;

public sealed record GetAllDepartmentsQuery
    : IRequest<IReadOnlyList<DepartmentDto>>;