using CareerLens.Application.Common.Interfaces;
using CareerLens.Domain.Entities;
using CareerLens.Domain.Interfaces;
using CareerLens.Shared.Constants;
using CareerLens.Shared.DTOs.Auth;
using MediatR;

namespace CareerLens.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IUnitOfWork uow, IJwtService jwtService, IPasswordHasher passwordHasher)
    {
        _uow = uow;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken ct)
    {
        if (await _uow.Users.ExistsAsync(request.Email, ct))
            throw new InvalidOperationException("Bu email adresi zaten kayıtlı.");

        var passwordHash = _passwordHasher.Hash(request.Password);
        var user = User.Create(request.Email, passwordHash);
        user.UpdateProfile(request.FirstName, request.LastName);

        var refreshToken = _jwtService.GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddDays(AppConstants.Jwt.RefreshTokenExpiryDays);
        user.SetRefreshToken(refreshToken, expiresAt);

        await _uow.Users.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        var accessToken = _jwtService.GenerateAccessToken(user);
        return new AuthResponse(accessToken, refreshToken, DateTime.UtcNow.AddMinutes(AppConstants.Jwt.AccessTokenExpiryMinutes));
    }
}
