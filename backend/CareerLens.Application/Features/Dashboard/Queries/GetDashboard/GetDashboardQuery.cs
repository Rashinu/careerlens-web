using CareerLens.Shared.DTOs.Dashboard;
using MediatR;

namespace CareerLens.Application.Features.Dashboard.Queries.GetDashboard;

public record GetDashboardQuery(Guid UserId) : IRequest<DashboardDto>;
