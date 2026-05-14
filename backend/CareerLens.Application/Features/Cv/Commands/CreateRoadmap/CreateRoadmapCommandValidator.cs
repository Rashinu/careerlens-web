using FluentValidation;

namespace CareerLens.Application.Features.Cv.Commands.CreateRoadmap;

public class CreateRoadmapCommandValidator : AbstractValidator<CreateRoadmapCommand>
{
    public CreateRoadmapCommandValidator()
    {
        RuleFor(x => x.TargetPosition)
            .NotEmpty().WithMessage("Hedef pozisyon zorunludur.")
            .MaximumLength(200);
    }
}
