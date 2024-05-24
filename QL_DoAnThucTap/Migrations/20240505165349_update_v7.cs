using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QL_DoAnThucTap.Migrations
{
    public partial class update_v7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "feedbacks");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "feedbacks",
                newName: "FeedBack");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FeedBack",
                table: "feedbacks",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "feedbacks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
