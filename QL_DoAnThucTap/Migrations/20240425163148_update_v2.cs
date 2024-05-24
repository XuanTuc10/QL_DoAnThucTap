using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QL_DoAnThucTap.Migrations
{
    public partial class update_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Createtime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DetailId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_feedbacks_details_DetailId",
                        column: x => x.DetailId,
                        principalTable: "details",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_feedbacks_DetailId",
                table: "feedbacks",
                column: "DetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feedbacks");
        }
    }
}
