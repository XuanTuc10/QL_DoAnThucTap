using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QL_DoAnThucTap.Migrations
{
    public partial class update_v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reminders_teachers_TeacherId",
                table: "reminders");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "reminders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "reminders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_reminders_ProjectId",
                table: "reminders",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_reminders_projects_ProjectId",
                table: "reminders",
                column: "ProjectId",
                principalTable: "projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reminders_teachers_TeacherId",
                table: "reminders",
                column: "TeacherId",
                principalTable: "teachers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reminders_projects_ProjectId",
                table: "reminders");

            migrationBuilder.DropForeignKey(
                name: "FK_reminders_teachers_TeacherId",
                table: "reminders");

            migrationBuilder.DropIndex(
                name: "IX_reminders_ProjectId",
                table: "reminders");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "reminders");

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "reminders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_reminders_teachers_TeacherId",
                table: "reminders",
                column: "TeacherId",
                principalTable: "teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
