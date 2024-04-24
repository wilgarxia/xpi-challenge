using FluentResults;

namespace PortfolioManager.Application.Common;

public class ValidationError : Error
{
    public ValidationError(string propertyName, string errorMessage) : base(errorMessage)
    {
        Metadata["PropertyName"] = propertyName;
        Metadata["ErrorMessage"] = errorMessage;
    }

    public string? PropertyName => Metadata["PropertyName"] as string;
    public string? ErrorMessage => Metadata["ErrorMessage"] as string;
}