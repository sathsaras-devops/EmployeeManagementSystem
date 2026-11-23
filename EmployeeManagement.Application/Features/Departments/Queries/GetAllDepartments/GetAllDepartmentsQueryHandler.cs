using EmployeeManagement.Application.Abstractions.Persistence;
using EmployeeManagement.Application.DTOs;
using MediatR;

namespace EmployeeManagement.Application.Features.Departments.Queries.GetAllDepartments;

public class GetAllDepartmentsQueryHandler
    : IRequestHandler<
        GetAllDepartmentsQuery,
        IReadOnlyList<DepartmentDto>>
{
    private readonly IDepartmentRepository _repository;

    public GetAllDepartmentsQueryHandler(
        IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<DepartmentDto>> Handle(
        GetAllDepartmentsQuery request,
        CancellationToken cancellationToken)
    {
        var departments = await _repository.GetAllDepartment(
            cancellationToken);

        return departments
            .Select(x => new DepartmentDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            })
            .ToList();
    }
}