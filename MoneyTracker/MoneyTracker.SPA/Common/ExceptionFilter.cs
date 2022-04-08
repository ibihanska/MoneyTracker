using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MoneyTracker.Application.Common.Exceptions;
using MoneyTracker.Domain.Common;

namespace MoneyTracker.SPA.Common
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            switch (exception)
            {
                case NotFoundException notFoundException:
                    var details = new ProblemDetails
                    {
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                        Title = "The specified resource was not found.",
                        Detail = notFoundException.Message
                    };
                    context.Result = new NotFoundObjectResult(details);
                    context.ExceptionHandled = true;
                    break;
                case ValidationException validationException:
                    var validationdetails = new ValidationProblemDetails(validationException.Failures.ToDictionary(x => x.Key, x => x.Value))
                    {
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        Title = "One or more validation errors occurred.",
                        Detail = validationException.Message
                    };
                    context.Result = new BadRequestObjectResult(validationdetails);
                    context.ExceptionHandled = true;
                    break;
                case ConflictException conflictException:
                    var badRequestdetails = new ProblemDetails
                    {
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                        Title = "Operation is not allowed.",
                        Detail = conflictException.Message
                    };
                    context.Result = new ObjectResult(badRequestdetails)
                    {
                        StatusCode = StatusCodes.Status409Conflict
                    };
                    context.ExceptionHandled = true;
                    break;

            }
        }
    }
}


