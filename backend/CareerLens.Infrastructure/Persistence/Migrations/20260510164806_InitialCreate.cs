using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerLens.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalaryRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Sector = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    YearsOfExperience = table.Column<int>(type: "integer", nullable: false),
                    NetSalary = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    TechStack = table.Column<string>(type: "text", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Plan = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RefreshToken = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    RefreshTokenExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CvAnalyses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginalFileName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    RawFileUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    ParsedRawText = table.Column<string>(type: "text", nullable: true),
                    ParsedData = table.Column<string>(type: "jsonb", nullable: true),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CvAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CvAnalyses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CareerRoadmaps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CvAnalysisId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentScore = table.Column<int>(type: "integer", nullable: false),
                    TargetPosition = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    GapAnalysis = table.Column<string>(type: "jsonb", nullable: false),
                    Recommendations = table.Column<string>(type: "jsonb", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerRoadmaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CareerRoadmaps_CvAnalyses_CvAnalysisId",
                        column: x => x.CvAnalysisId,
                        principalTable: "CvAnalyses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CareerRoadmaps_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CareerRoadmaps_CvAnalysisId",
                table: "CareerRoadmaps",
                column: "CvAnalysisId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CareerRoadmaps_UserId",
                table: "CareerRoadmaps",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CvAnalyses_Status",
                table: "CvAnalyses",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CvAnalyses_UserId",
                table: "CvAnalyses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryRecords_City",
                table: "SalaryRecords",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryRecords_Position",
                table: "SalaryRecords",
                column: "Position");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryRecords_YearsOfExperience",
                table: "SalaryRecords",
                column: "YearsOfExperience");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CareerRoadmaps");

            migrationBuilder.DropTable(
                name: "SalaryRecords");

            migrationBuilder.DropTable(
                name: "CvAnalyses");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
