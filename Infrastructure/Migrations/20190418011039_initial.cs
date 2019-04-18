using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "developers",
                columns: table => new
                {
                    developer_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    developer_fullname = table.Column<string>(maxLength: 150, nullable: false),
                    developer_nickname = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_developers", x => x.developer_id);
                    table.UniqueConstraint("AK_developers_developer_nickname", x => x.developer_nickname);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                columns: table => new
                {
                    project_id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    project_name = table.Column<string>(maxLength: 150, nullable: false),
                    project_description = table.Column<string>(maxLength: 450, nullable: true),
                    project_start_date = table.Column<DateTime>(nullable: false),
                    project_end_date = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.project_id);
                    table.UniqueConstraint("AK_projects_project_name", x => x.project_name);
                });

            migrationBuilder.CreateTable(
                name: "project_developer",
                columns: table => new
                {
                    project_id = table.Column<int>(nullable: false),
                    developer_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_project_developer", x => new { x.project_id, x.developer_id });
                    table.ForeignKey(
                        name: "FK_project_developer_developers_developer_id",
                        column: x => x.developer_id,
                        principalTable: "developers",
                        principalColumn: "developer_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_project_developer_projects_project_id",
                        column: x => x.project_id,
                        principalTable: "projects",
                        principalColumn: "project_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_project_developer_developer_id",
                table: "project_developer",
                column: "developer_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "project_developer");

            migrationBuilder.DropTable(
                name: "developers");

            migrationBuilder.DropTable(
                name: "projects");
        }
    }
}
