using CareerLens.Domain.Entities;
using FluentAssertions;

namespace CareerLens.Tests.Domain;

public class CvAnalysisEntityTests
{
    [Fact]
    public void Create_GecerliVeriler_UploadedStatusta()
    {
        var analysis = CvAnalysis.Create(Guid.NewGuid(), "cv.pdf", "blob/cv.pdf");

        analysis.Status.Should().Be(CvAnalysisStatus.Uploaded);
        analysis.ParsedRawText.Should().BeNull();
        analysis.ParsedData.Should().BeNull();
    }

    [Fact]
    public void SetExtractedText_MetinAtar_StatusDegisir()
    {
        var analysis = CvAnalysis.Create(Guid.NewGuid(), "cv.pdf", "blob/cv.pdf");

        analysis.SetExtractedText("ham metin içeriği");

        analysis.ParsedRawText.Should().Be("ham metin içeriği");
        analysis.Status.Should().Be(CvAnalysisStatus.TextExtracted);
    }

    [Fact]
    public void SetParsedData_JsonAtar_AnalyzedStatusa()
    {
        var analysis = CvAnalysis.Create(Guid.NewGuid(), "cv.pdf", "blob/cv.pdf");
        analysis.SetExtractedText("ham metin");

        analysis.SetParsedData("{\"name\":\"Test\"}");

        analysis.ParsedData.Should().Be("{\"name\":\"Test\"}");
        analysis.Status.Should().Be(CvAnalysisStatus.Analyzed);
    }

    [Fact]
    public void MarkFailed_StatusFailedOlur()
    {
        var analysis = CvAnalysis.Create(Guid.NewGuid(), "cv.pdf", "blob/cv.pdf");
        analysis.MarkFailed();
        analysis.Status.Should().Be(CvAnalysisStatus.Failed);
    }
}
