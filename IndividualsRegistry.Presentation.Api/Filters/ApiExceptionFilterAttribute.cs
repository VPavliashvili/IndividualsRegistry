using IndividualsRegistry.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IndividualsRegistry.Presentation.Api.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly Dictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ApiExceptionFilterAttribute()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(DoesNotExistException), HandleDoesNotExist },
            { typeof(AlreadyExistsException), HandleAlreadyExist },
        };
    }

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }

    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();
        if (_exceptionHandlers.TryGetValue(type, out Action<ExceptionContext>? action))
        {
            action.Invoke(context);
            return;
        }

        HandleUnknownException(context);
    }

    private static void HandleDoesNotExist(ExceptionContext context)
    {
        var details = new ProblemDetails()
        {
            Title = "The specified resource was not found.",
            Status = StatusCodes.Status404NotFound,
            Detail = context.Exception.Message,
        };

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }

    private static void HandleAlreadyExist(ExceptionContext context)
    {
        var details = new ProblemDetails()
        {
            Title = "The resource already exist",
            Status = StatusCodes.Status400BadRequest,
            Detail = context.Exception.Message,
        };

        context.Result = new NotFoundObjectResult(details);
        context.ExceptionHandled = true;
    }

    private static void HandleUnknownException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Title = "An error occurred while processing your request.",
            Status = StatusCodes.Status500InternalServerError,
            Detail = context.Exception.Message,
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError,
        };
        context.ExceptionHandled = true;
    }
}
