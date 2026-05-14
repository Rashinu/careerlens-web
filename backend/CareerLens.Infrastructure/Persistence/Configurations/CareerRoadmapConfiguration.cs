using CareerLens.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerLens.Infrastructure.Persistence.Configurations;

public class CareerRoadmapConfiguration : IEntityTypeConfiguration<CareerRoadmap>
{
    public void Configure(EntityTypeBuilder<CareerRoadmap> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.TargetPosition).IsRequired().HasMaxLength(200);
        builder.Property(r => r.GapAnalysis).IsRequired().HasColumnType("jsonb");
        builder.Property(r => r.Recommendations).IsRequired().HasColumnType("jsonb");

        builder.HasIndex(r => r.UserId);
        builder.HasIndex(r => r.CvAnalysisId).IsUnique();
    }
}
