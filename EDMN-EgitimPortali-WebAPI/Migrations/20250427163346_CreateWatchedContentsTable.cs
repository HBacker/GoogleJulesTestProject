using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDMN_EgitimPortali_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreateWatchedContentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
    name: "WatchedContents",
    columns: table => new
    {
        Id = table.Column<int>(type: "int", nullable: false)
            .Annotation("SqlServer:Identity", "1, 1"),
        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
        LessonId = table.Column<int>(type: "int", nullable: false),
        WatchedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
    },
    constraints: table =>
    {
        table.PrimaryKey("PK_WatchedContents", x => x.Id);
        table.ForeignKey(
            name: "FK_WatchedContents_AspNetUsers_UserId",
            column: x => x.UserId,
            principalTable: "AspNetUsers",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
        table.ForeignKey(
            name: "FK_WatchedContents_Lessons_LessonId",
            column: x => x.LessonId,
            principalTable: "Lessons",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchedContents");
        }

    }
}
