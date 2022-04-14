using FluentMigrator;

namespace GamesLand.Infrastructure.PostgreSQL.Migrations;

    [Migration(202204051939)]
    public class CreateUuidPgExtension: Migration {
        public override void Up()
        {
            Execute.Sql("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";");
        }

        public override void Down()
        {
            Execute.Sql("DROP EXTENSION \"uuid-ossp\";");
        }
    }
