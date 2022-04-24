using System.Data;
using FluentMigrator;

namespace GamesLand.Infrastructure.PostgreSQL.Migrations;

[Migration(202204231600)]
public class Create_Platforms_Table : Migration {
    public override void Up()
    {
        Create.Table("platforms")
            .WithColumn("id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewSequentialId)
            .WithColumn("external_id").AsInt64().NotNullable().Unique()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("created_at").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("updated_at").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime);
    }

    public override void Down()
    {
        Delete.Table("platforms");
    }
}