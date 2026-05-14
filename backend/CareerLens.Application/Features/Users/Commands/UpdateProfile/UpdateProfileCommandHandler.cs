using CareerLens.Application.Common.Exceptions;
using CareerLens.Domain.Interfaces;
using CareerLens.Shared.DTOs.User;
using MediatR;

namespace CareerLens.Application.Features.Users.Commands.UpdateProfile;

public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, UserProfileDto>
{
    private readonly IUnitOfWork _uow;

    public UpdateProfileCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<UserProfileDto> Handle(UpdateProfileCommand request, CancellationToken ct)
    {
        var user = await _uow.Users.GetByIdAsync(request.UserId, ct)
            ?? throw new NotFoundException("User", request.UserId);

        user.UpdateProfile(request.FirstName, request.LastName);
        _uow.Users.Update(user);
        await _uow.SaveChangesAsync(ct);

        return new UserProfileDto(user.Id, user.Email, user.FirstName, user.LastName, user.Plan.ToString(), user.CreatedAt);
    }
}
