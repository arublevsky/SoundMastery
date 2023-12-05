using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundMastery.Migration.Migrations
{
    /// <inheritdoc />
    public partial class FixIndividualLessonsConfig3 : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IndividualLesson_Users_TeacherId",
                schema: "SoundMastery",
                table: "IndividualLesson");

            migrationBuilder.AddForeignKey(
                name: "FK_IndividualLesson_Users_TeacherId",
                schema: "SoundMastery",
                table: "IndividualLesson",
                column: "TeacherId",
                principalSchema: "SoundMastery",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IndividualLesson_Users_TeacherId",
                schema: "SoundMastery",
                table: "IndividualLesson");

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
    }
}
