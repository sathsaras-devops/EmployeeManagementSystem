namespace EmployeeManagement.API.Contracts.Departments;

public record UpdateDepartmentRequest(
    string Name,
    string? Description);