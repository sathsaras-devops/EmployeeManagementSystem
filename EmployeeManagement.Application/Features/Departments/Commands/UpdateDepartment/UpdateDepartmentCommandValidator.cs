using FluentValidation;

namespace EmployeeManagement.Application.Features.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommandValidator
    : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Department name is required.")

            .Must(name =>
                !string.IsNullOrWhiteSpace(name) &&
                name.Trim().Length >= 3)
            .WithMessage(
                "Department name must be at least 3 characters.")

            .MaximumLength(100)
            .WithMessage(
                "Department name cannot exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Department name is required.")

            .Must(name => !string.IsNullOrWhiteSpace(name) &&
                          name.Trim().Length >= 3)
            .WithMessage("Department name must be at least 3 characters.")

            .MaximumLength(100)
            .WithMessage("Department name cannot exceed 100 characters.");
    }
}