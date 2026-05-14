using CareerLens.Shared.DTOs.Auth;
using MediatR;

namespace CareerLens.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<AuthResponse>;
