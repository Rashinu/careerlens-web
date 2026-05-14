using CareerLens.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerLens.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task SeedAsync(CareerLensDbContext context)
    {
        if (await context.SalaryRecords.AnyAsync()) return;

        var records = new List<SalaryRecord>
        {
            SalaryRecord.Create("Junior .NET Developer", "Teknoloji", "İstanbul", 1, 25000, [".NET", "C#", "SQL"]),
            SalaryRecord.Create("Junior .NET Developer", "Teknoloji", "İstanbul", 2, 30000, [".NET", "C#", "SQL", "Azure"]),
            SalaryRecord.Create("Mid .NET Developer", "Teknoloji", "İstanbul", 3, 45000, [".NET", "C#", "PostgreSQL", "Redis"]),
            SalaryRecord.Create("Mid .NET Developer", "Teknoloji", "İstanbul", 4, 52000, [".NET", "C#", "Docker", "Kubernetes"]),
            SalaryRecord.Create("Senior .NET Developer", "Teknoloji", "İstanbul", 6, 75000, [".NET", "C#", "Azure", "Microservices"]),
            SalaryRecord.Create("Senior .NET Developer", "Teknoloji", "İstanbul", 8, 90000, [".NET", "C#", "AWS", "Architecture"]),
            SalaryRecord.Create("Junior .NET Developer", "Teknoloji", "Ankara", 1, 22000, [".NET", "C#"]),
            SalaryRecord.Create("Mid .NET Developer", "Teknoloji", "Ankara", 3, 40000, [".NET", "C#", "SQL"]),
            SalaryRecord.Create("Senior .NET Developer", "Teknoloji", "Ankara", 6, 65000, [".NET", "C#", "Azure"]),
            SalaryRecord.Create("Junior Frontend Developer", "Teknoloji", "İstanbul", 1, 23000, ["React", "TypeScript", "CSS"]),
            SalaryRecord.Create("Mid Frontend Developer", "Teknoloji", "İstanbul", 3, 42000, ["React", "TypeScript", "Next.js"]),
            SalaryRecord.Create("Senior Frontend Developer", "Teknoloji", "İstanbul", 6, 70000, ["React", "TypeScript", "Next.js", "GraphQL"]),
            SalaryRecord.Create("Junior Backend Developer", "Teknoloji", "İstanbul", 1, 24000, ["Java", "Spring Boot"]),
            SalaryRecord.Create("Mid Backend Developer", "Teknoloji", "İstanbul", 3, 44000, ["Java", "Spring Boot", "Kubernetes"]),
            SalaryRecord.Create("Senior Backend Developer", "Teknoloji", "İstanbul", 6, 72000, ["Java", "Spring Boot", "Kafka", "Microservices"]),
            SalaryRecord.Create("Junior Python Developer", "Teknoloji", "İstanbul", 1, 22000, ["Python", "Django"]),
            SalaryRecord.Create("Mid Python Developer", "Teknoloji", "İstanbul", 3, 40000, ["Python", "FastAPI", "PostgreSQL"]),
            SalaryRecord.Create("Senior Python Developer", "Teknoloji", "İstanbul", 5, 65000, ["Python", "FastAPI", "ML", "AWS"]),
            SalaryRecord.Create("DevOps Engineer", "Teknoloji", "İstanbul", 3, 55000, ["Docker", "Kubernetes", "Terraform", "AWS"]),
            SalaryRecord.Create("Senior DevOps Engineer", "Teknoloji", "İstanbul", 6, 85000, ["Kubernetes", "Terraform", "AWS", "CI/CD"]),
            SalaryRecord.Create("Data Scientist", "Teknoloji", "İstanbul", 2, 45000, ["Python", "TensorFlow", "SQL"]),
            SalaryRecord.Create("Senior Data Scientist", "Teknoloji", "İstanbul", 5, 80000, ["Python", "PyTorch", "MLOps", "Spark"]),
            SalaryRecord.Create("Mobile Developer", "Teknoloji", "İstanbul", 2, 38000, ["Swift", "iOS"]),
            SalaryRecord.Create("Senior Mobile Developer", "Teknoloji", "İstanbul", 5, 68000, ["Swift", "iOS", "React Native"]),
            SalaryRecord.Create("Full Stack Developer", "Teknoloji", "İstanbul", 3, 48000, [".NET", "React", "PostgreSQL"]),
            SalaryRecord.Create("Senior Full Stack Developer", "Teknoloji", "İstanbul", 6, 78000, [".NET", "React", "Azure", "Docker"]),
            SalaryRecord.Create("Mid .NET Developer", "Finans", "İstanbul", 3, 50000, [".NET", "C#", "MSSQL"]),
            SalaryRecord.Create("Senior .NET Developer", "Finans", "İstanbul", 6, 85000, [".NET", "C#", "Azure", "Security"]),
            SalaryRecord.Create("Mid .NET Developer", "E-ticaret", "İstanbul", 3, 43000, [".NET", "C#", "Redis", "RabbitMQ"]),
            SalaryRecord.Create("Senior .NET Developer", "E-ticaret", "İstanbul", 5, 70000, [".NET", "C#", "Microservices", "Kubernetes"]),
        };

        await context.SalaryRecords.AddRangeAsync(records);
        await context.SaveChangesAsync();
    }
}
