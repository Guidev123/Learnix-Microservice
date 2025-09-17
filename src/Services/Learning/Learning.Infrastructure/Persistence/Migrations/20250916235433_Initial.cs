using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "learning");

            migrationBuilder.CreateTable(
                name: "InboxMessageConsumers",
                schema: "learning",
                columns: table => new
                {
                    InboxMessageCorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(256)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxMessageConsumers", x => new { x.InboxMessageCorrelationId, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "InboxMessages",
                schema: "learning",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    Content = table.Column<string>(type: "VARCHAR(MAX)", nullable: false),
                    OccurredOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Error = table.Column<string>(type: "VARCHAR(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxMessages", x => x.CorrelationId);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessageConsumers",
                schema: "learning",
                columns: table => new
                {
                    OutboxMessageCorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "VARCHAR(256)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessageConsumers", x => new { x.OutboxMessageCorrelationId, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                schema: "learning",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "VARCHAR(200)", nullable: false),
                    Content = table.Column<string>(type: "VARCHAR(MAX)", nullable: false),
                    OccurredOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Error = table.Column<string>(type: "VARCHAR(256)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.CorrelationId);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                schema: "learning",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: false),
                    FirstName = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    SubscriptionType = table.Column<string>(type: "VARCHAR(160)", nullable: true),
                    SubscriptionExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoursesProgress",
                schema: "learning",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnrollmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "VARCHAR(160)", nullable: false),
                    OverallCompletionPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalMinutesWatched = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursesProgress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                schema: "learning",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "VARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollments_CoursesProgress_CourseId",
                        column: x => x.CourseId,
                        principalSchema: "learning",
                        principalTable: "CoursesProgress",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Enrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalSchema: "learning",
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ModulesProgress",
                schema: "learning",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CourseProgressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "VARCHAR(160)", nullable: false),
                    CompletionPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulesProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModulesProgress_CoursesProgress_CourseProgressId",
                        column: x => x.CourseProgressId,
                        principalSchema: "learning",
                        principalTable: "CoursesProgress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LessonsProgress",
                schema: "learning",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModuleProgressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "VARCHAR(160)", nullable: false),
                    CompletionPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinutesWatched = table.Column<long>(type: "bigint", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonsProgress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonsProgress_ModulesProgress_ModuleProgressId",
                        column: x => x.ModuleProgressId,
                        principalSchema: "learning",
                        principalTable: "ModulesProgress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoursesProgress_EnrollmentId",
                schema: "learning",
                table: "CoursesProgress",
                column: "EnrollmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoursesProgress_StudentId_EnrollmentId_CourseId",
                schema: "learning",
                table: "CoursesProgress",
                columns: new[] { "StudentId", "EnrollmentId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CourseId",
                schema: "learning",
                table: "Enrollments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId",
                schema: "learning",
                table: "Enrollments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonsProgress_ModuleProgressId_LessonId",
                schema: "learning",
                table: "LessonsProgress",
                columns: new[] { "ModuleProgressId", "LessonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModulesProgress_CourseProgressId_ModuleId",
                schema: "learning",
                table: "ModulesProgress",
                columns: new[] { "CourseProgressId", "ModuleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                schema: "learning",
                table: "Students",
                column: "Email",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursesProgress_Enrollments_EnrollmentId",
                schema: "learning",
                table: "CoursesProgress",
                column: "EnrollmentId",
                principalSchema: "learning",
                principalTable: "Enrollments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursesProgress_Enrollments_EnrollmentId",
                schema: "learning",
                table: "CoursesProgress");

            migrationBuilder.DropTable(
                name: "InboxMessageConsumers",
                schema: "learning");

            migrationBuilder.DropTable(
                name: "InboxMessages",
                schema: "learning");

            migrationBuilder.DropTable(
                name: "LessonsProgress",
                schema: "learning");

            migrationBuilder.DropTable(
                name: "OutboxMessageConsumers",
                schema: "learning");

            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "learning");

            migrationBuilder.DropTable(
                name: "ModulesProgress",
                schema: "learning");

            migrationBuilder.DropTable(
                name: "Enrollments",
                schema: "learning");

            migrationBuilder.DropTable(
                name: "CoursesProgress",
                schema: "learning");

            migrationBuilder.DropTable(
                name: "Students",
                schema: "learning");
        }
    }
}
