using FluentValidation;
using IndividualsRegistry.Application.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace IndividualsRegistry.Application.Individuals.Commands.AddRelatedIndividual;

public class AddRelatedIndividualCommandValidator : AbstractValidator<AddRelatedIndividualCommand>
{
    public AddRelatedIndividualCommandValidator(
        IStringLocalizer<AddRelatedIndividualCommandValidator> localizer
    )
    {
        RuleFor(x => x.individualId)
            .GreaterThan(0)
            .WithMessage(x => localizer[LocalizationKeys.InvalidDbId])
            .WithErrorCode(StatusCodes.Status400BadRequest.ToString());

        RuleFor(x => x.relatedIndividualId)
            .GreaterThan(0)
            .WithMessage(x => localizer[LocalizationKeys.InvalidDbId])
            .WithErrorCode(StatusCodes.Status400BadRequest.ToString());
    }
}
