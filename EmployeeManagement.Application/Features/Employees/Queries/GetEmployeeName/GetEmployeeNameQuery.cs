using EmployeeManagement.Application.Common.Results;
using EmployeeManagement.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.Features.Employees.Queries.GetEmployeeName
{
    public record GetEmployeeNameQuery(int Id, CancellationToken cancellationToken) : IRequest<ApiResponse<IReadOnlyList<EmployeeDto>>>;
}
