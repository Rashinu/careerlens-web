using FluentValidation;

namespace CareerLens.Application.Features.Salary.Commands.SubmitSalary;

public class SubmitSalaryCommandValidator : AbstractValidator<SubmitSalaryCommand>
{
    public SubmitSalaryCommandValidator()
    {
        RuleFor(x => x.Position).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Sector).NotEmpty().MaximumLength(200);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.YearsOfExperience).InclusiveBetween(0, 50);
        RuleFor(x => x.NetSalary).GreaterThan(0).LessThanOrEqualTo(10_000_000);
    }
}
