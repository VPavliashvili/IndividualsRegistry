using System.Text.RegularExpressions;
using FluentValidation;
using IndividualsRegistry.Application.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace IndividualsRegistry.Application.Individuals.Commands.EditIndividual;

public class EditIndividualCommandValidator : AbstractValidator<EditIndividualCommand>
{
    public EditIndividualCommandValidator(
        IStringLocalizer<EditIndividualCommandValidator> localizer
    )
    {
        RuleFor(x => x.request.Name)
            .NotNull()
            .Length(2, 50)
            .Must(isValidWord)
            .WithMessage(localizer[LocalizationKeys.InvalidName])
            .WithErrorCode(StatusCodes.Status400BadRequest.ToString());
        RuleFor(x => x.request.Surname)
            .NotNull()
            .Length(2, 50)
            .Must(isValidWord)
            .WithMessage(localizer[LocalizationKeys.InvalidSurame])
            .WithErrorCode(StatusCodes.Status400BadRequest.ToString());
        RuleFor(x => x.request.PersonalId)
            .NotNull()
            .Length(11)
            .WithMessage(localizer[LocalizationKeys.InvalidPersonalId])
            .WithErrorCode(StatusCodes.Status400BadRequest.ToString());
        RuleFor(x => x.request.BirthDate)
            .Must(x => DateOnly.FromDateTime(DateTime.Now).Year - x?.Year >= 18)
            .WithMessage(localizer[LocalizationKeys.InvalidBirthDate])
            .WithErrorCode(StatusCodes.Status400BadRequest.ToString());
        RuleFor(x => x.request.PhoneNumbers)
            .ForEach(x => x.Must(y => y.Number.Length >= 4 && y.Number.Length <= 50))
            .WithMessage(localizer[LocalizationKeys.InvalidPhoneNumber])
            .WithErrorCode(StatusCodes.Status400BadRequest.ToString());

        static bool isValidWord(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            const string geoGex = @"^[\u10A0-\u10FF]+$";
            const string latGex = @"^[a-zA-Z]+$";

            var georgianRegex = new Regex(geoGex);
            var latinRegex = new Regex(latGex);

            return georgianRegex.IsMatch(input) || latinRegex.IsMatch(input);
        }
    }
}

