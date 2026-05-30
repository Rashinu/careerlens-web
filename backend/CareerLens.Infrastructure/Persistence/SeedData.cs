using CareerLens.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerLens.Infrastructure.Persistence;

public static class SeedData
{
    private const int MinExpectedRecords = 150;

    public static async Task SeedAsync(CareerLensDbContext context)
    {
        // Yeterli kayıt varsa seed'i atla, az varsa temizle ve yeniden ekle
        var count = await context.SalaryRecords.CountAsync();
        if (count >= MinExpectedRecords) return;

        if (count > 0)
        {
            context.SalaryRecords.RemoveRange(context.SalaryRecords);
            await context.SaveChangesAsync();
        }

        var records = new List<SalaryRecord>
        {
            // ── .NET Developer ──────────────────────────────────────────
            SalaryRecord.Create("Junior .NET Developer", "Teknoloji", "İstanbul", 1, 26000, [".NET", "C#", "SQL"]),
            SalaryRecord.Create("Junior .NET Developer", "Teknoloji", "İstanbul", 1, 28000, [".NET", "C#", "MSSQL"]),
            SalaryRecord.Create("Junior .NET Developer", "Teknoloji", "İstanbul", 2, 32000, [".NET", "C#", "Azure"]),
            SalaryRecord.Create("Mid .NET Developer",    "Teknoloji", "İstanbul", 3, 48000, [".NET", "C#", "PostgreSQL", "Redis"]),
            SalaryRecord.Create("Mid .NET Developer",    "Teknoloji", "İstanbul", 4, 55000, [".NET", "C#", "Docker"]),
            SalaryRecord.Create("Mid .NET Developer",    "Teknoloji", "İstanbul", 5, 62000, [".NET", "C#", "Kubernetes", "RabbitMQ"]),
            SalaryRecord.Create("Senior .NET Developer", "Teknoloji", "İstanbul", 6, 85000, [".NET", "C#", "Azure", "Microservices"]),
            SalaryRecord.Create("Senior .NET Developer", "Teknoloji", "İstanbul", 7, 95000, [".NET", "C#", "AWS", "Architecture"]),
            SalaryRecord.Create("Senior .NET Developer", "Teknoloji", "İstanbul", 9, 115000, [".NET", "C#", "Azure", "DDD"]),
            SalaryRecord.Create("Junior .NET Developer", "Teknoloji", "Ankara",   1, 23000, [".NET", "C#"]),
            SalaryRecord.Create("Junior .NET Developer", "Teknoloji", "Ankara",   2, 27000, [".NET", "C#", "SQL"]),
            SalaryRecord.Create("Mid .NET Developer",    "Teknoloji", "Ankara",   3, 42000, [".NET", "C#", "SQL"]),
            SalaryRecord.Create("Mid .NET Developer",    "Teknoloji", "Ankara",   4, 50000, [".NET", "C#", "Docker"]),
            SalaryRecord.Create("Senior .NET Developer", "Teknoloji", "Ankara",   6, 72000, [".NET", "C#", "Azure"]),
            SalaryRecord.Create("Senior .NET Developer", "Teknoloji", "Ankara",   8, 88000, [".NET", "C#", "Microservices"]),
            SalaryRecord.Create("Junior .NET Developer", "Teknoloji", "İzmir",    1, 22000, [".NET", "C#"]),
            SalaryRecord.Create("Mid .NET Developer",    "Teknoloji", "İzmir",    3, 40000, [".NET", "C#", "PostgreSQL"]),
            SalaryRecord.Create("Senior .NET Developer", "Teknoloji", "İzmir",    6, 68000, [".NET", "C#", "Azure"]),
            SalaryRecord.Create("Mid .NET Developer",    "Finans",    "İstanbul", 3, 55000, [".NET", "C#", "MSSQL"]),
            SalaryRecord.Create("Mid .NET Developer",    "Finans",    "İstanbul", 4, 63000, [".NET", "C#", "MSSQL", "Redis"]),
            SalaryRecord.Create("Senior .NET Developer", "Finans",    "İstanbul", 6, 95000, [".NET", "C#", "Azure", "Security"]),
            SalaryRecord.Create("Senior .NET Developer", "Finans",    "İstanbul", 8, 115000,[".NET", "C#", "Azure", "CQRS"]),
            SalaryRecord.Create("Mid .NET Developer",    "E-ticaret", "İstanbul", 3, 46000, [".NET", "C#", "Redis", "RabbitMQ"]),
            SalaryRecord.Create("Senior .NET Developer", "E-ticaret", "İstanbul", 5, 78000, [".NET", "C#", "Microservices"]),
            SalaryRecord.Create("Mid .NET Developer",    "Sağlık",    "İstanbul", 3, 44000, [".NET", "C#", "SQL"]),
            SalaryRecord.Create("Senior .NET Developer", "Sağlık",    "İstanbul", 6, 80000, [".NET", "C#", "Azure"]),
            SalaryRecord.Create("Mid .NET Developer",    "Danışmanlık","İstanbul",3, 50000, [".NET", "C#", "SQL"]),
            SalaryRecord.Create("Senior .NET Developer", "Danışmanlık","İstanbul",6, 90000, [".NET", "C#", "Azure"]),

            // ── Frontend Developer ───────────────────────────────────────
            SalaryRecord.Create("Junior Frontend Developer", "Teknoloji", "İstanbul", 1, 24000, ["React", "TypeScript", "CSS"]),
            SalaryRecord.Create("Junior Frontend Developer", "Teknoloji", "İstanbul", 2, 29000, ["React", "TypeScript", "Tailwind"]),
            SalaryRecord.Create("Mid Frontend Developer",    "Teknoloji", "İstanbul", 3, 45000, ["React", "TypeScript", "Next.js"]),
            SalaryRecord.Create("Mid Frontend Developer",    "Teknoloji", "İstanbul", 4, 53000, ["React", "TypeScript", "Redux"]),
            SalaryRecord.Create("Mid Frontend Developer",    "Teknoloji", "İstanbul", 5, 60000, ["React", "TypeScript", "Next.js", "GraphQL"]),
            SalaryRecord.Create("Senior Frontend Developer", "Teknoloji", "İstanbul", 6, 80000, ["React", "TypeScript", "Next.js", "GraphQL"]),
            SalaryRecord.Create("Senior Frontend Developer", "Teknoloji", "İstanbul", 8, 100000,["React", "TypeScript", "Next.js", "Architecture"]),
            SalaryRecord.Create("Junior Frontend Developer", "Teknoloji", "Ankara",   1, 21000, ["React", "JavaScript"]),
            SalaryRecord.Create("Mid Frontend Developer",    "Teknoloji", "Ankara",   3, 40000, ["React", "TypeScript"]),
            SalaryRecord.Create("Senior Frontend Developer", "Teknoloji", "Ankara",   6, 70000, ["React", "TypeScript", "Next.js"]),
            SalaryRecord.Create("Mid Frontend Developer",    "Teknoloji", "İzmir",    3, 38000, ["React", "TypeScript"]),
            SalaryRecord.Create("Senior Frontend Developer", "Teknoloji", "İzmir",    6, 65000, ["React", "TypeScript", "Next.js"]),
            SalaryRecord.Create("Mid Frontend Developer",    "E-ticaret", "İstanbul", 3, 48000, ["React", "TypeScript", "Next.js"]),
            SalaryRecord.Create("Senior Frontend Developer", "E-ticaret", "İstanbul", 6, 82000, ["React", "TypeScript", "GraphQL"]),
            SalaryRecord.Create("Mid Frontend Developer",    "Finans",    "İstanbul", 3, 52000, ["React", "TypeScript", "Redux"]),
            SalaryRecord.Create("Senior Frontend Developer", "Finans",    "İstanbul", 6, 88000, ["React", "TypeScript", "Next.js"]),
            SalaryRecord.Create("Mid Frontend Developer",    "Gaming",    "İstanbul", 3, 50000, ["React", "TypeScript", "WebGL"]),
            SalaryRecord.Create("Senior Frontend Developer", "Gaming",    "İstanbul", 6, 85000, ["React", "TypeScript", "WebGL", "Three.js"]),

            // ── Backend Developer ─────────────────────────────────────────
            SalaryRecord.Create("Junior Backend Developer", "Teknoloji", "İstanbul", 1, 25000, ["Java", "Spring Boot"]),
            SalaryRecord.Create("Junior Backend Developer", "Teknoloji", "İstanbul", 2, 30000, ["Java", "Spring Boot", "PostgreSQL"]),
            SalaryRecord.Create("Mid Backend Developer",    "Teknoloji", "İstanbul", 3, 47000, ["Java", "Spring Boot", "Kubernetes"]),
            SalaryRecord.Create("Mid Backend Developer",    "Teknoloji", "İstanbul", 4, 55000, ["Java", "Spring Boot", "Kafka"]),
            SalaryRecord.Create("Senior Backend Developer", "Teknoloji", "İstanbul", 6, 80000, ["Java", "Spring Boot", "Kafka", "Microservices"]),
            SalaryRecord.Create("Senior Backend Developer", "Teknoloji", "İstanbul", 8, 100000,["Java", "Spring Boot", "Architecture"]),
            SalaryRecord.Create("Junior Backend Developer", "Teknoloji", "Ankara",   1, 22000, ["Java", "Spring Boot"]),
            SalaryRecord.Create("Mid Backend Developer",    "Teknoloji", "Ankara",   3, 42000, ["Java", "Spring Boot"]),
            SalaryRecord.Create("Senior Backend Developer", "Teknoloji", "Ankara",   6, 70000, ["Java", "Spring Boot", "Microservices"]),
            SalaryRecord.Create("Mid Backend Developer",    "Finans",    "İstanbul", 3, 55000, ["Java", "Spring Boot", "Kafka"]),
            SalaryRecord.Create("Senior Backend Developer", "Finans",    "İstanbul", 6, 92000, ["Java", "Spring Boot", "Security"]),

            // ── Python Developer ──────────────────────────────────────────
            SalaryRecord.Create("Junior Python Developer", "Teknoloji", "İstanbul", 1, 23000, ["Python", "Django"]),
            SalaryRecord.Create("Junior Python Developer", "Teknoloji", "İstanbul", 2, 28000, ["Python", "FastAPI"]),
            SalaryRecord.Create("Mid Python Developer",    "Teknoloji", "İstanbul", 3, 43000, ["Python", "FastAPI", "PostgreSQL"]),
            SalaryRecord.Create("Mid Python Developer",    "Teknoloji", "İstanbul", 4, 52000, ["Python", "FastAPI", "Redis", "Celery"]),
            SalaryRecord.Create("Senior Python Developer", "Teknoloji", "İstanbul", 6, 75000, ["Python", "FastAPI", "ML", "AWS"]),
            SalaryRecord.Create("Senior Python Developer", "Teknoloji", "İstanbul", 8, 95000, ["Python", "Django", "Microservices"]),
            SalaryRecord.Create("Mid Python Developer",    "Teknoloji", "Ankara",   3, 38000, ["Python", "Django"]),
            SalaryRecord.Create("Senior Python Developer", "Teknoloji", "Ankara",   6, 65000, ["Python", "FastAPI"]),
            SalaryRecord.Create("Mid Python Developer",    "Finans",    "İstanbul", 3, 48000, ["Python", "FastAPI", "SQL"]),
            SalaryRecord.Create("Senior Python Developer", "Finans",    "İstanbul", 6, 82000, ["Python", "FastAPI", "AWS"]),
            SalaryRecord.Create("Mid Python Developer",    "E-ticaret", "İstanbul", 3, 45000, ["Python", "Django", "Redis"]),

            // ── Full Stack Developer ──────────────────────────────────────
            SalaryRecord.Create("Junior Full Stack Developer", "Teknoloji", "İstanbul", 1, 26000, [".NET", "React"]),
            SalaryRecord.Create("Mid Full Stack Developer",    "Teknoloji", "İstanbul", 3, 52000, [".NET", "React", "PostgreSQL"]),
            SalaryRecord.Create("Mid Full Stack Developer",    "Teknoloji", "İstanbul", 4, 60000, [".NET", "React", "Azure"]),
            SalaryRecord.Create("Senior Full Stack Developer", "Teknoloji", "İstanbul", 6, 85000, [".NET", "React", "Azure", "Docker"]),
            SalaryRecord.Create("Senior Full Stack Developer", "Teknoloji", "İstanbul", 8, 105000,[".NET", "React", "Microservices"]),
            SalaryRecord.Create("Mid Full Stack Developer",    "Teknoloji", "Ankara",   3, 45000, [".NET", "React"]),
            SalaryRecord.Create("Senior Full Stack Developer", "Teknoloji", "Ankara",   6, 72000, [".NET", "React", "Docker"]),
            SalaryRecord.Create("Mid Full Stack Developer",    "E-ticaret", "İstanbul", 3, 55000, ["Node.js", "React", "MongoDB"]),
            SalaryRecord.Create("Senior Full Stack Developer", "E-ticaret", "İstanbul", 6, 88000, ["Node.js", "React", "Microservices"]),

            // ── DevOps / Cloud ────────────────────────────────────────────
            SalaryRecord.Create("Junior DevOps Engineer",  "Teknoloji", "İstanbul", 1, 30000, ["Linux", "Docker", "Git"]),
            SalaryRecord.Create("DevOps Engineer",         "Teknoloji", "İstanbul", 2, 42000, ["Docker", "Kubernetes", "CI/CD"]),
            SalaryRecord.Create("DevOps Engineer",         "Teknoloji", "İstanbul", 3, 58000, ["Docker", "Kubernetes", "Terraform", "AWS"]),
            SalaryRecord.Create("DevOps Engineer",         "Teknoloji", "İstanbul", 4, 68000, ["Kubernetes", "Terraform", "AWS", "Helm"]),
            SalaryRecord.Create("Senior DevOps Engineer",  "Teknoloji", "İstanbul", 6, 92000, ["Kubernetes", "Terraform", "AWS", "CI/CD"]),
            SalaryRecord.Create("Senior DevOps Engineer",  "Teknoloji", "İstanbul", 8, 115000,["Kubernetes", "Terraform", "AWS", "SRE"]),
            SalaryRecord.Create("DevOps Engineer",         "Teknoloji", "Ankara",   3, 50000, ["Docker", "Kubernetes"]),
            SalaryRecord.Create("Senior DevOps Engineer",  "Teknoloji", "Ankara",   6, 80000, ["Kubernetes", "Terraform"]),
            SalaryRecord.Create("DevOps Engineer",         "Finans",    "İstanbul", 3, 65000, ["Docker", "Kubernetes", "Security"]),
            SalaryRecord.Create("Senior DevOps Engineer",  "Finans",    "İstanbul", 6, 100000,["Kubernetes", "Terraform", "Security"]),
            SalaryRecord.Create("Cloud Engineer",          "Teknoloji", "İstanbul", 3, 62000, ["AWS", "Terraform", "Python"]),
            SalaryRecord.Create("Senior Cloud Engineer",   "Teknoloji", "İstanbul", 6, 98000, ["AWS", "GCP", "Terraform"]),

            // ── Data / AI / ML ────────────────────────────────────────────
            SalaryRecord.Create("Junior Data Analyst",     "Teknoloji", "İstanbul", 1, 22000, ["Python", "SQL", "Excel"]),
            SalaryRecord.Create("Data Analyst",            "Teknoloji", "İstanbul", 2, 32000, ["Python", "SQL", "Tableau"]),
            SalaryRecord.Create("Mid Data Analyst",        "Teknoloji", "İstanbul", 3, 42000, ["Python", "SQL", "Power BI"]),
            SalaryRecord.Create("Senior Data Analyst",     "Teknoloji", "İstanbul", 6, 68000, ["Python", "SQL", "Spark"]),
            SalaryRecord.Create("Junior Data Scientist",   "Teknoloji", "İstanbul", 1, 28000, ["Python", "ML", "SQL"]),
            SalaryRecord.Create("Data Scientist",          "Teknoloji", "İstanbul", 2, 42000, ["Python", "TensorFlow", "SQL"]),
            SalaryRecord.Create("Mid Data Scientist",      "Teknoloji", "İstanbul", 3, 55000, ["Python", "TensorFlow", "MLflow"]),
            SalaryRecord.Create("Mid Data Scientist",      "Teknoloji", "İstanbul", 4, 65000, ["Python", "PyTorch", "Spark"]),
            SalaryRecord.Create("Senior Data Scientist",   "Teknoloji", "İstanbul", 6, 90000, ["Python", "PyTorch", "MLOps", "Spark"]),
            SalaryRecord.Create("Senior Data Scientist",   "Teknoloji", "İstanbul", 8, 115000,["Python", "PyTorch", "LLM", "MLOps"]),
            SalaryRecord.Create("ML Engineer",             "Teknoloji", "İstanbul", 3, 62000, ["Python", "TensorFlow", "Docker"]),
            SalaryRecord.Create("Senior ML Engineer",      "Teknoloji", "İstanbul", 6, 95000, ["Python", "PyTorch", "Kubernetes"]),
            SalaryRecord.Create("AI Engineer",             "Teknoloji", "İstanbul", 3, 68000, ["Python", "LLM", "LangChain"]),
            SalaryRecord.Create("Senior AI Engineer",      "Teknoloji", "İstanbul", 6, 105000,["Python", "LLM", "RAG", "MLOps"]),
            SalaryRecord.Create("Data Engineer",           "Teknoloji", "İstanbul", 3, 58000, ["Python", "Spark", "Airflow"]),
            SalaryRecord.Create("Senior Data Engineer",    "Teknoloji", "İstanbul", 6, 88000, ["Python", "Spark", "Kafka", "dbt"]),
            SalaryRecord.Create("Mid Data Scientist",      "Finans",    "İstanbul", 3, 62000, ["Python", "ML", "SQL"]),
            SalaryRecord.Create("Senior Data Scientist",   "Finans",    "İstanbul", 6, 100000,["Python", "PyTorch", "Risk Modeling"]),

            // ── Mobile Developer ──────────────────────────────────────────
            SalaryRecord.Create("Junior Mobile Developer", "Teknoloji", "İstanbul", 1, 25000, ["Swift", "iOS"]),
            SalaryRecord.Create("Mobile Developer",        "Teknoloji", "İstanbul", 2, 36000, ["Swift", "iOS", "Xcode"]),
            SalaryRecord.Create("Mid Mobile Developer",    "Teknoloji", "İstanbul", 3, 50000, ["Swift", "iOS", "SwiftUI"]),
            SalaryRecord.Create("Senior Mobile Developer", "Teknoloji", "İstanbul", 6, 82000, ["Swift", "iOS", "React Native"]),
            SalaryRecord.Create("Junior Mobile Developer", "Teknoloji", "İstanbul", 1, 24000, ["Kotlin", "Android"]),
            SalaryRecord.Create("Mid Mobile Developer",    "Teknoloji", "İstanbul", 3, 48000, ["Kotlin", "Android", "Jetpack Compose"]),
            SalaryRecord.Create("Senior Mobile Developer", "Teknoloji", "İstanbul", 6, 80000, ["Kotlin", "Android", "Architecture"]),
            SalaryRecord.Create("Mid Mobile Developer",    "Teknoloji", "İstanbul", 3, 52000, ["Flutter", "Dart"]),
            SalaryRecord.Create("Senior Mobile Developer", "Teknoloji", "İstanbul", 6, 85000, ["Flutter", "Dart", "React Native"]),
            SalaryRecord.Create("Mid Mobile Developer",    "Teknoloji", "Ankara",   3, 43000, ["Swift", "iOS"]),
            SalaryRecord.Create("Senior Mobile Developer", "Teknoloji", "Ankara",   6, 70000, ["Swift", "iOS", "Kotlin"]),
            SalaryRecord.Create("Mid Mobile Developer",    "E-ticaret", "İstanbul", 3, 55000, ["Flutter", "Dart"]),
            SalaryRecord.Create("Senior Mobile Developer", "Gaming",    "İstanbul", 6, 90000, ["Unity", "C#", "iOS", "Android"]),

            // ── QA / Test Engineer ────────────────────────────────────────
            SalaryRecord.Create("Junior QA Engineer",    "Teknoloji", "İstanbul", 1, 20000, ["Manuel Test", "JIRA"]),
            SalaryRecord.Create("QA Engineer",           "Teknoloji", "İstanbul", 2, 28000, ["Selenium", "Postman"]),
            SalaryRecord.Create("Mid QA Engineer",       "Teknoloji", "İstanbul", 3, 38000, ["Selenium", "Cypress", "API Test"]),
            SalaryRecord.Create("Senior QA Engineer",    "Teknoloji", "İstanbul", 6, 60000, ["Selenium", "Cypress", "Performance Test"]),
            SalaryRecord.Create("SDET",                  "Teknoloji", "İstanbul", 3, 48000, ["Selenium", "Python", "CI/CD"]),
            SalaryRecord.Create("Senior SDET",           "Teknoloji", "İstanbul", 6, 75000, ["Playwright", "Python", "k6"]),
            SalaryRecord.Create("Mid QA Engineer",       "Finans",    "İstanbul", 3, 42000, ["Selenium", "API Test"]),

            // ── Product / Design ──────────────────────────────────────────
            SalaryRecord.Create("Junior UX Designer",    "Teknoloji", "İstanbul", 1, 22000, ["Figma", "User Research"]),
            SalaryRecord.Create("Mid UX Designer",       "Teknoloji", "İstanbul", 3, 40000, ["Figma", "User Research", "Prototyping"]),
            SalaryRecord.Create("Senior UX Designer",    "Teknoloji", "İstanbul", 6, 65000, ["Figma", "Design System", "UX Research"]),
            SalaryRecord.Create("Product Designer",      "Teknoloji", "İstanbul", 4, 55000, ["Figma", "Design System", "Prototyping"]),
            SalaryRecord.Create("Senior Product Designer","Teknoloji", "İstanbul", 7, 85000, ["Figma", "Design System", "Leadership"]),
            SalaryRecord.Create("Junior Product Manager", "Teknoloji", "İstanbul", 1, 28000, ["JIRA", "Confluence", "Agile"]),
            SalaryRecord.Create("Product Manager",        "Teknoloji", "İstanbul", 3, 55000, ["JIRA", "Analytics", "Roadmap"]),
            SalaryRecord.Create("Senior Product Manager", "Teknoloji", "İstanbul", 6, 85000, ["Strategy", "OKR", "Analytics"]),
            SalaryRecord.Create("Product Manager",        "E-ticaret", "İstanbul", 3, 60000, ["JIRA", "Analytics", "A/B Test"]),
            SalaryRecord.Create("Senior Product Manager", "Finans",    "İstanbul", 6, 95000, ["Strategy", "Compliance", "Analytics"]),

            // ── Scrum / Agile / BA ────────────────────────────────────────
            SalaryRecord.Create("Scrum Master",          "Teknoloji", "İstanbul", 3, 48000, ["Scrum", "JIRA", "Agile"]),
            SalaryRecord.Create("Senior Scrum Master",   "Teknoloji", "İstanbul", 6, 72000, ["SAFe", "Coaching", "Agile"]),
            SalaryRecord.Create("Business Analyst",      "Teknoloji", "İstanbul", 2, 35000, ["JIRA", "SQL", "BPMN"]),
            SalaryRecord.Create("Mid Business Analyst",  "Teknoloji", "İstanbul", 4, 52000, ["JIRA", "SQL", "Business Analysis"]),
            SalaryRecord.Create("Senior Business Analyst","Teknoloji","İstanbul",  7, 78000, ["BPMN", "SQL", "Strategy"]),
            SalaryRecord.Create("Business Analyst",      "Finans",    "İstanbul", 3, 55000, ["JIRA", "SQL", "Fintech"]),

            // ── Security / Network ────────────────────────────────────────
            SalaryRecord.Create("Cybersecurity Engineer", "Teknoloji", "İstanbul", 3, 60000, ["SIEM", "Penetration Test"]),
            SalaryRecord.Create("Senior Cybersecurity",   "Teknoloji", "İstanbul", 6, 95000, ["SIEM", "SOC", "Architecture"]),
            SalaryRecord.Create("Network Engineer",       "Teknoloji", "İstanbul", 3, 45000, ["Cisco", "Network", "Firewall"]),
            SalaryRecord.Create("Senior Network Engineer","Teknoloji", "İstanbul", 6, 70000, ["Cisco", "SDN", "Cloud Network"]),
            SalaryRecord.Create("Cybersecurity Engineer", "Finans",    "İstanbul", 3, 70000, ["SIEM", "PCI-DSS"]),

            // ── Diğer şehirler (İzmir, Bursa, Antalya) ───────────────────
            SalaryRecord.Create("Mid Frontend Developer",    "Teknoloji", "İzmir",   3, 38000, ["React", "TypeScript"]),
            SalaryRecord.Create("Senior Frontend Developer", "Teknoloji", "İzmir",   6, 63000, ["React", "TypeScript", "Next.js"]),
            SalaryRecord.Create("Mid Backend Developer",     "Teknoloji", "İzmir",   3, 40000, ["Java", "Spring Boot"]),
            SalaryRecord.Create("Senior Backend Developer",  "Teknoloji", "İzmir",   6, 65000, ["Java", "Spring Boot"]),
            SalaryRecord.Create("Mid Python Developer",      "Teknoloji", "İzmir",   3, 37000, ["Python", "Django"]),
            SalaryRecord.Create("DevOps Engineer",           "Teknoloji", "İzmir",   3, 48000, ["Docker", "Kubernetes"]),
            SalaryRecord.Create("Mid Full Stack Developer",  "Teknoloji", "İzmir",   3, 43000, [".NET", "React"]),
            SalaryRecord.Create("Mid Frontend Developer",    "Teknoloji", "Bursa",   3, 35000, ["React", "TypeScript"]),
            SalaryRecord.Create("Mid Backend Developer",     "Teknoloji", "Bursa",   3, 37000, ["Java", "Spring Boot"]),
            SalaryRecord.Create("Mid .NET Developer",        "Teknoloji", "Bursa",   3, 38000, [".NET", "C#"]),
            SalaryRecord.Create("Senior .NET Developer",     "Teknoloji", "Bursa",   6, 62000, [".NET", "C#", "Azure"]),
            SalaryRecord.Create("Mid Frontend Developer",    "Teknoloji", "Antalya", 3, 34000, ["React", "TypeScript"]),
            SalaryRecord.Create("Mid Backend Developer",     "Teknoloji", "Antalya", 3, 36000, ["Java", "Spring Boot"]),
            SalaryRecord.Create("Mid .NET Developer",        "Teknoloji", "Antalya", 3, 37000, [".NET", "C#"]),
        };

        await context.SalaryRecords.AddRangeAsync(records);
        await context.SaveChangesAsync();
    }
}
