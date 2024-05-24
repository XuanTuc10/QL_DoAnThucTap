using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QL_DoAnThucTap.Migrations
{
    public partial class updatev6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "details");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "details",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "details");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "details",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
