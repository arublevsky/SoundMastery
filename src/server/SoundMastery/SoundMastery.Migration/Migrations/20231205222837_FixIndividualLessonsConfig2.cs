using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundMastery.Migration.Migrations
{
    /// <inheritdoc />
    public partial class FixIndividualLessonsConfig2 : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_IndividualLesson_StudentId",
                schema: "SoundMastery",
                table: "IndividualLesson",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualLesson_TeacherId",
                schema: "SoundMastery",
                table: "IndividualLesson",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_IndividualLesson_Users_StudentId",
                schema: "SoundMastery",
                table: "IndividualLesson",
                column: "StudentId",
                principalSchema: "SoundMastery",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IndividualLesson_Users_TeacherId",
                schema: "SoundMastery",
                table: "IndividualLesson",
                column: "TeacherId",
                principalSchema: "SoundMastery",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IndividualLesson_Users_StudentId",
                schema: "SoundMastery",
                table: "IndividualLesson");

            migrationBuilder.DropForeignKey(
                name: "FK_IndividualLesson_Users_TeacherId",
                schema: "SoundMastery",
                table: "IndividualLesson");

            migrationBuilder.DropIndex(
                name: "IX_IndividualLesson_StudentId",
                schema: "SoundMastery",
                table: "IndividualLesson");

            migrationBuilder.DropIndex(
                name: "IX_IndividualLesson_TeacherId",
                schema: "SoundMastery",
                table: "IndividualLesson");
        }
    }
}
