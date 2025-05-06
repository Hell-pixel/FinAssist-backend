using FinAssist.Domain.Dtos.Account;
using FluentValidation;

namespace FinAssist.Infrastructure.Validation;

public class CreateUserRequestValidator : AbstractValidator<CreateUserDto>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}