using EmployeeManagement.Application.Abstractions.Persistence;
using EmployeeManagement.Application.Common.Results;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Domain.Entities;
using MediatR;
using System.Security.Cryptography.X509Certificates;

namespace EmployeeManagement.Application.Features.Employees.Queries.GetEmployeeName;

public sealed class GetEmployeeNameQueryHandler
    : IRequestHandler<
        GetEmployeeNameQuery,
        ApiResponse<IReadOnlyList<EmployeeDto>>>
{
    private readonly IEmployeeRepository _repository;

    public GetEmployeeNameQueryHandler(
        IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<IReadOnlyList<EmployeeDto>>> Handle(
        GetEmployeeNameQuery request,
        CancellationToken cancellationToken)
    {
        var employees = await _repository.GetAllAsync(
            cancellationToken);


        var result = employees
            .Select(x => new EmployeeDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Salary = x.Salary,
                DateOfBirth = x.DateOfBirth,
                DateOfJoining = x.DateOfJoining,
                DepartmentId = x.DepartmentId,
                DepartmentName = x.DepartmentName
            })
            .ToList();

        return ApiResponse<IReadOnlyList<EmployeeDto>>.SuccessResponse(
            result,
            "Employees retrieved successfully.");
    }
}

