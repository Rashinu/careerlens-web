using CareerLens.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CareerLens.Infrastructure.Persistence;

public class CareerLensDbContext : DbContext
{
    public CareerLensDbContext(DbContextOptions<CareerLensDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<CvAnalysis> CvAnalyses => Set<CvAnalysis>();
    public DbSet<SalaryRecord> SalaryRecords => Set<SalaryRecord>();
    public DbSet<CareerRoadmap> CareerRoadmaps => Set<CareerRoadmap>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CareerLensDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
