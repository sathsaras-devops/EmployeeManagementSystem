namespace EmployeeManagement.API.Contracts.Departments;

public sealed record CreateDepartmentRequest(
    string Name,
    string? Description);