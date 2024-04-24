using FluentResults;

namespace PortfolioManager.Application.Common;

public class NotFound(string message) : Error(message)
{
    public static Result ToResult(string message) =>
        Result.Fail(new NotFound(message));
}