using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundMastery.Migration.Migrations
{
    /// <inheritdoc />
    public partial class FixUserRoleRelationShip : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_UserId",
                schema: "SoundMastery",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_UserId",
                schema: "SoundMastery",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "SoundMastery",
                table: "Roles");

            migrationBuilder.CreateTable(
                name: "RoleUser",
                schema: "SoundMastery",
                columns: table => new
                {
                    RolesId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesId, x.UserId });
                    table.ForeignKey(
                        name: "FK_RoleUser_Roles_RolesId",
                        column: x => x.RolesId,
                        principalSchema: "SoundMastery",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "SoundMastery",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UserId",
                schema: "SoundMastery",
                table: "RoleUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleUser",
                schema: "SoundMastery");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "SoundMastery",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserId",
                schema: "SoundMastery",
                table: "Roles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_UserId",
                schema: "SoundMastery",
                table: "Roles",
                column: "UserId",
                principalSchema: "SoundMastery",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
