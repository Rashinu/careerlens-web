using CareerLens.Shared.DTOs.User;
using MediatR;

namespace CareerLens.Application.Features.Users.Queries.GetProfile;

public record GetProfileQuery(Guid UserId) : IRequest<UserProfileDto>;
