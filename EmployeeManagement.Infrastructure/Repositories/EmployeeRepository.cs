using System.Data;
using Dapper;
using EmployeeManagement.Application.Abstractions.Persistence;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Persistence;
using EmployeeManagement.Application.DTOs;

namespace EmployeeManagement.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly IDbConnectionFactory _connectionFactory;

    public EmployeeRepository(IDbConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<int> CreateAsync(EmployeeDto employee, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();

        parameters.Add("@FirstName", employee.FirstName);
        parameters.Add("@LastName", employee.LastName);
        parameters.Add("@Email", employee.Email);
        parameters.Add("@Salary", employee.Salary);
        parameters.Add("@DateOfBirth", employee.DateOfBirth);
        parameters.Add("@DateOfJoining", employee.DateOfJoining);
        parameters.Add("@DepartmentId", employee.DepartmentId);

        var result = await connection.QuerySingleAsync<int>(
        new CommandDefinition(
            "dbo.EmployeeCreate",
            parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));

        return result;
    }

    public async Task<IReadOnlyList<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        var result = await connection.QueryAsync<EmployeeDto>(
            new CommandDefinition(
                "sp_Employee_GetAll",
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));

        return result.AsList();
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();

        parameters.Add("@Id", id);

        var result = await connection.QuerySingleOrDefaultAsync<EmployeeDto>(
            new CommandDefinition(
                "sp_Employee_GetById",
                parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));
        return result;
    }

    public async Task<int> UpdateAsync(EmployeeDto employee, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();

        parameters.Add("@Id", employee.Id);
        parameters.Add("@FirstName", employee.FirstName);
        parameters.Add("@LastName", employee.LastName);
        parameters.Add("@Email", employee.Email);
        parameters.Add("@Salary", employee.Salary);
        parameters.Add("@DateOfBirth", employee.DateOfBirth);
        parameters.Add("@DateOfJoining", employee.DateOfJoining);
        parameters.Add("@DepartmentId", employee.DepartmentId);

        var result = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                "sp_Employee_Update",
                parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));

        return result;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        using var connection = _connectionFactory.CreateConnection();

        var parameters = new DynamicParameters();

        parameters.Add("@Id", id);

        var rows = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                "sp_Employee_Delete",
                parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken));

        return rows > 0;
    }

    public Task<EmployeeDto?> GetEmployeeName(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
