﻿using GamesLand.Core;
using GamesLand.Core.Users;

namespace GamesLand.Web.Users.Responses;

public record UserResponse : BaseEntity<Guid>
{
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; }

    public static UserResponse FromUser(User user)
    {
        return new UserResponse()
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}