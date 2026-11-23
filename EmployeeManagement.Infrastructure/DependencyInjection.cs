using EmployeeManagement.Application.Abstractions.Persistence;
using EmployeeManagement.Infrastructure.Persistence;
using EmployeeManagement.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {

        //services.AddSingleton<IDbConnectionFactory>(
        //   new SqlConnectionFactory(connectionString));

        services.AddScoped<IDbConnectionFactory>(provider =>
        {
            return new SqlConnectionFactory(connectionString);
        });

        // Repositories
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();

        services.AddScoped<IEmployeeRepository, EmployeeRepository>();

        return services;
    }
}

