using FluentValidation;
using UserProvider.Entities;

namespace UserProvider.Helpers;

public class UserValidator : AbstractValidator<AspNetUser>
{
    public UserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
