using CareerLens.Shared.DTOs.Salary;
using MediatR;

namespace CareerLens.Application.Features.Salary.Queries.GetBenchmark;

public record GetSalaryBenchmarkQuery(string Position, string City, int YearsOfExperience) : IRequest<SalaryBenchmarkDto>;
