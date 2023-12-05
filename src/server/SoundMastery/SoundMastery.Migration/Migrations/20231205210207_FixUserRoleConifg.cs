using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundMastery.Migration.Migrations
{
    /// <inheritdoc />
    public partial class FixUserRoleConifg : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_Users_UserId",
                schema: "SoundMastery",
                table: "RoleUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "SoundMastery",
                table: "RoleUser",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleUser_UserId",
                schema: "SoundMastery",
                table: "RoleUser",
                newName: "IX_RoleUser_UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_Users_UsersId",
                schema: "SoundMastery",
                table: "RoleUser",
                column: "UsersId",
                principalSchema: "SoundMastery",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleUser_Users_UsersId",
                schema: "SoundMastery",
                table: "RoleUser");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                schema: "SoundMastery",
                table: "RoleUser",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleUser_UsersId",
                schema: "SoundMastery",
                table: "RoleUser",
                newName: "IX_RoleUser_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleUser_Users_UserId",
                schema: "SoundMastery",
                table: "RoleUser",
                column: "UserId",
                principalSchema: "SoundMastery",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
