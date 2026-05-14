using CareerLens.Domain.Interfaces;
using Moq;

namespace CareerLens.Tests.Helpers;

public static class MockUnitOfWork
{
    public static Mock<IUnitOfWork> Create()
    {
        var uow = new Mock<IUnitOfWork>();
        uow.Setup(u => u.Users).Returns(new Mock<IUserRepository>().Object);
        uow.Setup(u => u.CvAnalyses).Returns(new Mock<ICvAnalysisRepository>().Object);
        uow.Setup(u => u.SalaryRecords).Returns(new Mock<ISalaryRecordRepository>().Object);
        uow.Setup(u => u.CareerRoadmaps).Returns(new Mock<ICareerRoadmapRepository>().Object);
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        return uow;
    }
}
