using System.Data;
using FluentMigrator;

namespace GamesLand.Infrastructure.PostgreSQL.Migrations;

[Migration(202204231103)]
public class User_Game_Table : Migration {
    public override void Up()
    {
        Create.Table("user_game")
            .WithColumn("user_id").AsGuid().ForeignKey("users", "id").OnDelete(Rule.Cascade)
            .WithColumn("game_id").AsGuid().ForeignKey("games", "id").OnDelete(Rule.Cascade)
            .WithColumn("created_at").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("updated_at").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime);
    }

    public override void Down()
    {
        Delete.Table("user_game");
    }
}