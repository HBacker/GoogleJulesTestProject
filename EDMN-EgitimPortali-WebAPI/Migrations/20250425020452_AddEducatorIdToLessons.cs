using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDMN_EgitimPortali_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddEducatorIdToLessons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
               name: "EducatorId",
               table: "Lessons",
               type: "nvarchar(450)", 
               nullable: false); 

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_EducatorId",
                table: "Lessons",
                column: "EducatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_AspNetUsers_EducatorId",
                table: "Lessons",
                column: "EducatorId",
                principalTable: "AspNetUsers", 
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
             name: "FK_Lessons_AspNetUsers_EducatorId",
             table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_EducatorId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "EducatorId",
                table: "Lessons");
        }
    }
}
