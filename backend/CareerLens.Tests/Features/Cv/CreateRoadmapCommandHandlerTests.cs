using CareerLens.Application.Common.Interfaces;
using CareerLens.Application.Features.Cv.Commands.CreateRoadmap;
using CareerLens.Domain.Entities;
using CareerLens.Domain.Interfaces;
using CareerLens.Tests.Helpers;
using FluentAssertions;
using Moq;

namespace CareerLens.Tests.Features.Cv;

public class CreateRoadmapCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _uow;
    private readonly Mock<ICareerAiService> _aiService;
    private readonly Mock<ICpiIndexService> _cpiIndex;
    private readonly CreateRoadmapCommandHandler _handler;

    public CreateRoadmapCommandHandlerTests()
    {
        _uow = MockUnitOfWork.Create();
        _aiService = new Mock<ICareerAiService>();
        _cpiIndex = new Mock<ICpiIndexService>();
        _cpiIndex.Setup(c => c.GetAdjustmentFactorAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1.0m);
        _handler = new CreateRoadmapCommandHandler(_uow.Object, _aiService.Object, _cpiIndex.Object);
    }

    [Fact]
    public async Task Handle_CvdenDeneyimCikarir_VeUlkeCapindaBenchmarkKullanir()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var analysis = CvAnalysis.Create(userId, "cv.pdf", "https://blob/cv.pdf");
        analysis.SetParsedData("""{"yearsOfExperience": 5, "techStack": [".NET"]}""");

        Mock.Get(_uow.Object.CvAnalyses)
            .Setup(r => r.GetByIdAsync(analysis.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(analysis);

        var benchmarkRecords = new List<SalaryRecord>
        {
            SalaryRecord.Create("Senior Developer", "Teknoloji", "İstanbul", 5, 80000, [".NET"]),
            SalaryRecord.Create("Senior Developer", "Teknoloji", "Ankara", 5, 70000, [".NET"]),
        };

        Mock.Get(_uow.Object.SalaryRecords)
            .Setup(r => r.GetBenchmarkDataAsync("Senior Developer", null, 5, It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(benchmarkRecords);

        string? capturedBenchmarkJson = null;
        _aiService
            .Setup(a => a.GenerateRoadmapAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Callback<string, string, string, CancellationToken>((_, _, benchmarkJson, _) => capturedBenchmarkJson = benchmarkJson)
            .ReturnsAsync("""{"currentScore": 70, "gapAnalysis": {}, "recommendations": {}}""");

        var command = new CreateRoadmapCommand(analysis.Id, userId, "Senior Developer");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.CurrentScore.Should().Be(70);
        capturedBenchmarkJson.Should().NotBeNull();
        capturedBenchmarkJson!.Should().Contain("p50Salary");
        capturedBenchmarkJson.Should().Contain("\"sampleCount\":2");

        Mock.Get(_uow.Object.SalaryRecords).Verify(
            r => r.GetBenchmarkDataAsync("Senior Developer", null, 5, It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ParsedDataDaYearsYok_VarsayilanUcKullanir()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var analysis = CvAnalysis.Create(userId, "cv.pdf", "https://blob/cv.pdf");
        analysis.SetParsedData("""{"techStack": [".NET"]}""");

        Mock.Get(_uow.Object.CvAnalyses)
            .Setup(r => r.GetByIdAsync(analysis.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(analysis);

        Mock.Get(_uow.Object.SalaryRecords)
            .Setup(r => r.GetBenchmarkDataAsync(It.IsAny<string>(), null, 3, It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SalaryRecord>());

        _aiService
            .Setup(a => a.GenerateRoadmapAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("""{"currentScore": 50, "gapAnalysis": {}, "recommendations": {}}""");

        var command = new CreateRoadmapCommand(analysis.Id, userId, "Junior Developer");

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Mock.Get(_uow.Object.SalaryRecords).Verify(
            r => r.GetBenchmarkDataAsync("Junior Developer", null, 3, It.IsAny<int>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
