using System.Data;
using FluentMigrator;

namespace GamesLand.Infrastructure.PostgreSQL.Migrations;

[Migration(202204231712)]
public class Alter_UserGame_Table_Add_Preferred_Platform_And_Email : Migration {
    public override void Up()
    {
        Alter.Table("user_game")
            .AddColumn("platform_id").AsGuid().NotNullable().ForeignKey("platforms", "id").OnDelete(Rule.Cascade)
            .AddColumn("notified").AsBoolean().NotNullable().WithDefaultValue(false);
        Create.UniqueConstraint("IX_user_game_user_id_game_id_platform_id").OnTable("user_game").Columns("user_id","game_id", "platform_id");
    }

    public override void Down()
    {
        Delete
            .Column("platform_id")
            .Column("notified")
            .FromTable("user_game");
    }
}