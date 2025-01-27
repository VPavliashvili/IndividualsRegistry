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
            { typeof(RelationDoesNotExistException), HandleRelationDoesNotExist },
            { typeof(RelatedIndividualAlreadyExists), HandleRelatedIndividualExist },
            { typeof(PageNumberOverflowException), HandlePageNumberOverflow },
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

    private static void HandlePageNumberOverflow(ExceptionContext context)
    {
        var ex = (PageNumberOverflowException)context.Exception;

        var details = new ProblemDetails()
        {
            Title = "The specified resource was not found.",
            Status = StatusCodes.Status404NotFound,
            Detail =
                $"More pages has been requested than it exists in this request scope, maxPage: {ex.MaxPage}, requestedPage: {ex.GivenPage}, overallFilteredRecords: {ex.FilteredCount}",
        };

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }

    private static void HandleRelatedIndividualExist(ExceptionContext context)
    {
        var ex = (RelatedIndividualAlreadyExists)context.Exception;
        var details = new ProblemDetails()
        {
            Title = "The specified resource was not found.",
            Status = StatusCodes.Status404NotFound,
            Detail =
                $"For individual {ex.IndividualId} related individual already exists with Id {ex.RelatedIndividualId} in database",
        };

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }

    private static void HandleRelationDoesNotExist(ExceptionContext context)
    {
        var ex = (RelationDoesNotExistException)context.Exception;
        var details = new ProblemDetails()
        {
            Title = "The specified resource was not found.",
            Status = StatusCodes.Status404NotFound,
            Detail =
                $"For individual {ex.IndividualId} related individual does not exist with Id {ex.RelatedIndividualId} in database",
        };

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }

    private static void HandleDoesNotExist(ExceptionContext context)
    {
        var ex = (DoesNotExistException)context.Exception;
        var details = new ProblemDetails()
        {
            Title = "The specified resource was not found.",
            Status = StatusCodes.Status404NotFound,
            Detail = $"Individual does not exist with Id {ex.IndividualId} in database",
        };

        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }

    private static void HandleAlreadyExist(ExceptionContext context)
    {
        var ex = (AlreadyExistsException)context.Exception;
        var details = new ProblemDetails()
        {
            Title = "The resource already exist",
            Status = StatusCodes.Status400BadRequest,
            Detail = $"Individual with Id {ex.IndividualId} already exists database",
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
