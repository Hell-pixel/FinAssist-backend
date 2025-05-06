using FinAssist.Domain.Dtos.Account;
using FluentValidation;

namespace FinAssist.Infrastructure.Validation;

public class LoginUserRequestValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}