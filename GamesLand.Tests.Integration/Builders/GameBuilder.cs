using System;
using GamesLand.Core.Games.Entities;

namespace GamesLand.Tests.Integration.Builders;

public class GameBuilder
{
    private readonly Game _game = new Game();

    public GameBuilder()
    {
    }

    public GameBuilder WithExternalId(int id)
    {
        _game.ExternalId = id;
        return this;
    }

    public GameBuilder WithName(string name)
    {
        _game.Name = name;
        return this;
    }

    public GameBuilder WithNameOriginal(string nameOriginal)
    {
        _game.NameOriginal = nameOriginal;
        return this;
    }

    public GameBuilder WithDescription(string description)
    {
        _game.Description = description;
        return this;
    }

    public GameBuilder WithReleased(DateTime released)
    {
        _game.Released = released;
        return this;
    }

    public GameBuilder WithUpdated(DateTime updated)
    {
        _game.Updated = updated;
        return this;
    }

    public GameBuilder WithToBeAnnounced(bool tba)
    {
        _game.ToBeAnnounced = tba;
        return this;
    }

    public GameBuilder WithBackgroundImagePath(string backgroundPath)
    {
        _game.BackgroundImagePath = backgroundPath;
        return this;
    }

    public GameBuilder WithBackgroundImageAdditionalPath(string backgroundPath)
    {
        _game.BackgroundImageAdditionalPath = backgroundPath;
        return this;
    }

    public GameBuilder WithWebsite(string website)
    {
        _game.Website = website;
        return this;
    }

    public GameBuilder WithRating(double rating)
    {
        _game.Rating = rating;
        return this;
    }

    public GameBuilder WithRatingsCount(int ratingsCount)
    {
        _game.RatingsCount = ratingsCount;
        return this;
    }

    public Game Build() => _game;
}