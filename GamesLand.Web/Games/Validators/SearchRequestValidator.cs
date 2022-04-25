using FluentValidation;
using GamesLand.Infrastructure.RAWG.Requests;

namespace GamesLand.Web.Games.Validators;

public class SearchRequestValidator : AbstractValidator<SearchRequest>
{
    public SearchRequestValidator()
    {
        RuleFor(x => x.Name).NotNull();
    }
}