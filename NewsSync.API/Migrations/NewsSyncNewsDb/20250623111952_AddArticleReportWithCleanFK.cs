using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsSync.API.Migrations.NewsSyncNewsDb
{
    /// <inheritdoc />
    public partial class AddArticleReportWithCleanFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleReports_AppUser_ReportedByUserId",
                table: "ArticleReports");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleReports_Articles_ArticleId",
                table: "ArticleReports");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropIndex(
                name: "IX_ArticleReports_ArticleId_ReportedByUserId",
                table: "ArticleReports");

            migrationBuilder.DropIndex(
                name: "IX_ArticleReports_ReportedByUserId",
                table: "ArticleReports");

            migrationBuilder.AlterColumn<string>(
                name: "ReportedByUserId",
                table: "ArticleReports",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleReports_ArticleId",
                table: "ArticleReports",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleReports_Articles_ArticleId",
                table: "ArticleReports",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleReports_Articles_ArticleId",
                table: "ArticleReports");

            migrationBuilder.DropIndex(
                name: "IX_ArticleReports_ArticleId",
                table: "ArticleReports");

            migrationBuilder.AlterColumn<string>(
                name: "ReportedByUserId",
                table: "ArticleReports",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleReports_ArticleId_ReportedByUserId",
                table: "ArticleReports",
                columns: new[] { "ArticleId", "ReportedByUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArticleReports_ReportedByUserId",
                table: "ArticleReports",
                column: "ReportedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleReports_AppUser_ReportedByUserId",
                table: "ArticleReports",
                column: "ReportedByUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleReports_Articles_ArticleId",
                table: "ArticleReports",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
