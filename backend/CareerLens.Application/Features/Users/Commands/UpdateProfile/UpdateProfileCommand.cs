using CareerLens.Shared.DTOs.User;
using MediatR;

namespace CareerLens.Application.Features.Users.Commands.UpdateProfile;

public record UpdateProfileCommand(Guid UserId, string? FirstName, string? LastName) : IRequest<UserProfileDto>;
