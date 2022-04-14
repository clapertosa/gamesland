using FluentMigrator;

namespace GamesLand.Infrastructure.PostgreSQL.Migrations;

[Migration(202204061910)]
public class CreateUserstable : Migration {
    public override void Up()
    {
        Create.Table("users")
            .WithColumn("id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewSequentialId)
            .WithColumn("first_name").AsString().Nullable()
            .WithColumn("last_name").AsString().Nullable()
            .WithColumn("email").AsString().Unique().NotNullable()
            .WithColumn("password").AsString().NotNullable()
            .WithColumn("created_at").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("updated_at").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime);;
    }

    public override void Down()
    {
        Delete.Table("users");
    }
}