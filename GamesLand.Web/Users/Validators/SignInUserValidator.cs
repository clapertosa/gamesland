using FluentValidation;
using GamesLand.Web.Users.Requests;

namespace GamesLand.Web.Users.Validators;

public class SignInUserValidator : AbstractValidator<SignInUserRequest>
{
    public SignInUserValidator()
    {
        RuleFor(x => x.Email).EmailAddress().NotNull();
        RuleFor(x => x.Password).NotNull();
    }
}