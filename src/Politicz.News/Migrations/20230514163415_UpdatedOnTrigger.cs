#nullable disable

namespace Politicz.News.Migrations;

/// <inheritdoc />
public partial class UpdatedOnTrigger : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.AddColumn<DateTime>(
            name: "UpdatedOn",
            table: "News",
            type: "datetime2",
            nullable: true);

        _ = migrationBuilder.Sql(
            @"
CREATE TRIGGER UpdateNewsTrigger
ON News
AFTER UPDATE
AS
BEGIN
    UPDATE News
    SET UpdatedOn = GETUTCDATE()
    WHERE Id IN (SELECT Id FROM inserted)
END
            ");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.DropColumn(
            name: "UpdatedOn",
            table: "News");

        _ = migrationBuilder.Sql("DROP TRIGGER UpdateNewsTrigger");
    }
}
