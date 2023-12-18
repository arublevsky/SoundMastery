using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundMastery.Migration.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLessonStructure : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartAt",
                schema: "SoundMastery",
                table: "IndividualLesson",
                newName: "Date");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Time",
                schema: "SoundMastery",
                table: "IndividualLesson",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                schema: "SoundMastery",
                table: "IndividualLesson");

            migrationBuilder.RenameColumn(
                name: "Date",
                schema: "SoundMastery",
                table: "IndividualLesson",
                newName: "StartAt");
        }
    }
}
