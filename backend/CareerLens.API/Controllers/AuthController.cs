using CareerLens.Application.Features.Auth.Commands.Login;
using CareerLens.Application.Features.Auth.Commands.RefreshToken;
using CareerLens.Application.Features.Auth.Commands.Register;
using CareerLens.Shared.DTOs.Auth;
using CareerLens.Shared.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CareerLens.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new RegisterCommand(request.Email, request.Password, request.FirstName, request.LastName), ct);
        return Ok(ApiResponse<AuthResponse>.Ok(result));
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new LoginCommand(request.Email, request.Password), ct);
        return Ok(ApiResponse<AuthResponse>.Ok(result));
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Refresh([FromBody] RefreshTokenRequest request, CancellationToken ct)
    {
        var result = await _mediator.Send(new RefreshTokenCommand(request.RefreshToken), ct);
        return Ok(ApiResponse<AuthResponse>.Ok(result));
    }
}
