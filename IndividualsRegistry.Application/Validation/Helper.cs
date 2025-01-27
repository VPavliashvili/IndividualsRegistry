using FluentValidation;
using IndividualsRegistry.Application.Models.Exceptions;

namespace IndividualsRegistry.Application.Validation;

public static class Helper
{
    public static async Task ValidateAndCustomException<T>(
        this IValidator<T> validator,
        T instance
    )
    {
        var res = await validator.ValidateAsync(instance);

        if (!res.IsValid)
        {
            var ex = new ValidationException(res.Errors);
            throw new UserInputValidation(ex.Message);
        }
    }
}

