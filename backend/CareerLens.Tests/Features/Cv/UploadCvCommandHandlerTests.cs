using CareerLens.Application.Common.Interfaces;
using CareerLens.Application.Features.Cv.Commands.UploadCv;
using CareerLens.Domain.Entities;
using CareerLens.Domain.Interfaces;
using CareerLens.Tests.Helpers;
using FluentAssertions;
using Moq;

namespace CareerLens.Tests.Features.Cv;

public class UploadCvCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _uow;
    private readonly Mock<IBlobStorageService> _blob;
    private readonly Mock<ICvProcessingJobService> _jobService;
    private readonly UploadCvCommandHandler _handler;

    public UploadCvCommandHandlerTests()
    {
        _uow = MockUnitOfWork.Create();
        _blob = new Mock<IBlobStorageService>();
        _jobService = new Mock<ICvProcessingJobService>();
        _handler = new UploadCvCommandHandler(_uow.Object, _blob.Object, _jobService.Object);
    }

    [Fact]
    public async Task Handle_GecerliDosya_CvAnalysisOlusturur()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = User.Create("test@example.com", "hash");
        var cmd = new UploadCvCommand(userId, "cv.pdf", Stream.Null, "application/pdf", 1024);

        Mock.Get(_uow.Object.Users)
            .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _blob.Setup(b => b.UploadAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("blob/cv.pdf");

        // Act
        var result = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        result.OriginalFileName.Should().Be("cv.pdf");
        result.Status.Should().Be("Uploaded");

        Mock.Get(_uow.Object.CvAnalyses)
            .Verify(r => r.AddAsync(It.IsAny<CvAnalysis>(), It.IsAny<CancellationToken>()), Times.Once);

        _jobService.Verify(j => j.EnqueueTextExtraction(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_KullaniciBulunamadi_ExceptionFirlatir()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var cmd = new UploadCvCommand(userId, "cv.pdf", Stream.Null, "application/pdf", 1024);

        Mock.Get(_uow.Object.Users)
            .Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var act = async () => await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Application.Common.Exceptions.NotFoundException>();
    }
}
