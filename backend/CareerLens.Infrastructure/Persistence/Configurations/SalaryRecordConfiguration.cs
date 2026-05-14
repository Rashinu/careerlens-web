using CareerLens.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerLens.Infrastructure.Persistence.Configurations;

public class SalaryRecordConfiguration : IEntityTypeConfiguration<SalaryRecord>
{
    public void Configure(EntityTypeBuilder<SalaryRecord> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Position).IsRequired().HasMaxLength(200);
        builder.Property(s => s.Sector).IsRequired().HasMaxLength(200);
        builder.Property(s => s.City).IsRequired().HasMaxLength(100);
        builder.Property(s => s.NetSalary).HasColumnType("decimal(18,2)");

        var comparer = new ValueComparer<List<string>>(
            (a, b) => a != null && b != null && a.SequenceEqual(b),
            v => v.Aggregate(0, (acc, s) => HashCode.Combine(acc, s.GetHashCode())),
            v => v.ToList());

        builder.Property(s => s.TechStack)
            .HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList())
            .HasColumnType("text")
            .Metadata.SetValueComparer(comparer);

        builder.HasIndex(s => s.Position);
        builder.HasIndex(s => s.City);
        builder.HasIndex(s => s.YearsOfExperience);
    }
}
