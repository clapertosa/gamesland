using GamesLand.Core.Users;
using GamesLand.Core.Users.Entities;

namespace GamesLand.Web.Users.Requests;

public record SignUpUserRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public User ToUser()
    {
        return new User
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Password = Password
        };
    }
}