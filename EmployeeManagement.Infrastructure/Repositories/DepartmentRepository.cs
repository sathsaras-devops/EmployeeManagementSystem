using Dapper;
using EmployeeManagement.Application.Abstractions.Persistence;
using EmployeeManagement.Application.DTOs;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace EmployeeManagement.Infrastructure.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<DepartmentRepository> _logger;

    public DepartmentRepository(IDbConnectionFactory dbConnectionFactory, ILogger<DepartmentRepository> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<int> CreateDepartment(DepartmentDto department,CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        //if (connection.State != ConnectionState.Open)
        //{
        //    connection.Open();
        //}
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            var parameters = new DynamicParameters();

            parameters.Add("@Name", department.Name);
            parameters.Add("@Description", department.Description);

            var departmentId = await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(
                    "sp_Department_Create",
                    parameters,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken
                ));

            transaction.Commit();

            _logger.LogInformation("Department created successfully. DepartmentId: {DepartmentId}",
            departmentId);

            return departmentId;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex,"Error occurred while creating department.");

            transaction.Rollback();

            throw;
        }
    }

    public async Task<IReadOnlyList<DepartmentDto>> GetAllDepartment(
    CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        try
        {
            var result = await connection.QueryAsync<DepartmentDto>(
                new CommandDefinition(
                    "sp_Department_GetAll",
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken));

            var departments = result.AsList();

            _logger.LogInformation("Retrieved {DepartmentCount} departments.", departments.Count);

            return departments;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error occurred while retrieving departments.");

            throw;
        }
    }

    public async Task<DepartmentDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        var parameters = new DynamicParameters();

        parameters.Add("@Id", id);

        return await connection.QuerySingleOrDefaultAsync<DepartmentDto>(
            new CommandDefinition(
                "sp_Department_GetById",
                parameters,
                commandType: CommandType.StoredProcedure,
                cancellationToken: cancellationToken
                ));
    }


    public async Task<int> UpdateDepartment(DepartmentDto department,CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        //if (connection.State != ConnectionState.Open)
        //{
        //    connection.Open();
        //}
        connection.Open();

        using var transaction = connection.BeginTransaction();

        try
        {
            var parameters = new DynamicParameters();

            parameters.Add("@Id", department.Id);
            parameters.Add("@Name", department.Name);
            parameters.Add("@Description", department.Description);

            var rows = await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(
                    "sp_Department_Update",
                    parameters,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken));

            transaction.Commit();

            _logger.LogInformation("Department updated successfully. DepartmentId: {DepartmentId}",department.Id);

            return rows;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error occurred while updating department. DepartmentId: {DepartmentId}",department.Id);

            transaction.Rollback();

            throw;
        }
    }

    public async Task<int> DeleteDepartment(int id,CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.CreateConnection();

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        using var transaction = connection.BeginTransaction();

        try
        {
            var parameters = new DynamicParameters();

            parameters.Add("@Id", id);

            var result = await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(
                    "sp_Department_Delete",
                    parameters,
                    transaction: transaction,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: cancellationToken));

            transaction.Commit();

            _logger.LogInformation("Department deleted successfully. DepartmentId: {DepartmentId}",id);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error occurred while deleting department. DepartmentId: {DepartmentId}",id);

            transaction.Rollback();

            throw;
        }
    }

    private static DynamicParameters MapToDepartMentDetails(Department department) {

        DynamicParameters dynamicParameters = new DynamicParameters();

        dynamicParameters.Add("Name", department.Name.ToString(), DbType.String, ParameterDirection.Input);
        dynamicParameters.Add("Description", department.Description.ToString(), DbType.String, ParameterDirection.Input);

        return dynamicParameters;

    }
}