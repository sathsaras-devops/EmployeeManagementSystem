namespace EmployeeManagement.API.Contracts.Employees;

public class UpdateEmployeeRequest
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public decimal Salary { get; set; }

    public DateTime DateOfBirth { get; set; }

    public DateTime DateOfJoining { get; set; }

    public int DepartmentId { get; set; }
}