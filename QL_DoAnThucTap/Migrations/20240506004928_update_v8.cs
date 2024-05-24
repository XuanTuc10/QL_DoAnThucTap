using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QL_DoAnThucTap.Migrations
{
    public partial class update_v8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_feedbacks_details_DetailId",
                table: "feedbacks");

            migrationBuilder.RenameColumn(
                name: "DetailId",
                table: "feedbacks",
                newName: "ProgressId");

            migrationBuilder.RenameIndex(
                name: "IX_feedbacks_DetailId",
                table: "feedbacks",
                newName: "IX_feedbacks_ProgressId");

            migrationBuilder.AddForeignKey(
                name: "FK_feedbacks_progresses_ProgressId",
                table: "feedbacks",
                column: "ProgressId",
                principalTable: "progresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_feedbacks_progresses_ProgressId",
                table: "feedbacks");

            migrationBuilder.RenameColumn(
                name: "ProgressId",
                table: "feedbacks",
                newName: "DetailId");

            migrationBuilder.RenameIndex(
                name: "IX_feedbacks_ProgressId",
                table: "feedbacks",
                newName: "IX_feedbacks_DetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_feedbacks_details_DetailId",
                table: "feedbacks",
                column: "DetailId",
                principalTable: "details",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
