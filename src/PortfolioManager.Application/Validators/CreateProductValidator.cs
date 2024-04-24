using FluentValidation;

using PortfolioManager.Application.Contracts;

namespace PortfolioManager.Application.Validators;

public class CreateProductValidator : AbstractValidator<CreateProductRequest>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty();
        RuleFor(x => x.DueAt)
            .NotEmpty()
            .Must(x => x > DateTime.Now.Date);
        RuleFor(x => x.ManagerEmail)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.MinimumInvestmentAmount)
            .NotEmpty()
            .GreaterThan(10);
    }
}