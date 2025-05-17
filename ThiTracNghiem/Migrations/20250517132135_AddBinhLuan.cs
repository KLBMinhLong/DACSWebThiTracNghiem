using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThiTracNghiem.Migrations
{
    /// <inheritdoc />
    public partial class AddBinhLuan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BinhLuans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTaiKhoan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeThiId = table.Column<int>(type: "int", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ThoiGian = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BinhLuans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BinhLuans_DeThis_DeThiId",
                        column: x => x.DeThiId,
                        principalTable: "DeThis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BinhLuans_TaiKhoans_TenTaiKhoan",
                        column: x => x.TenTaiKhoan,
                        principalTable: "TaiKhoans",
                        principalColumn: "TenTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BinhLuans_DeThiId",
                table: "BinhLuans",
                column: "DeThiId");

            migrationBuilder.CreateIndex(
                name: "IX_BinhLuans_TenTaiKhoan",
                table: "BinhLuans",
                column: "TenTaiKhoan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BinhLuans");
        }
    }
}
