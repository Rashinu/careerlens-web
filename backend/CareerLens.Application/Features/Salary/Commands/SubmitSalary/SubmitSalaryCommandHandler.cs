using CareerLens.Domain.Entities;
using CareerLens.Domain.Interfaces;
using MediatR;

namespace CareerLens.Application.Features.Salary.Commands.SubmitSalary;

public class SubmitSalaryCommandHandler : IRequestHandler<SubmitSalaryCommand, Guid>
{
    private readonly IUnitOfWork _uow;

    public SubmitSalaryCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Guid> Handle(SubmitSalaryCommand request, CancellationToken ct)
    {
        var record = SalaryRecord.Create(
            request.Position,
            request.Sector,
            request.City,
            request.YearsOfExperience,
            request.NetSalary,
            request.TechStack);

        await _uow.SalaryRecords.AddAsync(record, ct);
        await _uow.SaveChangesAsync(ct);

        return record.Id;
    }
}
