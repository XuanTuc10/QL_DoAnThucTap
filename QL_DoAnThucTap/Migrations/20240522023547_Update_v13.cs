using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QL_DoAnThucTap.Migrations
{
    public partial class Update_v13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reminders_projects_ProjectId",
                table: "reminders");

            migrationBuilder.DropIndex(
                name: "IX_reminders_ProjectId",
                table: "reminders");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "reminders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "reminders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_reminders_ProjectId",
                table: "reminders",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_reminders_projects_ProjectId",
                table: "reminders",
                column: "ProjectId",
                principalTable: "projects",
                principalColumn: "Id");
        }
    }
}
