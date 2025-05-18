using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThiTracNghiem.Migrations
{
    /// <inheritdoc />
    public partial class AddLienHe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LienHes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTaiKhoan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    NgayGui = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThaiXuLy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LienHes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LienHes_TaiKhoans_TenTaiKhoan",
                        column: x => x.TenTaiKhoan,
                        principalTable: "TaiKhoans",
                        principalColumn: "TenTaiKhoan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LienHes_TenTaiKhoan",
                table: "LienHes",
                column: "TenTaiKhoan");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LienHes");
        }
    }
}
