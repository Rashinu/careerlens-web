using CareerLens.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CareerLens.Infrastructure.Persistence.Configurations;

public class CvAnalysisConfiguration : IEntityTypeConfiguration<CvAnalysis>
{
    public void Configure(EntityTypeBuilder<CvAnalysis> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.OriginalFileName)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(c => c.RawFileUrl)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(c => c.ParsedRawText).HasColumnType("text");
        builder.Property(c => c.ParsedData).HasColumnType("jsonb");

        builder.HasIndex(c => c.UserId);
        builder.HasIndex(c => c.Status);

        builder.HasOne(c => c.CareerRoadmap)
            .WithOne(r => r.CvAnalysis)
            .HasForeignKey<CareerRoadmap>(r => r.CvAnalysisId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
