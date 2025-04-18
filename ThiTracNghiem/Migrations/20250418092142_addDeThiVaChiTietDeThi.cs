using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThiTracNghiem.Migrations
{
    /// <inheritdoc />
    public partial class addDeThiVaChiTietDeThi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeThis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDeThi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThoiGianLamBai = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThaiMo = table.Column<bool>(type: "bit", nullable: false),
                    ChuDeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeThis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeThis_ChuDes_ChuDeId",
                        column: x => x.ChuDeId,
                        principalTable: "ChuDes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDeThis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeThiId = table.Column<int>(type: "int", nullable: false),
                    CauHoiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDeThis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietDeThis_CauHois_CauHoiId",
                        column: x => x.CauHoiId,
                        principalTable: "CauHois",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDeThis_DeThis_DeThiId",
                        column: x => x.DeThiId,
                        principalTable: "DeThis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDeThis_CauHoiId",
                table: "ChiTietDeThis",
                column: "CauHoiId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDeThis_DeThiId",
                table: "ChiTietDeThis",
                column: "DeThiId");

            migrationBuilder.CreateIndex(
                name: "IX_DeThis_ChuDeId",
                table: "DeThis",
                column: "ChuDeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDeThis");

            migrationBuilder.DropTable(
                name: "DeThis");
        }
    }
}
