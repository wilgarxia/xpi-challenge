using FluentResults;

using FluentValidation.Results;

using PortfolioManager.Application.Common;

namespace PortfolioManager.Application.Extensions;

public static class ValidationExtensions
{
    public static Result Fail(this ValidationResult validationResult)
    {
        var result = new Result();

        foreach (var item in validationResult.Errors)
        {
            result.WithError(new ValidationError(item.PropertyName, item.ErrorMessage));
        }

        return result;
    }
}
