namespace CareerLens.Domain.Entities;

public class SalaryRecord : BaseEntity
{
    public string Position { get; private set; } = default!;
    public string Sector { get; private set; } = default!;
    public string City { get; private set; } = default!;
    public int YearsOfExperience { get; private set; }
    public decimal NetSalary { get; private set; }
    public List<string> TechStack { get; private set; } = new();
    public DateTime SubmittedAt { get; private set; } = DateTime.UtcNow;

    private SalaryRecord() { }

    public static SalaryRecord Create(
        string position,
        string sector,
        string city,
        int yearsOfExperience,
        decimal netSalary,
        IEnumerable<string> techStack)
    {
        return new SalaryRecord
        {
            Position = position,
            Sector = sector,
            City = city,
            YearsOfExperience = yearsOfExperience,
            NetSalary = netSalary,
            TechStack = techStack.ToList(),
            SubmittedAt = DateTime.UtcNow
        };
    }
}
