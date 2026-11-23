namespace EmployeeManagement.Api.Contracts.Departments
{
    public class DepartmentResponse
    {
        public int Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string? Description { get; init; }
    }
}
