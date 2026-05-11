using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations.Migrations
{
    /// <inheritdoc />
    public partial class addarticlenavigationtosource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleCategory_Articles_NewsId",
                table: "ArticleCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticleCategory",
                table: "ArticleCategory");

            migrationBuilder.DropIndex(
                name: "IX_ArticleCategory_NewsId",
                table: "ArticleCategory");

            migrationBuilder.RenameColumn(
                name: "NewsId",
                table: "ArticleCategory",
                newName: "ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticleCategory",
                table: "ArticleCategory",
                columns: new[] { "ArticleId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleCategory_CategoryId",
                table: "ArticleCategory",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleCategory_Articles_ArticleId",
                table: "ArticleCategory",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleCategory_Articles_ArticleId",
                table: "ArticleCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticleCategory",
                table: "ArticleCategory");

            migrationBuilder.DropIndex(
                name: "IX_ArticleCategory_CategoryId",
                table: "ArticleCategory");

            migrationBuilder.RenameColumn(
                name: "ArticleId",
                table: "ArticleCategory",
                newName: "NewsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticleCategory",
                table: "ArticleCategory",
                columns: new[] { "CategoryId", "NewsId" });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleCategory_NewsId",
                table: "ArticleCategory",
                column: "NewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleCategory_Articles_NewsId",
                table: "ArticleCategory",
                column: "NewsId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
