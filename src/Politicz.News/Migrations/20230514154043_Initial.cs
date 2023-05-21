#nullable disable

namespace Politicz.News.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        _ = migrationBuilder.CreateTable(
            name: "News",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Heading = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ExternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
            },
            constraints: table
                => table.PrimaryKey("PK_News", x => x.Id));

        _ = migrationBuilder.CreateIndex(
            name: "IX_News_ExternalId",
            table: "News",
            column: "ExternalId")
            .Annotation("SqlServer:Clustered", false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
        => _ = migrationBuilder.DropTable(
            name: "News");
}
