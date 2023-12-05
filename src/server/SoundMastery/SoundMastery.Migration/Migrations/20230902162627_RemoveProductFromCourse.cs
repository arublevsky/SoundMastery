using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundMastery.Migration.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProductFromCourse : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Products_ProductId",
                schema: "SoundMastery",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_ProductId",
                schema: "SoundMastery",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ProductId",
                schema: "SoundMastery",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "SoundMastery",
                table: "CourseLessonAttendee",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                schema: "SoundMastery",
                table: "CourseLessonAttendee");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                schema: "SoundMastery",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ProductId",
                schema: "SoundMastery",
                table: "Courses",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Products_ProductId",
                schema: "SoundMastery",
                table: "Courses",
                column: "ProductId",
                principalSchema: "SoundMastery",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
