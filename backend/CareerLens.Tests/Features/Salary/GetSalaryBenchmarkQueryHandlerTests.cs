using CareerLens.Application.Common.Interfaces;
using CareerLens.Application.Features.Salary.Queries.GetBenchmark;
using CareerLens.Domain.Entities;
using CareerLens.Domain.Interfaces;
using CareerLens.Tests.Helpers;
using FluentAssertions;
using Moq;

namespace CareerLens.Tests.Features.Salary;

public class GetSalaryBenchmarkQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _uow;
    private readonly Mock<ICacheService> _cache;
    private readonly Mock<ICpiIndexService> _cpiIndex;
    private readonly GetSalaryBenchmarkQueryHandler _handler;

    public GetSalaryBenchmarkQueryHandlerTests()
    {
        _uow = MockUnitOfWork.Create();
        _cache = new Mock<ICacheService>();
        _cpiIndex = new Mock<ICpiIndexService>();
        _cpiIndex.Setup(c => c.GetAdjustmentFactorAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1.0m);
        _handler = new GetSalaryBenchmarkQueryHandler(_uow.Object, _cache.Object, _cpiIndex.Object);
    }

    [Fact]
    public async Task Handle_YeterliVeri_P25P50P75Hesaplar()
    {
        // Arrange
        var query = new GetSalaryBenchmarkQuery(".NET Developer", "İstanbul", 3);
        var records = new List<SalaryRecord>
        {
            SalaryRecord.Create(".NET Developer", "Teknoloji", "İstanbul", 3, 40000, [".NET"]),
            SalaryRecord.Create(".NET Developer", "Teknoloji", "İstanbul", 3, 50000, [".NET"]),
            SalaryRecord.Create(".NET Developer", "Teknoloji", "İstanbul", 3, 60000, [".NET"]),
            SalaryRecord.Create(".NET Developer", "Teknoloji", "İstanbul", 3, 70000, [".NET"]),
        };

        _cache.Setup(c => c.GetAsync<CareerLens.Shared.DTOs.Salary.SalaryBenchmarkDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CareerLens.Shared.DTOs.Salary.SalaryBenchmarkDto?)null);

        Mock.Get(_uow.Object.SalaryRecords)
            .Setup(r => r.GetBenchmarkDataAsync(query.Position, query.City, query.YearsOfExperience, It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(records);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.SampleCount.Should().Be(4);
        result.P25.Should().Be(40000);
        result.P50.Should().Be(50000);
        result.P75.Should().Be(60000);
        result.Position.Should().Be(".NET Developer");
    }

    [Fact]
    public async Task Handle_VeriBulunamadi_SifirDoner()
    {
        // Arrange
        var query = new GetSalaryBenchmarkQuery("Bilinmeyen Pozisyon", "Trabzon", 10);

        _cache.Setup(c => c.GetAsync<CareerLens.Shared.DTOs.Salary.SalaryBenchmarkDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CareerLens.Shared.DTOs.Salary.SalaryBenchmarkDto?)null);

        Mock.Get(_uow.Object.SalaryRecords)
            .Setup(r => r.GetBenchmarkDataAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SalaryRecord>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.SampleCount.Should().Be(0);
        result.P50.Should().Be(0);
    }

    [Fact]
    public async Task Handle_EnflasyonCarpaniVarsa_MaaslariDuzeltir()
    {
        // Arrange
        var query = new GetSalaryBenchmarkQuery(".NET Developer", "İstanbul", 3);
        var records = new List<SalaryRecord>
        {
            SalaryRecord.Create(".NET Developer", "Teknoloji", "İstanbul", 3, 100000, [".NET"]),
        };

        _cache.Setup(c => c.GetAsync<CareerLens.Shared.DTOs.Salary.SalaryBenchmarkDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((CareerLens.Shared.DTOs.Salary.SalaryBenchmarkDto?)null);

        Mock.Get(_uow.Object.SalaryRecords)
            .Setup(r => r.GetBenchmarkDataAsync(query.Position, query.City, query.YearsOfExperience, It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(records);

        _cpiIndex.Setup(c => c.GetAdjustmentFactorAsync(It.IsAny<DateTime>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1.2m);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.P50.Should().Be(120000);
        result.IsInflationAdjusted.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_CacheMevcut_DbSorgulamaz()
    {
        // Arrange
        var query = new GetSalaryBenchmarkQuery(".NET Developer", "İstanbul", 3);
        var cached = new CareerLens.Shared.DTOs.Salary.SalaryBenchmarkDto(30000, 45000, 60000, 10, ".NET Developer", "İstanbul", 3);

        _cache.Setup(c => c.GetAsync<CareerLens.Shared.DTOs.Salary.SalaryBenchmarkDto>(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cached);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.P50.Should().Be(45000);
        Mock.Get(_uow.Object.SalaryRecords)
            .Verify(r => r.GetBenchmarkDataAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
