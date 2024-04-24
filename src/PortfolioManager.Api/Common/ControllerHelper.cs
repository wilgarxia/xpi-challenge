using ErrorOr;

using Microsoft.AspNetCore.Mvc;

namespace PortfolioManager.Api.Common;

public static class ControllerHelper
{
    public static IActionResult Problem(Error error)
    {
        var validationProblem = new ProblemDetails
        {
            Title = error.Type switch
            {
                ErrorType.Conflict => "Bad Request",
                ErrorType.Validation => "Bad Request",
                ErrorType.NotFound => "Not Found",
                _ => "Internal Server Error",
            },
            Detail = error.Description,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            Status = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError,
            }
        };

        return error.Type switch
        {
            ErrorType.Conflict => new BadRequestObjectResult(validationProblem),
            ErrorType.Validation => new BadRequestObjectResult(validationProblem),
            ErrorType.NotFound => new NotFoundObjectResult(validationProblem),
            _ => new ObjectResult(validationProblem),
        };
    }
}