using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Application.DTOs;
namespace EmployeeManagement.Application.Abstractions.Persistence;

public interface IEmployeeRepository
{
    Task<int> CreateAsync(EmployeeDto employee,CancellationToken cancellationToken);

    Task<IReadOnlyList<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<EmployeeDto?> GetByIdAsync(int id,CancellationToken cancellationToken);

    //Task<bool> UpdateAsync(Employee employee, CancellationToken cancellationToken);
    Task<int> UpdateAsync(EmployeeDto employee, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken);
    Task<EmployeeDto?> GetEmployeeName(int id, CancellationToken cancellationToken);

}