using FluentValidation;
using GamesLand.Web.Users.Requests;

namespace GamesLand.Web.Users.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email).EmailAddress().NotNull();
        RuleFor(x => x.Password).MinimumLength(8).NotNull();
    }
}