using CareerLens.Shared.DTOs.Auth;
using MediatR;

namespace CareerLens.Application.Features.Auth.Commands.Register;

public record RegisterCommand(string Email, string Password, string? FirstName, string? LastName) : IRequest<AuthResponse>;
