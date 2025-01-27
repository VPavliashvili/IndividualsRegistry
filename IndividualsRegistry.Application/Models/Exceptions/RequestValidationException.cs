using Microsoft.AspNetCore.Http;

namespace IndividualsRegistry.Application.Models.Exceptions;

public class UserInputValidation : Exception
{
    public static readonly int StatusCode = StatusCodes.Status400BadRequest;

    public UserInputValidation(string message)
        : base(message) { }
}
