using CareerLens.Application.Common.Interfaces;
using CareerLens.Application.Features.Auth.Commands.Register;
using CareerLens.Domain.Entities;
using CareerLens.Domain.Interfaces;
using CareerLens.Tests.Helpers;
using FluentAssertions;
using Moq;

namespace CareerLens.Tests.Features.Auth;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _uow;
    private readonly Mock<IJwtService> _jwt;
    private readonly Mock<IPasswordHasher> _hasher;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _uow = MockUnitOfWork.Create();
        _jwt = new Mock<IJwtService>();
        _hasher = new Mock<IPasswordHasher>();
        _handler = new RegisterCommandHandler(_uow.Object, _jwt.Object, _hasher.Object);
    }

    [Fact]
    public async Task Handle_YeniKullanici_TokenDoner()
    {
        // Arrange
        var cmd = new RegisterCommand("test@example.com", "Password1!", "Test", "User");

        Mock.Get(_uow.Object.Users)
            .Setup(r => r.ExistsAsync(cmd.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _hasher.Setup(h => h.Hash(cmd.Password)).Returns("hashed_password");
        _jwt.Setup(j => j.GenerateAccessToken(It.IsAny<User>())).Returns("access_token");
        _jwt.Setup(j => j.GenerateRefreshToken()).Returns("refresh_token");

        // Act
        var result = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        result.AccessToken.Should().Be("access_token");
        result.RefreshToken.Should().Be("refresh_token");

        Mock.Get(_uow.Object.Users)
            .Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        _uow.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_VarOlanEmail_ExceptionFirlatir()
    {
        // Arrange
        var cmd = new RegisterCommand("varolan@example.com", "Password1!", null, null);

        Mock.Get(_uow.Object.Users)
            .Setup(r => r.ExistsAsync(cmd.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var act = async () => await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*zaten kayıtlı*");
    }
}
