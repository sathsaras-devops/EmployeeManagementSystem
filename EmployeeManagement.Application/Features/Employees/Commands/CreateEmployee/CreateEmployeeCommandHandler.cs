using EmployeeManagement.Application.Abstractions.Persistence;
using EmployeeManagement.Application.Common.Results;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;


namespace EmployeeManagement.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler
    : IRequestHandler<
        CreateEmployeeCommand,
        ApiResponse<int>>
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILogger<CreateEmployeeCommandHandler> _logger;

    public CreateEmployeeCommandHandler(
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository,
        ILogger<CreateEmployeeCommandHandler> logger)
    {
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
        _logger = logger;
    }

    public async Task<ApiResponse<int>> Handle(
        CreateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        // Check whether department exists
        var department = await _departmentRepository.GetByIdAsync(
            request.DepartmentId,
            cancellationToken);

        if (department is null)
        {
            return ApiResponse<int>.ErrorResponse(
                "Department not found.",
                new[]
                {
                    $"Department with ID {request.DepartmentId} was not found."
                });
        }

        var employee = new EmployeeDto
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = request.Email.Trim(),
            Salary = request.Salary,
            DateOfBirth = request.DateOfBirth,
            DateOfJoining = request.DateOfJoining,
            DepartmentId = request.DepartmentId
        };

        try
        {
            var result = await _employeeRepository.CreateAsync(
                employee,
                cancellationToken);

            _logger.LogInformation(
                "Employee created successfully. EmployeeId: {EmployeeId}",
                result);

            return ApiResponse<int>.SuccessResponse(
                result,
                "Employee created successfully.");
        }
        catch (SqlException ex)
        {
            _logger.LogError(
                ex,
                "Database error occurred while creating employee.");

            return ApiResponse<int>.ErrorResponse(
                "Employee could not be created.",
                new[]
                {
            ex.Message
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error occurred while creating employee.");

            return ApiResponse<int>.ErrorResponse(
                "An unexpected error occurred.");
        }
    }
}
