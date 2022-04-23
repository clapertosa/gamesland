using FluentMigrator;

namespace GamesLand.Infrastructure.PostgreSQL.Migrations;

[Migration(202204231003)]
public class Create_Games_Table : Migration {
    public override void Up()
    {
        Create.Table("games")
            .WithColumn("id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewSequentialId)
            .WithColumn("external_id").AsInt64().NotNullable().Unique()
            .WithColumn("slug").AsString().NotNullable().Unique()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("name_original").AsString().Nullable()
            .WithColumn("description").AsString().Nullable()
            .WithColumn("to_be_announced").AsBoolean().NotNullable()
            .WithColumn("background_image_path").AsString().Nullable()
            .WithColumn("background_image_additional_path").AsString().Nullable()
            .WithColumn("website").AsString().Nullable()
            .WithColumn("rating").AsDouble().WithDefaultValue(0)
            .WithColumn("ratings_count").AsInt64().WithDefaultValue(0)
            .WithColumn("released").AsDateTime2().Nullable()
            .WithColumn("updated").AsDateTime2().NotNullable()
            .WithColumn("created_at").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime)
            .WithColumn("updated_at").AsDateTime2().WithDefault(SystemMethods.CurrentUTCDateTime);
    }

    public override void Down()
    {
        Delete.Table("games");
    }
}