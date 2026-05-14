using CareerLens.API.Extensions;
using CareerLens.Application.Features.Cv.Commands.CreateRoadmap;
using CareerLens.Application.Features.Cv.Commands.UploadCv;
using CareerLens.Application.Features.Cv.Queries.GetCvAnalysis;
using CareerLens.Application.Features.Cv.Queries.GetCvList;
using CareerLens.Application.Features.Cv.Queries.GetRoadmap;
using CareerLens.Shared.Constants;
using CareerLens.Shared.DTOs.Cv;
using CareerLens.Shared.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CareerLens.API.Controllers;

[ApiController]
[Route("api/cv")]
[Authorize]
public class CvController : ControllerBase
{
    private readonly IMediator _mediator;

    public CvController(IMediator mediator) => _mediator = mediator;

    [HttpPost("upload")]
    [RequestSizeLimit(6 * 1024 * 1024)]
    public async Task<ActionResult<ApiResponse<CvAnalysisListItemDto>>> Upload(IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
            return BadRequest(ApiResponse<CvAnalysisListItemDto>.Fail("Dosya seçilmedi."));

        var userId = User.GetUserId();

        using var stream = file.OpenReadStream();
        var result = await _mediator.Send(new UploadCvCommand(
            userId,
            file.FileName,
            stream,
            file.ContentType,
            file.Length), ct);

        return Ok(ApiResponse<CvAnalysisListItemDto>.Ok(result));
    }

    [HttpGet("list")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<CvAnalysisListItemDto>>>> List(CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await _mediator.Send(new GetCvListQuery(userId), ct);
        return Ok(ApiResponse<IReadOnlyList<CvAnalysisListItemDto>>.Ok(result));
    }

    [HttpGet("{id:guid}/analysis")]
    public async Task<ActionResult<ApiResponse<CvAnalysisDto>>> GetAnalysis(Guid id, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await _mediator.Send(new GetCvAnalysisQuery(id, userId), ct);
        return Ok(ApiResponse<CvAnalysisDto>.Ok(result));
    }

    [HttpPost("{id:guid}/roadmap")]
    public async Task<ActionResult<ApiResponse<RoadmapDto>>> CreateRoadmap(
        Guid id,
        [FromBody] CreateRoadmapRequest request,
        CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await _mediator.Send(new CreateRoadmapCommand(id, userId, request.TargetPosition), ct);
        return Ok(ApiResponse<RoadmapDto>.Ok(result));
    }

    [HttpGet("{id:guid}/roadmap")]
    public async Task<ActionResult<ApiResponse<RoadmapDto>>> GetRoadmap(Guid id, CancellationToken ct)
    {
        var userId = User.GetUserId();
        var result = await _mediator.Send(new GetRoadmapQuery(id, userId), ct);
        return Ok(ApiResponse<RoadmapDto>.Ok(result));
    }
}
