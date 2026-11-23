using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeManagement.Application.DTOs;

public class EmployeeDto
{
    public int Id { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public decimal Salary { get; init; }
    public DateTime DateOfBirth { get; set; }

    public DateTime DateOfJoining { get; init; }

    public int DepartmentId { get; init; }

    public string DepartmentName { get; init; } = string.Empty;
}