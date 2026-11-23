using EmployeeManagement.Application.Abstractions.Persistence;
using EmployeeManagement.Application.Common.Results;
using EmployeeManagement.Application.DTOs;
using MediatR;

namespace EmployeeManagement.Application.Features.Employees.Queries.GetEmployeeById;

public sealed class GetEmployeeByIdQueryHandler
    : IRequestHandler<
        GetEmployeeByIdQuery,
        ApiResponse<EmployeeDto>>
{
    private readonly IEmployeeRepository _repository;

    public GetEmployeeByIdQueryHandler(
        IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<EmployeeDto>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _repository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (employee is null)


        {
            return ApiResponse<EmployeeDto>.ErrorResponse(
                "Employee not found.",
                new[]
                {
                    $"Employee with ID {request.Id} was not found."
                });
        }
        Console.WriteLine(employee);

        var result = new EmployeeDto
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Salary = employee.Salary,
            DateOfBirth = employee.DateOfBirth,
            DateOfJoining = employee.DateOfJoining,
            DepartmentId = employee.DepartmentId,
            DepartmentName = employee.DepartmentName
        };

        return ApiResponse<EmployeeDto>.SuccessResponse(
            result,
            "Employee retrieved successfully.");
    }
}
