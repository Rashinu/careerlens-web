namespace CareerLens.Shared.DTOs.Salary;

public record SalaryBenchmarkDto(
    decimal P25,
    decimal P50,
    decimal P75,
    int SampleCount,
    string Position,
    string City,
    int YearsOfExperience);

public record SubmitSalaryRequest(
    string Position,
    string Sector,
    string City,
    int YearsOfExperience,
    decimal NetSalary,
    IEnumerable<string> TechStack);
