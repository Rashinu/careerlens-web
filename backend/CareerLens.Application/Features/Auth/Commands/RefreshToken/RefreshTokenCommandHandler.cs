using CareerLens.Application.Common.Exceptions;
using CareerLens.Application.Common.Interfaces;
using CareerLens.Domain.Interfaces;
using CareerLens.Shared.Constants;
using CareerLens.Shared.DTOs.Auth;
using MediatR;

namespace CareerLens.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResponse>
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtService _jwtService;

    public RefreshTokenCommandHandler(IUnitOfWork uow, IJwtService jwtService)
    {
        _uow = uow;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var user = await _uow.Users.GetByRefreshTokenAsync(request.RefreshToken, ct)
            ?? throw new UnauthorizedException("Geçersiz refresh token.");

        if (user.RefreshTokenExpiresAt < DateTime.UtcNow)
            throw new UnauthorizedException("Refresh token süresi dolmuş.");

        var newRefreshToken = _jwtService.GenerateRefreshToken();
        user.SetRefreshToken(newRefreshToken, DateTime.UtcNow.AddDays(AppConstants.Jwt.RefreshTokenExpiryDays));

        _uow.Users.Update(user);
        await _uow.SaveChangesAsync(ct);

        var accessToken = _jwtService.GenerateAccessToken(user);
        return new AuthResponse(accessToken, newRefreshToken, DateTime.UtcNow.AddMinutes(AppConstants.Jwt.AccessTokenExpiryMinutes));
    }
}
