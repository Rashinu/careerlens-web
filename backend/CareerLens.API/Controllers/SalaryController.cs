using CareerLens.Application.Features.Salary.Commands.SubmitSalary;
using CareerLens.Application.Features.Salary.Queries.GetBenchmark;
using CareerLens.Shared.DTOs.Salary;
using CareerLens.Shared.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CareerLens.API.Controllers;

[ApiController]
[Route("api/salary")]
public class SalaryController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalaryController(IMediator mediator) => _mediator = mediator;

    [HttpGet("benchmark")]
    public async Task<ActionResult<ApiResponse<SalaryBenchmarkDto>>> GetBenchmark(
        [FromQuery] string position,
        [FromQuery] string city,
        [FromQuery] int years,
        CancellationToken ct)
    {
        var result = await _mediator.Send(new GetSalaryBenchmarkQuery(position, city, years), ct);
        return Ok(ApiResponse<SalaryBenchmarkDto>.Ok(result));
    }

    [HttpPost("submit")]
    public async Task<ActionResult<ApiResponse<Guid>>> Submit([FromBody] SubmitSalaryRequest request, CancellationToken ct)
    {
        var id = await _mediator.Send(new SubmitSalaryCommand(
            request.Position, request.Sector, request.City,
            request.YearsOfExperience, request.NetSalary, request.TechStack), ct);
        return Ok(ApiResponse<Guid>.Ok(id));
    }
}
