using CareerLens.Application.Common.Exceptions;
using CareerLens.Domain.Interfaces;
using CareerLens.Shared.DTOs.User;
using MediatR;

namespace CareerLens.Application.Features.Users.Queries.GetProfile;

public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserProfileDto>
{
    private readonly IUnitOfWork _uow;

    public GetProfileQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<UserProfileDto> Handle(GetProfileQuery request, CancellationToken ct)
    {
        var user = await _uow.Users.GetByIdAsync(request.UserId, ct)
            ?? throw new NotFoundException("User", request.UserId);

        return new UserProfileDto(user.Id, user.Email, user.FirstName, user.LastName, user.Plan.ToString(), user.CreatedAt);
    }
}
