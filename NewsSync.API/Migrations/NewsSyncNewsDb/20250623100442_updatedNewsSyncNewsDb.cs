using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsSync.API.Migrations.NewsSyncNewsDb
{
    /// <inheritdoc />
    public partial class updatedNewsSyncNewsDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleReport_AppUser_ReportedByUserId",
                table: "ArticleReport");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleReport_Articles_ArticleId",
                table: "ArticleReport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticleReport",
                table: "ArticleReport");

            migrationBuilder.RenameTable(
                name: "ArticleReport",
                newName: "ArticleReports");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleReport_ReportedByUserId",
                table: "ArticleReports",
                newName: "IX_ArticleReports_ReportedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleReport_ArticleId_ReportedByUserId",
                table: "ArticleReports",
                newName: "IX_ArticleReports_ArticleId_ReportedByUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticleReports",
                table: "ArticleReports",
                column: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleReports_AppUser_ReportedByUserId",
                table: "ArticleReports");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleReports_Articles_ArticleId",
                table: "ArticleReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticleReports",
                table: "ArticleReports");

            migrationBuilder.RenameTable(
                name: "ArticleReports",
                newName: "ArticleReport");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleReports_ReportedByUserId",
                table: "ArticleReport",
                newName: "IX_ArticleReport_ReportedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleReports_ArticleId_ReportedByUserId",
                table: "ArticleReport",
                newName: "IX_ArticleReport_ArticleId_ReportedByUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticleReport",
                table: "ArticleReport",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleReport_AppUser_ReportedByUserId",
                table: "ArticleReport",
                column: "ReportedByUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleReport_Articles_ArticleId",
                table: "ArticleReport",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
