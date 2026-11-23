using EmployeeManagement.Domain.Entities;
namespace EmployeeManagement.Application.Abstractions.Persistence;
using EmployeeManagement.Application.DTOs;

public interface IDepartmentRepository
{
    Task<int> CreateDepartment(DepartmentDto department,CancellationToken cancellationToken);
    Task<IReadOnlyList<DepartmentDto>> GetAllDepartment(CancellationToken cancellationToken);
    Task<DepartmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    //Task<bool> UpdateDepartment(Department department, CancellationToken cancellationToken);
    Task<int> UpdateDepartment(DepartmentDto department, CancellationToken cancellationToken);
    Task<int> DeleteDepartment(int id, CancellationToken cancellationToken);
}