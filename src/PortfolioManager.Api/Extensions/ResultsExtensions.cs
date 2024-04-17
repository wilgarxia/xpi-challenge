using FluentResults;

using Microsoft.AspNetCore.Mvc;

using PortfolioManager.Application.Common;

namespace PortfolioManager.Api.Extensions;

public static class ResultsExtensions
{
    public static ProblemDetails ToProblem<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException();

        if (result.Errors.FirstOrDefault() is ValidationError)
        {
            var validations = result.Errors
                .OfType<ValidationError>()
                .Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage
                })
                .ToList();

            var validationProblem = new ProblemDetails
            {
                Title = "Validation Error",
                Detail = "Validation errors have occurred.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Status = StatusCodes.Status400BadRequest
            };

            validationProblem.Extensions.Add("errors", validations);

            return validationProblem;
        }

        var problem = new ProblemDetails
        {
            Title = "Bad Request",
            Detail = result.Errors.FirstOrDefault()?.Message,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Status = StatusCodes.Status400BadRequest
        };

        return problem;
    }
}