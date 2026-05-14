using MediatR;

namespace CareerLens.Application.Features.Salary.Commands.SubmitSalary;

public record SubmitSalaryCommand(
    string Position,
    string Sector,
    string City,
    int YearsOfExperience,
    decimal NetSalary,
    IEnumerable<string> TechStack) : IRequest<Guid>;
