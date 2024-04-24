using FluentValidation;

using PortfolioManager.Application.Contracts;

namespace PortfolioManager.Application.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(2);
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(2);
    }
}