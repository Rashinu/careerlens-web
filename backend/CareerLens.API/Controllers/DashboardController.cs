using CareerLens.API.Extensions;
using CareerLens.Application.Features.Dashboard.Queries.GetDashboard;
using CareerLens.Shared.DTOs.Dashboard;
using CareerLens.Shared.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerLens.API.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<ActionResult<ApiResponse<DashboardDto>>> Get(CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await _mediator.Send(new GetDashboardQuery(userId), ct);
        return Ok(ApiResponse<DashboardDto>.Ok(result));
    }
}
