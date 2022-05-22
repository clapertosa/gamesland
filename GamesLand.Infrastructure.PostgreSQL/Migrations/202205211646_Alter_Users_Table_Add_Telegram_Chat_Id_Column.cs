using FluentMigrator;

namespace GamesLand.Infrastructure.PostgreSQL.Migrations;

[Migration(202205211646)]
public class Alter_Users_Table_Add_Telegram_Chat_Id_Column : Migration {
    public override void Up()
    {
        Alter.Table("users")
            .AddColumn("telegram_chat_id")
            .AsInt64()
            .WithDefaultValue(0);

        Alter.Column("telegram_chat_id").OnTable("users").AsInt64().NotNullable();
    }

    public override void Down()
    {
        Delete.Column("telegram_chat_id").FromTable("users");
    }
}