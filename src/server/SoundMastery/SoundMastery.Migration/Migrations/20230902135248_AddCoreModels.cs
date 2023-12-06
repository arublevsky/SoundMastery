using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoundMastery.Migration.Migrations
{
    /// <inheritdoc />
    public partial class AddCoreModels : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "SoundMastery",
                table: "Products",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "SoundMastery",
                table: "Courses",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                schema: "SoundMastery",
                table: "Courses",
                type: "decimal(18,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishAt",
                schema: "SoundMastery",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartAt",
                schema: "SoundMastery",
                table: "Courses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                schema: "SoundMastery",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CourseLesson",
                schema: "SoundMastery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Completed = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
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
                name: "IndividualLesson",
                schema: "SoundMastery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    StartAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Completed = table.Column<bool>(type: "bit", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualLesson", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndividualLesson_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "SoundMastery",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndividualLesson_Users_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "SoundMastery",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IndividualLesson_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalSchema: "SoundMastery",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Material",
                schema: "SoundMastery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Material", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseLessonAttendee",
                schema: "SoundMastery",
                columns: table => new
                {
                    CourseLessonId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
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
                name: "IndividualHomeAssignment",
                schema: "SoundMastery",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IndividualLessonId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualHomeAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IndividualHomeAssignment_IndividualLesson_IndividualLessonId",
                        column: x => x.IndividualLessonId,
                        principalSchema: "SoundMastery",
                        principalTable: "IndividualLesson",
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
                name: "IndividualLessonMaterial",
                schema: "SoundMastery",
                columns: table => new
                {
                    IndividualLessonId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualLessonMaterial", x => new { x.IndividualLessonId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_IndividualLessonMaterial_IndividualLesson_IndividualLessonId",
                        column: x => x.IndividualLessonId,
                        principalSchema: "SoundMastery",
                        principalTable: "IndividualLesson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndividualLessonMaterial_Material_MaterialId",
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

            migrationBuilder.CreateTable(
                name: "IndividualLessonHomeAssignmentMaterial",
                schema: "SoundMastery",
                columns: table => new
                {
                    IndividualHomeAssignmentId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualLessonHomeAssignmentMaterial", x => new { x.IndividualHomeAssignmentId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_IndividualLessonHomeAssignmentMaterial_IndividualHomeAssignment_IndividualHomeAssignmentId",
                        column: x => x.IndividualHomeAssignmentId,
                        principalSchema: "SoundMastery",
                        principalTable: "IndividualHomeAssignment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndividualLessonHomeAssignmentMaterial_Material_MaterialId",
                        column: x => x.MaterialId,
                        principalSchema: "SoundMastery",
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                schema: "SoundMastery",
                table: "Courses",
                column: "TeacherId");

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
                name: "IX_FollowingStudent_StudentId",
                schema: "SoundMastery",
                table: "FollowingStudent",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualHomeAssignment_IndividualLessonId",
                schema: "SoundMastery",
                table: "IndividualHomeAssignment",
                column: "IndividualLessonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IndividualLesson_ProductId",
                schema: "SoundMastery",
                table: "IndividualLesson",
                column: "ProductId");

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

            migrationBuilder.CreateIndex(
                name: "IX_IndividualLessonHomeAssignmentMaterial_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonHomeAssignmentMaterial",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualLessonMaterial_MaterialId",
                schema: "SoundMastery",
                table: "IndividualLessonMaterial",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Users_TeacherId",
                schema: "SoundMastery",
                table: "Courses",
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
                name: "FK_Courses_Users_TeacherId",
                schema: "SoundMastery",
                table: "Courses");

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
                name: "IndividualLessonHomeAssignmentMaterial",
                schema: "SoundMastery");

            migrationBuilder.DropTable(
                name: "IndividualLessonMaterial",
                schema: "SoundMastery");

            migrationBuilder.DropTable(
                name: "CourseLessonHomeAssignment",
                schema: "SoundMastery");

            migrationBuilder.DropTable(
                name: "IndividualHomeAssignment",
                schema: "SoundMastery");

            migrationBuilder.DropTable(
                name: "Material",
                schema: "SoundMastery");

            migrationBuilder.DropTable(
                name: "CourseLesson",
                schema: "SoundMastery");

            migrationBuilder.DropTable(
                name: "IndividualLesson",
                schema: "SoundMastery");

            migrationBuilder.DropIndex(
                name: "IX_Courses_TeacherId",
                schema: "SoundMastery",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Cost",
                schema: "SoundMastery",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "FinishAt",
                schema: "SoundMastery",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "StartAt",
                schema: "SoundMastery",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                schema: "SoundMastery",
                table: "Courses");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "SoundMastery",
                table: "Products",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "SoundMastery",
                table: "Courses",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);
        }
    }
}
