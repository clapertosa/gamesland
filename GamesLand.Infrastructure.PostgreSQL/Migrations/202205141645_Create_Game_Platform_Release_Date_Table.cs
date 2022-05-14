using System.Data;
using FluentMigrator;

namespace GamesLand.Infrastructure.PostgreSQL.Migrations;

[Migration(202205141645)]
public class Create_Game_Platform_Release_Date_Table : Migration {
    public override void Up()
    {
        Create.Table("games_release_date")
            .WithColumn("game_id").AsGuid().NotNullable().ForeignKey("games", "id").OnDelete(Rule.Cascade)
            .WithColumn("platform_id").AsGuid().NotNullable().ForeignKey("platforms", "id").OnDelete(Rule.Cascade)
            .WithColumn("release_date").AsDateTime2().Nullable()
            .WithColumn("created_at").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("updated_at").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime);
        Create.UniqueConstraint("IX_games_release_date_game_id_platform_id").OnTable("games_release_date").Columns("game_id", "platform_id");
    }

    public override void Down()
    {
        Delete.Table("games_release_date");
    }
}