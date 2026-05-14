using CareerLens.API.Extensions;
using CareerLens.Application.Features.Users.Commands.UpdateProfile;
using CareerLens.Application.Features.Users.Queries.GetProfile;
using CareerLens.Shared.DTOs.User;
using CareerLens.Shared.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerLens.API.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator) => _mediator = mediator;

    [HttpGet("me")]
    public async Task<ActionResult<ApiResponse<UserProfileDto>>> GetProfile(CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await _mediator.Send(new GetProfileQuery(userId), ct);
        return Ok(ApiResponse<UserProfileDto>.Ok(result));
    }

    [HttpPut("me")]
    public async Task<ActionResult<ApiResponse<UserProfileDto>>> UpdateProfile([FromBody] UpdateProfileRequest request, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await _mediator.Send(new UpdateProfileCommand(userId, request.FirstName, request.LastName), ct);
        return Ok(ApiResponse<UserProfileDto>.Ok(result));
    }
}
