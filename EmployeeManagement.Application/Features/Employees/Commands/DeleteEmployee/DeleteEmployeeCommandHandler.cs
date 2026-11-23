using EmployeeManagement.Application.Abstractions.Persistence;
using EmployeeManagement.Application.Exceptions;
using MediatR;

namespace EmployeeManagement.Application.Features.Employees.Commands.DeleteEmployee;

public class DeleteEmployeeCommandHandler
    : IRequestHandler<DeleteEmployeeCommand>
{
    private readonly IEmployeeRepository _repository;

    public DeleteEmployeeCommandHandler(
        IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(
        DeleteEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        var deleted = await _repository.DeleteAsync(
            request.Id,
            cancellationToken);

        if (!deleted)
        {
            throw new NotFoundException(
                $"Employee with id {request.Id} was not found.");
        }
    }
}