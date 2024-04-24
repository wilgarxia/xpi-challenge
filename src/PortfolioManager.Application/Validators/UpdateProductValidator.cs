using FluentValidation;

using PortfolioManager.Application.Contracts;

namespace PortfolioManager.Application.Validators;

public class UpdateProductValidator : AbstractValidator<UpdateProductRequest>
{
    public UpdateProductValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty();
        RuleFor(x => x.MinimumInvestmentAmount)
            .NotEmpty()
            .GreaterThan(10);
    }
}