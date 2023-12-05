using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundMastery.Migration.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyDomain : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IndividualLesson_Products_ProductId",
                schema: "SoundMastery",
                table: "IndividualLesson");

            migrationBuilder.DropForeignKey(
                name: "FK_IndividualLessonHomeAssignmentMaterial_Material_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonHomeAssignmentMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_IndividualLessonMaterial_Material_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial");

            migrationBuilder.DropTable(
                name: "CourseLessonAttendee",
                schema: "SoundMastery");

            migrationBuilder.DropTable(
                name: "CourseLessonHomeAssignmentMaterial",
                schema: "SoundMastery");

            migrationBuilder.DropTable(
                name: "CourseLessonMaterial",
                schema: "SoundMastery");

            migrationBuilder.DropTable(
                name: "CourseParticipant",
                schema: "SoundMastery");

            migrationBuilder.DropTable(
                name: "FollowingStudent",
                schema: "SoundMastery");

            migrationBuilder.DropTable(
                name: "CourseLessonHomeAssignment",
                schema: "SoundMastery");

            migrationBuilder.DropTable(
                name: "CourseLesson",
                schema: "SoundMastery");

            migrationBuilder.DropTable(
                name: "Courses",
                schema: "SoundMastery");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "SoundMastery");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IndividualLessonMaterial",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IndividualLessonHomeAssignmentMaterial",
                schema: "SoundMastery",
                table: "IndividualLessonHomeAssignmentMaterial");

            migrationBuilder.DropIndex(
                name: "IX_IndividualLesson_ProductId",
                schema: "SoundMastery",
                table: "IndividualLesson");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Material",
                schema: "SoundMastery",
                table: "Material");

            migrationBuilder.RenameTable(
                name: "Material",
                schema: "SoundMastery",
                newName: "Materials",
                newSchema: "SoundMastery");

            migrationBuilder.AlterColumn<int>(
                name: "MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "IndividualLessonId",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonHomeAssignmentMaterial",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "IndividualHomeAssignmentId",
                schema: "SoundMastery",
                table: "IndividualLessonHomeAssignmentMaterial",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "SoundMastery",
                table: "IndividualLessonHomeAssignmentMaterial",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IndividualLessonMaterial",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IndividualLessonHomeAssignmentMaterial",
                schema: "SoundMastery",
                table: "IndividualLessonHomeAssignmentMaterial",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Materials",
                schema: "SoundMastery",
                table: "Materials",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_IndividualLessonHomeAssignmentMaterial_Materials_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonHomeAssignmentMaterial",
                column: "MaterialId",
                principalSchema: "SoundMastery",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IndividualLessonHomeAssignmentMaterial_Materials_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonHomeAssignmentMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_IndividualLessonMaterial_Materials_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IndividualLessonMaterial",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial");

            migrationBuilder.DropIndex(
                name: "IX_IndividualLessonMaterial_IndividualLessonId_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IndividualLessonHomeAssignmentMaterial",
                schema: "SoundMastery",
                table: "IndividualLessonHomeAssignmentMaterial");

            migrationBuilder.DropIndex(
                name: "IX_IndividualLessonHomeAssignmentMaterial_IndividualHomeAssignmentId_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonHomeAssignmentMaterial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Materials",
                schema: "SoundMastery",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "SoundMastery",
                table: "IndividualLessonHomeAssignmentMaterial");

            migrationBuilder.RenameTable(
                name: "Materials",
                schema: "SoundMastery",
                newName: "Material",
                newSchema: "SoundMastery");

            migrationBuilder.AlterColumn<int>(
                name: "MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "IndividualLessonId",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<int>(
                name: "MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonHomeAssignmentMaterial",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "IndividualHomeAssignmentId",
                schema: "SoundMastery",
                table: "IndividualLessonHomeAssignmentMaterial",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IndividualLessonMaterial",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial",
                columns: new[] { "IndividualLessonId", "MaterialId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_IndividualLessonHomeAssignmentMaterial",
                schema: "SoundMastery",
                table: "IndividualLessonHomeAssignmentMaterial",
                columns: new[] { "IndividualHomeAssignmentId", "MaterialId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Material",
                schema: "SoundMastery",
                table: "Material",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FollowingStudent",
                schema: "SoundMastery",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowingStudent", x => new { x.UserId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_FollowingStudent_Users_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "SoundMastery",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FollowingStudent_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "SoundMastery",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "SoundMastery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                schema: "SoundMastery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    FinishAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SoundMastery",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "SoundMastery",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseLesson",
                schema: "SoundMastery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    Completed = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLesson", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseLesson_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "SoundMastery",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseLesson_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "SoundMastery",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseParticipant",
                schema: "SoundMastery",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseParticipant", x => new { x.UserId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_CourseParticipant_Courses_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "SoundMastery",
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseParticipant_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "SoundMastery",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseLessonAttendee",
                schema: "SoundMastery",
                columns: table => new
                {
                    CourseLessonId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLessonAttendee", x => new { x.UserId, x.CourseLessonId });
                    table.ForeignKey(
                        name: "FK_CourseLessonAttendee_CourseLesson_CourseLessonId",
                        column: x => x.CourseLessonId,
                        principalSchema: "SoundMastery",
                        principalTable: "CourseLesson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseLessonAttendee_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "SoundMastery",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseLessonHomeAssignment",
                schema: "SoundMastery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseLessonId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLessonHomeAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseLessonHomeAssignment_CourseLesson_CourseLessonId",
                        column: x => x.CourseLessonId,
                        principalSchema: "SoundMastery",
                        principalTable: "CourseLesson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseLessonMaterial",
                schema: "SoundMastery",
                columns: table => new
                {
                    CourseLessonId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLessonMaterial", x => new { x.CourseLessonId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_CourseLessonMaterial_CourseLesson_CourseLessonId",
                        column: x => x.CourseLessonId,
                        principalSchema: "SoundMastery",
                        principalTable: "CourseLesson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseLessonMaterial_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalSchema: "SoundMastery",
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseLessonHomeAssignmentMaterial",
                schema: "SoundMastery",
                columns: table => new
                {
                    CourseHomeAssignmentId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLessonHomeAssignmentMaterial", x => new { x.CourseHomeAssignmentId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_CourseLessonHomeAssignmentMaterial_CourseLessonHomeAssignment_CourseHomeAssignmentId",
                        column: x => x.CourseHomeAssignmentId,
                        principalSchema: "SoundMastery",
                        principalTable: "CourseLessonHomeAssignment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseLessonHomeAssignmentMaterial_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalSchema: "SoundMastery",
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IndividualLesson_ProductId",
                schema: "SoundMastery",
                table: "IndividualLesson",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLesson_CourseId",
                schema: "SoundMastery",
                table: "CourseLesson",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLesson_TeacherId",
                schema: "SoundMastery",
                table: "CourseLesson",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessonAttendee_CourseLessonId",
                schema: "SoundMastery",
                table: "CourseLessonAttendee",
                column: "CourseLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessonHomeAssignment_CourseLessonId",
                schema: "SoundMastery",
                table: "CourseLessonHomeAssignment",
                column: "CourseLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessonHomeAssignmentMaterial_MaterialId",
                schema: "SoundMastery",
                table: "CourseLessonHomeAssignmentMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLessonMaterial_MaterialId",
                schema: "SoundMastery",
                table: "CourseLessonMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseParticipant_CourseId",
                schema: "SoundMastery",
                table: "CourseParticipant",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ProductId",
                schema: "SoundMastery",
                table: "Courses",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                schema: "SoundMastery",
                table: "Courses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_FollowingStudent_StudentId",
                schema: "SoundMastery",
                table: "FollowingStudent",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_IndividualLesson_Products_ProductId",
                schema: "SoundMastery",
                table: "IndividualLesson",
                column: "ProductId",
                principalSchema: "SoundMastery",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IndividualLessonHomeAssignmentMaterial_Material_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonHomeAssignmentMaterial",
                column: "MaterialId",
                principalSchema: "SoundMastery",
                principalTable: "Material",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IndividualLessonMaterial_Material_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial",
                column: "MaterialId",
                principalSchema: "SoundMastery",
                principalTable: "Material",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
