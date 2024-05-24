using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QL_DoAnThucTap.Migrations
{
    public partial class update_v11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_confirmEmails_accounts_AccountId",
                table: "confirmEmails");

            migrationBuilder.DropColumn(
                name: "AccountrId",
                table: "confirmEmails");

            migrationBuilder.AddColumn<string>(
                name: "ShortContent",
                table: "reminders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "confirmEmails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_confirmEmails_accounts_AccountId",
                table: "confirmEmails",
                column: "AccountId",
                principalTable: "accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_confirmEmails_accounts_AccountId",
                table: "confirmEmails");

            migrationBuilder.DropColumn(
                name: "ShortContent",
                table: "reminders");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "confirmEmails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AccountrId",
                table: "confirmEmails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_confirmEmails_accounts_AccountId",
                table: "confirmEmails",
                column: "AccountId",
                principalTable: "accounts",
                principalColumn: "Id");
        }
    }
}
