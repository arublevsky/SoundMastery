using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundMastery.Migration.Migrations
{
    /// <inheritdoc />
    public partial class AddFilesAndMaterials : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IndividualLessonMaterial_Materials_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial");

            migrationBuilder.DropTable(
                name: "IndividualLessonHomeAssignmentMaterial",
                schema: "SoundMastery");

            migrationBuilder.DropIndex(
                name: "IX_IndividualLessonMaterial_IndividualLessonId_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial");

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                schema: "SoundMastery",
                table: "Materials",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "SoundMastery",
                table: "Materials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Files",
                schema: "SoundMastery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materials_FileId",
                schema: "SoundMastery",
                table: "Materials",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualLessonMaterial_IndividualLessonId",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial",
                column: "IndividualLessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_IndividualLessonMaterial_Materials_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial",
                column: "MaterialId",
                principalSchema: "SoundMastery",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Files_FileId",
                schema: "SoundMastery",
                table: "Materials",
                column: "FileId",
                principalSchema: "SoundMastery",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IndividualLessonMaterial_Materials_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Files_FileId",
                schema: "SoundMastery",
                table: "Materials");

            migrationBuilder.DropTable(
                name: "Files",
                schema: "SoundMastery");

            migrationBuilder.DropIndex(
                name: "IX_Materials_FileId",
                schema: "SoundMastery",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_IndividualLessonMaterial_IndividualLessonId",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial");

            migrationBuilder.DropColumn(
                name: "FileId",
                schema: "SoundMastery",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "SoundMastery",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial");

            migrationBuilder.CreateTable(
                name: "IndividualLessonHomeAssignmentMaterial",
                schema: "SoundMastery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndividualHomeAssignmentId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualLessonHomeAssignmentMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndividualLessonHomeAssignmentMaterial_IndividualHomeAssignment_IndividualHomeAssignmentId",
                        column: x => x.IndividualHomeAssignmentId,
                        principalSchema: "SoundMastery",
                        principalTable: "IndividualHomeAssignment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndividualLessonHomeAssignmentMaterial_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalSchema: "SoundMastery",
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndividualLessonMaterial_IndividualLessonId_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial",
                columns: new[] { "IndividualLessonId", "MaterialId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndividualLessonHomeAssignmentMaterial_IndividualHomeAssignmentId_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonHomeAssignmentMaterial",
                columns: new[] { "IndividualHomeAssignmentId", "MaterialId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndividualLessonHomeAssignmentMaterial_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonHomeAssignmentMaterial",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_IndividualLessonMaterial_Materials_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial",
                column: "MaterialId",
                principalSchema: "SoundMastery",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
