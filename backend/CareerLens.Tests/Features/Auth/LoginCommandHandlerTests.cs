using CareerLens.Application.Common.Exceptions;
using CareerLens.Application.Common.Interfaces;
using CareerLens.Application.Features.Auth.Commands.Login;
using CareerLens.Domain.Entities;
using CareerLens.Domain.Interfaces;
using CareerLens.Tests.Helpers;
using FluentAssertions;
using Moq;

namespace CareerLens.Tests.Features.Auth;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _uow;
    private readonly Mock<IJwtService> _jwt;
    private readonly Mock<IPasswordHasher> _hasher;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _uow = MockUnitOfWork.Create();
        _jwt = new Mock<IJwtService>();
        _hasher = new Mock<IPasswordHasher>();
        _handler = new LoginCommandHandler(_uow.Object, _jwt.Object, _hasher.Object);
    }

    [Fact]
    public async Task Handle_GecerliKimlik_TokenDoner()
    {
        // Arrange
        var user = User.Create("test@example.com", "hashed_pw");
        var cmd = new LoginCommand("test@example.com", "Password1!");

        Mock.Get(_uow.Object.Users)
            .Setup(r => r.GetByEmailAsync(cmd.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _hasher.Setup(h => h.Verify(cmd.Password, user.PasswordHash)).Returns(true);
        _jwt.Setup(j => j.GenerateAccessToken(user)).Returns("access_token");
        _jwt.Setup(j => j.GenerateRefreshToken()).Returns("refresh_token");

        // Act
        var result = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        result.AccessToken.Should().Be("access_token");
        result.RefreshToken.Should().Be("refresh_token");
    }

    [Fact]
    public async Task Handle_YanlisParola_UnauthorizedFirlatir()
    {
        // Arrange
        var user = User.Create("test@example.com", "hashed_pw");
        var cmd = new LoginCommand("test@example.com", "YanlisParola");

        Mock.Get(_uow.Object.Users)
            .Setup(r => r.GetByEmailAsync(cmd.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _hasher.Setup(h => h.Verify(cmd.Password, user.PasswordHash)).Returns(false);

        // Act
        var act = async () => await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task Handle_YanlisEmail_UnauthorizedFirlatir()
    {
        // Arrange
        var cmd = new LoginCommand("yok@example.com", "Password1!");

        Mock.Get(_uow.Object.Users)
            .Setup(r => r.GetByEmailAsync(cmd.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var act = async () => await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>();
    }
}
