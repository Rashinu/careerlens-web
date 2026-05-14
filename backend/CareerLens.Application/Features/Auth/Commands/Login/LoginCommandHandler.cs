using CareerLens.Application.Common.Exceptions;
using CareerLens.Application.Common.Interfaces;
using CareerLens.Domain.Interfaces;
using CareerLens.Shared.Constants;
using CareerLens.Shared.DTOs.Auth;
using MediatR;

namespace CareerLens.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(IUnitOfWork uow, IJwtService jwtService, IPasswordHasher passwordHasher)
    {
        _uow = uow;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _uow.Users.GetByEmailAsync(request.Email, ct)
            ?? throw new UnauthorizedException("Email veya şifre hatalı.");

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Email veya şifre hatalı.");

        var refreshToken = _jwtService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddDays(AppConstants.Jwt.RefreshTokenExpiryDays);
        user.SetRefreshToken(refreshToken, expiresAt);

        _uow.Users.Update(user);
        await _uow.SaveChangesAsync(ct);

        var accessToken = _jwtService.GenerateAccessToken(user);
        return new AuthResponse(accessToken, refreshToken, DateTime.UtcNow.AddMinutes(AppConstants.Jwt.AccessTokenExpiryMinutes));
    }
}
