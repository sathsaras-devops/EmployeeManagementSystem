using EmployeeManagement.Application.Abstractions.Persistence;
using EmployeeManagement.Application.Common.Results;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.Features.Departments.Commands.DeleteDepartment;

public class DeleteDepartmentCommandHandler
    : IRequestHandler<
        DeleteDepartmentCommand,
        ApiResponse<bool>>
{
    private readonly IDepartmentRepository _repository;
    private readonly ILogger<DeleteDepartmentCommandHandler> _logger;

    public DeleteDepartmentCommandHandler(
        IDepartmentRepository repository,
        ILogger<DeleteDepartmentCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ApiResponse<bool>> Handle(
        DeleteDepartmentCommand request,
        CancellationToken cancellationToken)
    {

        try
        {
            await _repository.DeleteDepartment(
                request.Id,
                cancellationToken);

            _logger.LogInformation(
                "Department deleted successfully. DepartmentId: {DepartmentId}",
                request.Id);

            return ApiResponse<bool>.SuccessResponse(
                true,
                "Department deleted successfully.");
        }
        catch (SqlException ex)
        {
            _logger.LogError(
                ex,
                "Database error while deleting department. DepartmentId: {DepartmentId}",
                request.Id);

            return ApiResponse<bool>.ErrorResponse(
                "Department could not be deleted.",
                new[]
                {
                    ex.Message
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error while deleting department. DepartmentId: {DepartmentId}",
                request.Id);

            return ApiResponse<bool>.ErrorResponse(
                "Department could not be deleted.",
                new[]
                {
                    "An unexpected error occurred while deleting the department."
                });
        }
    }
}



