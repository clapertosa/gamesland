using GamesLand.Core.Users.Entities;

namespace GamesLand.Web.Users.Requests;

public record SignInUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }

    public User ToUser()
    {
        return new User { Email = Email, Password = Password };
    }
}