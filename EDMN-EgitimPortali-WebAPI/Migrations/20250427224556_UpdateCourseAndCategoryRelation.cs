using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EDMN_EgitimPortali_WebAPI.Migrations
{
    public partial class UpdateCourseAndCategoryRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Course ve Category arasındaki ilişkiyi sağlamak için yeni tabloyu oluşturuyoruz.
            migrationBuilder.CreateTable(
                name: "CourseCategories",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCategories", x => new { x.CourseId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_CourseCategories_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Eğer ilişkili CourseCategories, Courses ve Categories tablolarında değişiklik yapmanız gerekiyorsa burada yapılabilir.
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // CourseCategories tablosunu siliyoruz, geri alma işlemi
            migrationBuilder.DropTable(
                name: "CourseCategories");

            // Diğer geri alma işlemleri yapılabilir.
        }
    }
}
