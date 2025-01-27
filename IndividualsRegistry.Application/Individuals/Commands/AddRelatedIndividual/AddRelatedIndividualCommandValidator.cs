using FluentValidation;
using Microsoft.Extensions.Localization;

namespace IndividualsRegistry.Application.Individuals.Commands.AddRelatedIndividual;

public class AddRelatedIndividualCommandValidator : AbstractValidator<AddRelatedIndividualCommand>
{
    public AddRelatedIndividualCommandValidator(
        IStringLocalizer<AddRelatedIndividualCommandValidator> localizer
    )
    {
        RuleFor(x => x.individualId).GreaterThan(0).WithMessage(x => localizer["InvalidIdValue"]);
        RuleFor(x => x.relatedIndividualId)
            .GreaterThan(0)
            .WithMessage(x => localizer["InvalidIdValue"]);
    }
}
