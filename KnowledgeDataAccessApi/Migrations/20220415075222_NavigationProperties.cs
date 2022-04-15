using Microsoft.EntityFrameworkCore.Migrations;

namespace KnowledgeDataAccessApi.Migrations
{
    public partial class NavigationProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Themes_ThemeId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_Themes_Catalogs_CatalogId",
                table: "Themes");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Themes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CatalogId",
                table: "Themes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "IssuesUnderStudy",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ThemeId",
                table: "Issues",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Question",
                table: "Issues",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Catalogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IssuesUnderStudy_IssueId",
                table: "IssuesUnderStudy",
                column: "IssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Themes_ThemeId",
                table: "Issues",
                column: "ThemeId",
                principalTable: "Themes",
                principalColumn: "ThemeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuesUnderStudy_Issues_IssueId",
                table: "IssuesUnderStudy",
                column: "IssueId",
                principalTable: "Issues",
                principalColumn: "IssueId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Themes_Catalogs_CatalogId",
                table: "Themes",
                column: "CatalogId",
                principalTable: "Catalogs",
                principalColumn: "CatalogId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Themes_ThemeId",
                table: "Issues");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuesUnderStudy_Issues_IssueId",
                table: "IssuesUnderStudy");

            migrationBuilder.DropForeignKey(
                name: "FK_Themes_Catalogs_CatalogId",
                table: "Themes");

            migrationBuilder.DropIndex(
                name: "IX_IssuesUnderStudy_IssueId",
                table: "IssuesUnderStudy");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Themes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CatalogId",
                table: "Themes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "IssuesUnderStudy",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ThemeId",
                table: "Issues",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Question",
                table: "Issues",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Catalogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Themes_ThemeId",
                table: "Issues",
                column: "ThemeId",
                principalTable: "Themes",
                principalColumn: "ThemeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Themes_Catalogs_CatalogId",
                table: "Themes",
                column: "CatalogId",
                principalTable: "Catalogs",
                principalColumn: "CatalogId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
