using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDMN_EgitimPortali_WebAPI.Migrations
{
    /// <inheritdoc />
    using Microsoft.EntityFrameworkCore.Migrations; 
    using Microsoft.EntityFrameworkCore;

#nullable disable

    public partial class AddEducatorIdToCourses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EducatorId",
                table: "Courses",
                type: "nvarchar(450)", 
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_EducatorId",
                table: "Courses",
                column: "EducatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_AspNetUsers_EducatorId",
                table: "Courses",
                column: "EducatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict); 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_AspNetUsers_EducatorId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_EducatorId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "EducatorId",
                table: "Courses");
        }
    }
}

