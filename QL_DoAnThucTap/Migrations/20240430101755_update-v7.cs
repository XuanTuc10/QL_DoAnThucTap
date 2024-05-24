using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QL_DoAnThucTap.Migrations
{
    public partial class updatev7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "details");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "details",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
