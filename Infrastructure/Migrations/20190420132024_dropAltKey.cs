using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class dropAltKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_projects_project_name",
                table: "projects");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_developers_developer_nickname",
                table: "developers");

            migrationBuilder.AlterColumn<string>(
                name: "developer_nickname",
                table: "developers",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "developer_nickname",
                table: "developers",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_projects_project_name",
                table: "projects",
                column: "project_name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_developers_developer_nickname",
                table: "developers",
                column: "developer_nickname");
        }
    }
}
